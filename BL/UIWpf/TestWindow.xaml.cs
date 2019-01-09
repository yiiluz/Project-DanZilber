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
namespace UIWpf
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        Test test = new Test();
        //List<Tester> lst;
        public TestWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.DataContext = test;
            test.DateOfTest = DateTime.Today;
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
        private void Button_Click_GetTestersListByDate(object sender, RoutedEventArgs e)
        {
            //
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
                var lst = MainWindow.bl.GetOptionalTests(this.test, trainee);
                CombBx_TestsListByDate.ItemsSource = lst;
                CombBx_TestsListByDate.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("The input is not correct. The bad inputs marked at Red Unless you hav'nt update nothing.", "Wrong Input", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        private void Button_Click_GetTestersListByHour(object sender, RoutedEventArgs e)
        {

        }

        private void TxtBx_HourByHour_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TxtBx_HourByHour_GotFocus(object sender, RoutedEventArgs e)
        {

        }
    }

}