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
    /// Interaction logic for AdminViewTesterListWindow.xaml
    /// </summary>
    public partial class AdminViewTesterListWindow : Window
    {

        public AdminViewTesterListWindow()
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
            try
            {
                MainWindow.bl.RemoveTester((ListBox_TestersList.SelectedItem as Tester).Id);
                TestersList = MainWindow.bl.GetTestersList();
                ListBox_TestersList.ItemsSource = TestersList;
                MessageBox.Show("The tester has been successfully deleted from the system", "AdminViewTesterListWindow", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(KeyNotFoundException ee)
            {
                MessageBox.Show(ee.Message, "AdminViewTesterListWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
