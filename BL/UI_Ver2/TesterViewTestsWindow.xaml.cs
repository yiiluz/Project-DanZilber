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
    /// Interaction logic for TesterViewTestsWindow.xaml
    /// </summary>
    public partial class TesterViewTestsWindow : Window
    {
        Tester tester;
        public TesterViewTestsWindow(Tester tester)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.DataContext = tester;
            this.tester = tester;
        }

        private void TestsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show((TestsList.SelectedItem as TesterTest).ToString(), "Test Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_UpdateTestResult(object sender, RoutedEventArgs e)
        {
            string ErrorList = "";
            if (TestsList.SelectedIndex == -1)
                return;
            TesterTest temp = (TesterTest)TestsList.SelectedItem;
            if (DateTime.Now == temp.DateOfTest && DateTime.Now.Hour<temp.HourOfTest || DateTime.Now < temp.DateOfTest)
                ErrorList += "ERROR! You can not update test information before the intended date. \n";
            if (temp.IsTesterUpdateStatus)
                ErrorList += "ERROR! Test results have already been entered. You can not change the test details. \n";
            if (temp.IsTestAborted)
                ErrorList += "ERROR! The test has been canceled. details can not be updated for this test \n";
            if (ErrorList == "")
            {
                TesterResultUpdateWindow testerResultUpdateWindow = new TesterResultUpdateWindow(temp);
                testerResultUpdateWindow.ShowDialog();
                try
                {
                    tester = MainWindow.bl.GetTesterByID(tester.Id);
                    this.TestsList.ItemsSource = tester.TestList;
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show("Internal error on TesterViewTestsWindow at UpdateMenu");
                }
            }
            else
            {
                MessageBox.Show(ErrorList, "TesterWindow" , MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
