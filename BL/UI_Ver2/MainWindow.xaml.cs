using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    enum ScreenChoose { Admin, Office, ExistingTester, ExistingTrainee }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //for resizing option
        static int resizeMode = 0;
        //passwords
        string adminPass = "1111", officePass = "1111";

        //List To View
        public static IBL bl = BO.Factory.GetBLObj();
        private ObservableCollection<Tester> testersCollection;
        private ObservableCollection<Trainee> traineeCollection;
        private ObservableCollection<Test> testCollection;

        //Object To Bind
        Tester tester;
        Trainee trainee;

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


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl.SelectedIndex == 1)
            {
                OfficeMainWindowBorder.Visibility = Visibility.Collapsed;
                OfficeStatistics.Visibility = Visibility.Collapsed;
                OfficePasswordBorder.Visibility = Visibility.Visible;

                testersCollection = new ObservableCollection<Tester>(bl.GetTestersList());
                traineeCollection = new ObservableCollection<Trainee>(bl.GetTraineeList());
                testCollection = new ObservableCollection<Test>(bl.GetTestsList());
                //ListView_Testers.ItemsSource = testersCollection;
            }
            if (tabControl.SelectedIndex == 2)
            {
                ExistingTesterMainWindowBorder.Visibility = Visibility.Collapsed;
                ExistingTesterIDBorder.Visibility = Visibility.Visible;
            }

        }


        //office tester implement
        //------------------------------------------------------------------------------------------------------------
        private void PassBox_passOffice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                Button_Click_EnterAsOffice(null, null);
        }
        private void Button_Click_EnterAsOffice(object sender, RoutedEventArgs e)
        {
            if (PassBox_passOffice.Password == officePass)
            {
                PassBox_passOffice.Password = "";
                OfficePasswordBorder.Visibility = Visibility.Collapsed;
                OfficeMainWindowBorder.Visibility = Visibility.Visible;
                OfficeStatistics.Visibility = Visibility.Visible;
                return;
            }
            MessageBox.Show("Password is not correct. Try again.", "Security Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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
                if (bl.GetTestersList().Exists(x => x.Id == getIDWindow.TxtBx_ID.Text) && MessageBox.Show("Are You sure you want to delete this tester?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
                        + (aborted != "" ? "Aborted Tests:\n" + aborted : "No Test Aborted.\n"), "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Tester not on system", "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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


        //Existing Tester Implement
        //------------------------------------------------------------------------------------------------------------
        private void TextBox_TesterID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                Button_Click_EnterAsExistingTester(null, null);
        }
        private void TextBox_ExistingTesterID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBox_TesterID.Background != Brushes.White)
                TextBox_TesterID.Background = Brushes.White;
        }
        private void TextBox_TesterID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TextBox_TesterID.Text.All(char.IsDigit))
                TextBox_TesterID.Background = Brushes.Red;
            else
                TextBox_TesterID.Background = Brushes.White;
        }
        private void ListView_TesterTests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
        private void Button_Click_EnterAsExistingTester(object sender, RoutedEventArgs e)
        {
            if (TextBox_TesterID.Text.Length != TextBox_TesterID.MaxLength || !TextBox_TesterID.Text.All(char.IsDigit))
            {
                TextBox_TesterID.Background = Brushes.Red;
                MessageBox.Show("ID must be exactly 9 Digitis, and Digits only.", "Wrong input", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                try
                {
                    tester = bl.GetTesterByID(TextBox_TesterID.Text);
                }
                catch (KeyNotFoundException)
                {
                    MessageBox.Show("The ID that enterd wasn't fpound as a Tester ID.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ExistingTesterIDBorder.Visibility = Visibility.Collapsed;
                ExistingTesterMainWindowBorder.Visibility = Visibility.Visible;
                TextBox_TesterID.Text = "";
                ListView_TesterTests.ItemsSource = tester.TestList;
                return;
            }
        }

        private void MenuItem_Click_UpdateTestResult(object sender, RoutedEventArgs e)
        {
            string ErrorList = "";
            if (ListView_TesterTests.SelectedIndex == -1)
                return;
            TesterTest temp = (TesterTest)ListView_TesterTests.SelectedItem;
            if (DateTime.Now == temp.DateOfTest && DateTime.Now.Hour < temp.HourOfTest || DateTime.Now < temp.DateOfTest)
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
                    this.ListView_TesterTests.ItemsSource = tester.TestList;
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show("Internal error on Existing Tester Tub at UpdateMenu");
                }
            }
            else
            {
                MessageBox.Show(ErrorList, "TesterWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //------------------------------------------------------------------------------------------------------------

        //implement sort at list views by columns
        //------------------------------------------------------------------------------------------------------------
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private void Sort(string sortBy, ListSortDirection direction)
        {
            TabItem tmpTab = TabControl_Login.SelectedItem as TabItem;

            if (tmpTab.TabIndex == 2)
            {
                ICollectionView dataView = CollectionViewSource.GetDefaultView(ListView_TesterTests.ItemsSource);
                dataView.SortDescriptions.Clear();
                SortDescription sd = new SortDescription(sortBy, direction);
                dataView.SortDescriptions.Add(sd);
                dataView.Refresh();
            }
            //if (tmpTab.TabIndex == 2)
            //{
            //    ICollectionView dataView = CollectionViewSource.GetDefaultView(TraineeList.ItemsSource);
            //    dataView.SortDescriptions.Clear();
            //    SortDescription sd = new SortDescription(sortBy, direction);
            //    dataView.SortDescriptions.Add(sd);
            //    dataView.Refresh();
            //}
            //if (tmpTab.TabIndex == 3)
            //{
            //    ICollectionView dataView = CollectionViewSource.GetDefaultView(TestsList.ItemsSource);
            //    dataView.SortDescriptions.Clear();
            //    SortDescription sd = new SortDescription(sortBy, direction);
            //    dataView.SortDescriptions.Add(sd);
            //    dataView.Refresh();
            //}
        }
        public void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header  
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------
    }
}
