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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class TesterWindow : Window
    {
        Tester tester;
        public TesterWindow(Tester tester)
        {
            this.tester = tester;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void Button_Back_MouseEnter(object sender, MouseEventArgs e)
        {
            this.BackImage.Height += 5;
            this.BackImage.Width += 5;
        }

        private void Button_Back_MouseLeave(object sender, MouseEventArgs e)
        {
            this.BackImage.Height -= 5;
            this.BackImage.Width -= 5;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetSerialWindow getSerialWindow = new GetSerialWindow();
            getSerialWindow.ShowDialog();
            if (getSerialWindow.IsClosedByButton)
            {
                if (getSerialWindow.TxtBx_Serial)
                {

                }
            
            }
        }

        private void Button_Click_ViewTesterDetails(object sender, RoutedEventArgs e)
        {
            TesterDetailsWindow testerDetailsWindow = new TesterDetailsWindow(tester, "View");
            testerDetailsWindow.ShowDialog();
        }

        private void Button_Click_ViewTestList(object sender, RoutedEventArgs e)
        {
            if (tester.TestList.Count == 0)
            {
                MessageBox.Show("There is no Tests to show yet.", "Nothing to show", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                TesterViewTestsWindow testerViewTestsWindow = new TesterViewTestsWindow(tester);
                testerViewTestsWindow.ShowDialog();
            }
        }
    }
}
