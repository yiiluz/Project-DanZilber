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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;

namespace UI_Ver2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //for resizing option
        static int resizeMode = 0;
        //passwords
        string adminPass = "1111", officePass = "1111";
        public static IBL bl = BO.Factory.GetBLObj();
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Button_Click_CloseWindow(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }
        private void Button_Click_MinimizeWindow(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        private void Button_Click_MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (resizeMode++ == 0)
            {
                SystemCommands.MaximizeWindow(this);
            }
            else
            {
                resizeMode -= 2;
                SystemCommands.RestoreWindow(this);
            }

        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Width *= 1.1;
            ((Button)sender).Height *= 1.1;
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Width /= 1.1;
            ((Button)sender).Height /= 1.1;
        }

        private void PassBox_passAdmin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                Button_Click_EnterAsOffice(null, null);
        }
        private void Button_Click_EnterAsOffice(object sender, RoutedEventArgs e)
        {
            if (PassBox_passOffice.Password == adminPass)
            {
                PassBox_passOffice.Password = "";
                OfficePasswordBorder.Visibility = Visibility.Collapsed;
                OfficeMainWindowBorder.Visibility = Visibility.Visible;
                return;
            }
            MessageBox.Show("Password is not correct. Try again.", "Security Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OfficeMainWindowBorder.Visibility = Visibility.Collapsed;
            OfficePasswordBorder.Visibility = Visibility.Visible;
        }


        //office tester implement
        //------------------------------------------------------------------------------------------------------------
        private void Button_Click_AddTester(object sender, RoutedEventArgs e)
        {
            TesterDetailsWindow testerDetailsWindow = new TesterDetailsWindow();
            testerDetailsWindow.ShowDialog();
        }
        private void Button_Click_UpdateTester(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            if (getIDWindow.IsClosedByButton)
            {
                Tester tester;
                try
                {
                    tester = bl.GetTesterByID(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                TesterDetailsWindow testerDetailsWindow = new TesterDetailsWindow(tester, "Update");
                testerDetailsWindow.ShowDialog();
            }
        }
        private void Button_Click_RemoveTester(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            List<TesterTest> abortedTests;
            if (getIDWindow.IsClosedByButton)
            {
                try
                {
                    abortedTests = MainWindow.bl.RemoveTester(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string aborted = "";
                foreach (var item in abortedTests)
                    aborted += "Test Serial: " + item.TestId + ". Date: " + item.DateOfTest.ToShortDateString() + ". Hour: " + item.HourOfTest + ":00.\n";
                MessageBox.Show("Tester with ID " + getIDWindow.TxtBx_ID.Text + " successfuly deleted.\n"
                    + "Aborted Tests:\n" + aborted, "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Button_Click_ViewTestersLists(object sender, RoutedEventArgs e)
        {
            OfficeViewTesterListWindow officeViewTesterListWindow = new OfficeViewTesterListWindow();
            officeViewTesterListWindow.ShowDialog();
        }
        private void Button_Click_SearchTesterByName(object sender, RoutedEventArgs e)
        {
            OfficeSearchTester officeSearchTester = new OfficeSearchTester();
            officeSearchTester.ShowDialog();
        }
        //------------------------------------------------------------------------------------------------------------
        //office trainee implement
        //------------------------------------------------------------------------------------------------------------
        private void Button_Click_AddTrainee(object sender, RoutedEventArgs e)
        {
            TraineeDetailsWindow traineeDetailsWindow = new TraineeDetailsWindow();
            traineeDetailsWindow.ShowDialog();
        }
        private void Button_Click_UpdateTrainee(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            if (getIDWindow.IsClosedByButton)
            {
                Trainee traine;
                try
                {
                    traine = MainWindow.bl.GetTraineeByID(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                TraineeDetailsWindow traineeDetailsWindow = new TraineeDetailsWindow(traine, "Update");
                traineeDetailsWindow.ShowDialog();
            }
        }
        private void Button_Click_RemoveTrainee(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            if (getIDWindow.IsClosedByButton)
            {
                try
                {
                    MainWindow.bl.RemoveTrainee(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Trainee with ID " + getIDWindow.TxtBx_ID.Text + " successfuly deleted.", "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Button_Click_ViewAllTrainees(object sender, RoutedEventArgs e)
        {
            OfficeViewTraineesListWindow adminViewTraineesListWindow = new OfficeViewTraineesListWindow();
            adminViewTraineesListWindow.ShowDialog();
        }
        private void Button_Click_SearchTrainee(object sender, RoutedEventArgs e)
        {
            OfficeSearchTrainee searchTrainee = new OfficeSearchTrainee();
            searchTrainee.ShowDialog();
        }
        //------------------------------------------------------------------------------------------------------------
        //office test implement
        //------------------------------------------------------------------------------------------------------------
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
        //------------------------------------------------------------------------------------------------------------

    }
}
