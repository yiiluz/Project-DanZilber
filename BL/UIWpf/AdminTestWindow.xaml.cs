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
    /// Interaction logic for AdminTestWindow.xaml
    /// </summary>
    public partial class AdminTestWindow : Window
    {
        public AdminTestWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            this.Close();
            adminWindow.Show();
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

        private void Button_Click_AddTest(object sender, RoutedEventArgs e)
        {
            AddTestWindow addTestWindow = new AddTestWindow();
            addTestWindow.ShowDialog();
        }

        private void Button_Click_AbortTest(object sender, RoutedEventArgs e)
        {
            GetSerialWindow getSerialWindow = new GetSerialWindow(this, "Abort");
            getSerialWindow.ShowDialog();
        }

        private void Button_Click_ViewTestDetails(object sender, RoutedEventArgs e)
        {
            GetSerialWindow getSerialWindow = new GetSerialWindow(this, "View");
            getSerialWindow.ShowDialog();
        }

        //    private void Button_Click_UpdateTester(object sender, RoutedEventArgs e)
        //    {
        //        GetIDWindow getIDWindow = new GetIDWindow();
        //        getIDWindow.ShowDialog();
        //        if (getIDWindow.IsClosedByButton)
        //        {
        //            Tester tester;
        //            try
        //            {
        //                tester = MainWindow.bl.GetTesterByID(getIDWindow.TxtBx_ID.Text);
        //            }
        //            catch (KeyNotFoundException ex)
        //            {
        //                MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
        //                return;
        //            }
        //            TesterDetailsWindow testerDetailsWindow = new TesterDetailsWindow(tester);
        //            testerDetailsWindow.ShowDialog();
        //        }
        //    }

        //    private void Button_Click_RemoveTester(object sender, RoutedEventArgs e)
        //    {
        //        GetIDWindow getIDWindow = new GetIDWindow();
        //        getIDWindow.ShowDialog();
        //        if (getIDWindow.IsClosedByButton)
        //        {
        //            try
        //            {
        //                MainWindow.bl.RemoveTester(getIDWindow.TxtBx_ID.Text);
        //            }
        //            catch (KeyNotFoundException ex)
        //            {
        //                MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
        //                return;
        //            }
        //            MessageBox.Show("Tester with ID " + getIDWindow.TxtBx_ID.Text + " successfuly deleted.", "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //    }
        //}
    }
}
