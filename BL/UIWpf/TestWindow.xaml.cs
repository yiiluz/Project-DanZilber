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
        class TesterWithHour
        {
            public Tester tester;
            public int hour;
            public TesterWithHour(Tester t, int h)
            {
                tester = t;
                hour = h;
            }
        }
        Test test = new Test();
        //List<Tester> lst;
        public TestWindow()
        {
            InitializeComponent();
            this.DataContext = test;
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            
            //Trainee trainee = MainWindow.bl.GetTraineeByID(IDTxt.Text);
            //var lst = from item in MainWindow.bl.GetAvailableTestersForSpecificDay(test.DateOfTest, test.HourOfTest, trainee.CurrCarType)
            //          orderby Math.Abs(item.GetClosetHour(test.DateOfTest, test.HourOfTest))
            //          select new TesterWithHour(item, item.GetClosetHour(test.DateOfTest, test.HourOfTest));
            ////TestersList.ItemsSource = lst;
            ////TestersList.IsEnabled = true;
            //test.ExTrainee = new ExternalTrainee(MainWindow.bl.GetTraineeByID(IDTxt.Text));
            //test.ExTester = new ExternalTester(((TesterWithHour)TestersList.SelectedItem).tester);
            //MainWindow.bl.AddTest(test);

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

        private void TxtBx_Hour_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtBx_Hour.Text.All(char.IsDigit) || (int.Parse(TxtBx_Hour.Text) <= 15 && int.Parse(TxtBx_Hour.Text) >= 9))
                TxtBx_Hour.Background = Brushes.Red;
            else
                TxtBx_Hour.BorderBrush = Brushes.Green;
        }

        private void TxtBx_Hour_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_Hour.Background = Brushes.White;
            TxtBx_Hour.BorderBrush = Brushes.Gray;
        }
    }
}
