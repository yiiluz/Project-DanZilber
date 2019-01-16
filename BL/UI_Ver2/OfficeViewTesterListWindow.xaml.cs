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
    /// Interaction logic for AdminViewTesterListWindow.xaml
    /// </summary>
    public partial class OfficeViewTesterListWindow : Window
    {

        public OfficeViewTesterListWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.DataContext = TestersList;
            InitializeComponent();
        }
        List<Tester> TestersList = MainWindow.bl.GetTestersList();
        private void MenuItem_Click_UpdateTester(object sender, RoutedEventArgs e)
        {
            TesterDetailsWindow testerDetailsWindow = new TesterDetailsWindow(ListBox_TestersList.SelectedItem as Tester, "Update");
            testerDetailsWindow.ShowDialog();
        }

        private void MenuItem_Click_RemoveTester(object sender, RoutedEventArgs e)
        {
            List<TesterTest> abortedTests;
            try
            {
                abortedTests = MainWindow.bl.RemoveTester(((Tester)ListBox_TestersList.SelectedItem).Id);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string aborted = "";
            foreach (var item in abortedTests)
                aborted += "Test Serial: " + item.TestId + ". Date: " + item.DateOfTest.ToShortDateString() + ". Hour: " + item.HourOfTest + ":00.\n";
            MessageBox.Show("Tester with ID " + (((Tester)ListBox_TestersList.SelectedItem).Id) + " successfuly deleted.\n"
                + "Aborted Tests:\n" + aborted, "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ListBox_TestersListMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(ListBox_TestersList.SelectedItem.ToString(), "AdminViewTesterListWindow", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
