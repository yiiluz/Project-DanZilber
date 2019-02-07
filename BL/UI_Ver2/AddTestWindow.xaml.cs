using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
namespace UI_Ver2
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class AddTestWindow : Window
    {
        Test test = new Test();
        Trainee trainee = null;
        List<Test> lst = new List<Test>();
        //List<Tester> lst;
        BackgroundWorker worker = new BackgroundWorker();
        public AddTestWindow(Trainee trainee = null)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            //add test only from tomorrow
            test.DateOfTest = DateTime.Today.AddDays(1);
            test.HourOfTest = 9;
            this.DataContext = test;
            CmbBx_City.ItemsSource = MainWindow.cities;
            CmbBx_Street.IsEnabled = false;
            DatePicker_DateOfTest_ByHour.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now));
            DatePicker_DateOfTest_ByDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now));
            CombBx_TestsListByHour.IsEnabled = false;
            CombBx_TestsListByDate.IsEnabled = false;
            if (trainee != null)
            {
                TxtBx_ID.Text = trainee.Id;
                TxtBx_ID.IsEnabled = false;
                CmbBx_City.Text = trainee.City;
                CmbBx_Street.Text = trainee.Street;
                TxtBx_BuildNum.Text = trainee.BuildingNumber.ToString();
            }
            worker.DoWork += AddChosenTestThreadFunc;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
        }



        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to cancel? The Test has'nt Added.", "Cancel Request", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
                Close();
        }
        private void TxtBx_ID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtBx_ID.Text.All(char.IsDigit) || (TxtBx_ID.Text.Length != 9))
                TxtBx_ID.Background = Brushes.Red;
            else
                TxtBx_ID.BorderBrush = Brushes.Green;
        }
        private void TxtBx_ID_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_ID.Background = Brushes.White;
            TxtBx_ID.BorderBrush = Brushes.Gray;
        }
        private void TxtBx_BuildNum_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtBx_BuildNum.Text.All(char.IsDigit) || (TxtBx_BuildNum.Text.Length == 0))
                TxtBx_BuildNum.Background = Brushes.Red;
            else
                TxtBx_BuildNum.BorderBrush = Brushes.Green;
        }
        private void TxtBx_BuildNum_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_BuildNum.Background = Brushes.White;
            TxtBx_BuildNum.BorderBrush = Brushes.Gray;
        }
        private void TxtBx_HourByDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtBx_HourByDate.Text.All(char.IsDigit) || (int.Parse(TxtBx_HourByDate.Text) > 14 || (int.Parse(TxtBx_HourByDate.Text)) < 9))
                TxtBx_HourByDate.Background = Brushes.Red;
            else
                TxtBx_HourByDate.BorderBrush = Brushes.Green;
        }
        private void TxtBx_HourByDate_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_HourByDate.Background = Brushes.White;
            TxtBx_HourByDate.BorderBrush = Brushes.Gray;
        }

        private void Button_AddChosenTest(object sender, RoutedEventArgs e)
        {
            try
            {
                string serialOfTest = MainWindow.bl.AddTest(CombBx_TestsListByDate.SelectedItem as Test);
                MessageBox.Show("Test added successfuly. Test ID: " + serialOfTest, "Add Status", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                var result = MessageBox.Show("Internal ERROR: " + ex.Message + ". Do you want to try agein?", "ERROR", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.No)
                {
                    MessageBox.Show("The test has'nt Added.", "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    return;
                }
            }
        }
        private void Button_Click_GetTestsListByDate(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy)
            {
                return;
            }

            if (
            (TxtBx_HourByDate.Text.All(char.IsDigit) && (int.Parse(TxtBx_HourByDate.Text) < 15 && (int.Parse(TxtBx_HourByDate.Text)) >= 9)) &&
            (TxtBx_BuildNum.Text.All(char.IsDigit) && (TxtBx_BuildNum.Text.Length != 0)) &&
            (TxtBx_ID.Text.All(char.IsDigit) && (TxtBx_ID.Text.Length == 9))
            )
            {
                if (!MainWindow.cities.Exists(x => x == (string)CmbBx_City.SelectedItem) || !MainWindow.streetsGroupedByCity.Find(x => x.Key == (string)CmbBx_City.SelectedItem).ToList().Exists(x => x == (string)CmbBx_Street.SelectedItem))
                {
                    MessageBox.Show("Address input was wrong.", "Wrong Input", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    trainee = MainWindow.bl.GetTraineeByID(TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    var result = MessageBox.Show("Trainee ID does Not Exist. Do you want to try agein?", "ID Not exist", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.No)
                    {
                        MessageBox.Show("The test has'nt Added." + ex.Message, "Operation Canceld", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else
                    {

                        return;
                    }
                }
                test.CarType = trainee.CurrCarType;
                if (!worker.IsBusy)
                {
                    worker.RunWorkerAsync("Add Test By Date");
                    AddTestProgressBarByDate.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("The input is not correct. The bad inputs marked at Red Unless you hav'nt update nothing.", "Wrong Input", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_GetTestsListByHour(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy)
            {
                return;
            }

            if (
                (TxtBx_HourByHour.Text.All(char.IsDigit) && (int.Parse(TxtBx_HourByHour.Text) < 15 && (int.Parse(TxtBx_HourByHour.Text)) >= 9)) &&
                (TxtBx_BuildNum.Text.All(char.IsDigit) && (TxtBx_BuildNum.Text.Length != 0)) &&
                (TxtBx_ID.Text.All(char.IsDigit) && (TxtBx_ID.Text.Length == 9))
                )
            {
                if (!MainWindow.cities.Exists(x => x == (string)CmbBx_City.SelectedItem) || !MainWindow.streetsGroupedByCity.Find(x => x.Key == (string)CmbBx_City.SelectedItem).ToList().Exists(x => x == (string)CmbBx_Street.SelectedItem))
                {
                    MessageBox.Show("Address input was wrong.", "Wrong Input", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Trainee trainee = null;
                try
                {
                    trainee = MainWindow.bl.GetTraineeByID(TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    var result = MessageBox.Show("Trainee ID does Not Exist. Do you want to try agein?", "ID Not exist", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.No)
                    {
                        MessageBox.Show("The test has'nt Added." + ex.Message, "Operation Canceld", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else
                    {
                        return;
                    }
                }
                test.CarType = trainee.CurrCarType;
                if (!worker.IsBusy)
                {
                    worker.RunWorkerAsync("Add Test By Hour");
                    AddTestProgressBarByHour.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("The input is not correct. The bad inputs marked at Red Unless you hav'nt update nothing.", "Wrong Input", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TxtBx_HourByHour_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtBx_HourByHour.Text.All(char.IsDigit) || (int.Parse(TxtBx_HourByHour.Text) > 14 || (int.Parse(TxtBx_HourByHour.Text)) < 9))
                TxtBx_HourByHour.Background = Brushes.Red;
            else
                TxtBx_HourByHour.BorderBrush = Brushes.Green;
        }

        private void TxtBx_HourByHour_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_HourByHour.Background = Brushes.White;
            TxtBx_HourByHour.BorderBrush = Brushes.Gray;
        }

        //private void Button_AddByHour_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        string serialOfTest = MainWindow.bl.AddTest((CombBx_TestsListByHour.SelectedItem) as Test);
        //        MessageBox.Show("Test added successfuly. Test ID: " + serialOfTest, "Add Status", MessageBoxButton.OK, MessageBoxImage.Information);
        //        Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        var result = MessageBox.Show("Internal ERROR: " + ex.Message + ". Do you want to try agein?", "ERROR", MessageBoxButton.YesNo, MessageBoxImage.Error);
        //        if (result == MessageBoxResult.No)
        //        {
        //            MessageBox.Show("The test has'nt Added.", "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Information);
        //            Close();
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}
        private void CmbBx_City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CmbBx_Street.IsEnabled = true;
            CmbBx_Street.ItemsSource = MainWindow.streetsGroupedByCity.Find(x => x.Key == (string)CmbBx_City.SelectedItem);
        }

        private void AddChosenTestThreadFunc(Object sender, DoWorkEventArgs e)
        {

            switch ((string)e.Argument)
            {
                case "Add Test By Date":
                    try
                    {
                        lst = MainWindow.bl.GetOptionalTestsByDate(this.test, trainee);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        MessageBox.Show("Can't find Test time. " + ex.Message + "\nYou can try again.", "Failed", MessageBoxButton.OK, MessageBoxImage.Hand);
                        worker.CancelAsync();
                        break;
                    }
                    if (lst.Count == 0)
                    {
                        MessageBox.Show("There is no Availiable test on this Date. Try another", "OOPS", MessageBoxButton.OK, MessageBoxImage.Information);
                        worker.CancelAsync();
                    }
                    else
                    {
                        e.Result = "Add Test By Date";
                    }
                    break;

                case "Add Test By Hour":
                    try
                    {
                        lst = MainWindow.bl.GetOptionalTestsByHour(this.test, trainee);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        MessageBox.Show("Can't find Test time. " + ex.Message + "\nYou can try again.", "Failed", MessageBoxButton.OK, MessageBoxImage.Hand);
                        worker.CancelAsync();
                        break;
                    }
                    if (lst.Count == 0)
                    {
                        MessageBox.Show("There is no Availiable test on this Hour. Try another", "OOPS", MessageBoxButton.OK, MessageBoxImage.Information);
                        worker.CancelAsync();
                    }
                    else
                    {
                        e.Result = "Add Test By Hour";
                    }
                    break;
            }
            if (worker.CancellationPending)
                e.Cancel = true;
        }
        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                AddTestProgressBarByDate.Visibility = Visibility.Hidden;
                AddTestProgressBarByHour.Visibility = Visibility.Hidden;
            }
            else if (e.Error != null)
            {
                AddTestProgressBarByDate.Visibility = Visibility.Hidden;
                AddTestProgressBarByHour.Visibility = Visibility.Hidden;
                MessageBox.Show("Can't find test. Check your internet Conection.", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);

            }
            else
            {
                switch ((string)e.Result)
                {
                    case "Add Test By Hour":
                        CombBx_TestsListByHour.ItemsSource = lst;
                        CombBx_TestsListByHour.SelectedItem = lst[0];
                        CombBx_TestsListByHour.IsEnabled = true;
                        AddTestProgressBarByHour.Visibility = Visibility.Hidden;
                        break;
                    case "Add Test By Date":
                        CombBx_TestsListByDate.ItemsSource = lst;
                        CombBx_TestsListByDate.SelectedItem = lst[0];
                        CombBx_TestsListByDate.IsEnabled = true;
                        AddTestProgressBarByDate.Visibility = Visibility.Hidden;
                        break;
                }

            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
