using System;
using System.Collections.Generic;
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
        //List<Tester> lst;
        public AddTestWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            //add test only from tomorrow
            test.DateOfTest = DateTime.Today.AddDays(1);
            test.HourOfTest = 9;
            this.DataContext = test;
            
            DatePicker_DateOfTest_ByHour.BlackoutDates.Add(new CalendarDateRange( DateTime.MinValue, DateTime.Now));
            DatePicker_DateOfTest_ByDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now));
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
        private void TxtBx_City_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_City.Text.Length == 0 || !TxtBx_City.Text.All(x => x == ' ' || char.IsLetter(x)))
                TxtBx_City.Background = Brushes.Red;
            else
                TxtBx_City.BorderBrush = Brushes.Green;
        }
        private void TxtBx_City_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_City.Background = Brushes.White;
            TxtBx_City.BorderBrush = Brushes.Gray;
        }
        private void TxtBx_Street_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_Street.Text.Length == 0 || !TxtBx_Street.Text.All(x => x == ' ' || char.IsLetter(x)))
                TxtBx_Street.Background = Brushes.Red;
            else
                TxtBx_Street.BorderBrush = Brushes.Green;
        }
        private void TxtBx_Street_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_Street.Background = Brushes.White;
            TxtBx_Street.BorderBrush = Brushes.Gray;
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

        private void Button_Click_AddByDate(object sender, RoutedEventArgs e)
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
            
            if (
                (TxtBx_HourByDate.Text.All(char.IsDigit) && (int.Parse(TxtBx_HourByDate.Text) < 15 && (int.Parse(TxtBx_HourByDate.Text)) >= 9)) &&
                (TxtBx_BuildNum.Text.All(char.IsDigit) && (TxtBx_BuildNum.Text.Length != 0)) &&
                (TxtBx_Street.Text.Length != 0 || TxtBx_Street.Text.All(x => x == ' ' || char.IsLetter(x))) &&
                (TxtBx_City.Text.Length != 0 || TxtBx_City.Text.All(x => x == ' ' || char.IsLetter(x))) &&
                (TxtBx_ID.Text.All(char.IsDigit) && (TxtBx_ID.Text.Length == 9))
                )
            {
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
                var lst = MainWindow.bl.GetOptionalTestsByDate(this.test, trainee);
                if (lst.Count == 0)
                {
                    MessageBox.Show("There is no Availiable test on this Date. Try another", "OOPS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    CombBx_TestsListByDate.ItemsSource = lst;
                    CombBx_TestsListByDate.SelectedItem = lst[0];
                    CombBx_TestsListByDate.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("The input is not correct. The bad inputs marked at Red Unless you hav'nt update nothing.", "Wrong Input", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_GetTestsListByHour(object sender, RoutedEventArgs e)
        {
            if (
                (TxtBx_HourByHour.Text.All(char.IsDigit) && (int.Parse(TxtBx_HourByHour.Text) < 15 && (int.Parse(TxtBx_HourByHour.Text)) >= 9)) &&
                (TxtBx_BuildNum.Text.All(char.IsDigit) && (TxtBx_BuildNum.Text.Length != 0)) &&
                (TxtBx_Street.Text.Length != 0 || TxtBx_Street.Text.All(x => x == ' ' || char.IsLetter(x))) &&
                (TxtBx_City.Text.Length != 0 || TxtBx_City.Text.All(x => x == ' ' || char.IsLetter(x))) &&
                (TxtBx_ID.Text.All(char.IsDigit) && (TxtBx_ID.Text.Length == 9))
                )
            {
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
                var lst = MainWindow.bl.GetOptionalTestsByHour(this.test, trainee);
                if (lst.Count == 0)
                {
                    MessageBox.Show("There is no Availiable test on this Hour. Try another", "OOPS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    CombBx_TestsListByHour.ItemsSource = lst;
                    CombBx_TestsListByHour.SelectedItem = lst[0];
                    CombBx_TestsListByHour.IsEnabled = true;
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

        private void Button_AddByHour_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string serialOfTest = MainWindow.bl.AddTest((CombBx_TestsListByHour.SelectedItem) as Test);
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
    }

}