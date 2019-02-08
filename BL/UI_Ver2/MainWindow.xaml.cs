﻿using System;
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
using System.IO;
using System.Threading;
using BO;
using System.Xml.Linq;
using System.Globalization;

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
        static int numOfActivatedMainWindow = 0;
        //passwords
        string adminPass = "1111", officePass = "1111";

        //List To View
        public static IBL bl = BO.Factory.GetBLObj();
        private ObservableCollection<Tester> testersCollection;
        private ObservableCollection<Trainee> traineeCollection;
        private ObservableCollection<Test> testCollection;

        public static List<string> cities = new List<string>();
        public static List<IGrouping<string, string>> streetsGroupedByCity = new List<IGrouping<string, string>>();

        //Object To Bind
        Tester tester;
        Trainee trainee;

        public MainWindow()
        {
            numOfActivatedMainWindow++;
            this.FlowDirection = FlowDirection.RightToLeft;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            bl.AddEventIfConfigChanged(IsNeedToUpdateConfigThreadFunc);
            StatisticsGrid.DataContext = new BO.SystemStatistics();
            bl.UpdateStatistics();
            bl.AddStatisticsChangedObserve(IsNeedToUpdateStatisticsThreadFunc);

            string cityPath = @"..\..\..\Cities and Streets xml\CitiesList.xml";
            XElement citiesRoot = XElement.Load(cityPath);
            cities = (from item in citiesRoot.Elements() select item.Value).ToList();
            string streetPath = @"..\..\..\Cities and Streets xml\StreetsList.xml";
            XElement streetsRoot = XElement.Load(streetPath);
            streetsGroupedByCity = (from item in streetsRoot.Elements() group item.Element("Street").Value by item.Element("City").Value).ToList();
        }

        private void Button_Click_CloseWindow(object sender, EventArgs e)
        {
            if (--numOfActivatedMainWindow == 0)
                Environment.Exit(Environment.ExitCode);
        }
        private void Button_Click_CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            PassBox_passAdmin.Password = "";
            PassBox_passOffice.Password = "";
            TextBox_TesterID.Text = "";
            TextBox_TraineeID.Text = "";
            try
            {
                TabControl tabControl = sender as TabControl;
                if (tabControl.SelectedIndex == 0)
                {
                    AdminMainWindowBorder.Visibility = Visibility.Collapsed;
                    AdminPasswordBorder.Visibility = Visibility.Visible;
                }
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
                if (tabControl.SelectedIndex == 3)
                {
                    ExistingTraineeMainWindowBorder.Visibility = Visibility.Collapsed;
                    ExistingTraineeIDBorder.Visibility = Visibility.Visible;
                }
            }
            catch (DirectoryNotFoundException D)
            {
                MessageBox.Show(D.Message, "מערכת לניהול מבחני נהיגה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (KeyNotFoundException a)
            {
                MessageBox.Show(a.Message, "מערכת לניהול מבחני נהיגה", MessageBoxButton.OK, MessageBoxImage.Error);
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
            MessageBox.Show("קוד שגוי, אנא נסה שוב.", "אזעקת אבטחה", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show(ex.Message, "תעודת זהות לא קיימת.", MessageBoxButton.OK, MessageBoxImage.Error);
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
                if (bl.GetTestersList().Exists(x => x.Id == getIDWindow.TxtBx_ID.Text) && MessageBox.Show("אתה בטוח שברצונך למחוק את הבוחן?", "אזהרה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        abortedTests = MainWindow.bl.RemoveTester(getIDWindow.TxtBx_ID.Text);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, "תעודת זהות לא קיימת", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    string aborted = "";
                    foreach (var item in abortedTests)
                        aborted += "Test Serial: " + item.TestId + ". Date: " + item.DateOfTest.ToShortDateString() + ". Hour: " + item.HourOfTest + ":00.\n";
                    MessageBox.Show("בוחן עם תעודת זהות" + getIDWindow.TxtBx_ID.Text + "נמחק בהצלחה.\n"
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
            GetSerialWindow getSerialWindow = new GetSerialWindow("Abort");
            getSerialWindow.ShowDialog();
        }
        private void Button_Click_ViewTestDetails(object sender, RoutedEventArgs e)
        {
            GetSerialWindow getSerialWindow = new GetSerialWindow("View");
            getSerialWindow.ShowDialog();
        }
        private void SearchTestsButton_Click(object sender, RoutedEventArgs e)
        {
            (new OfficeTestSearchWindow()).ShowDialog();
        }

        //------------------------------------------------------------------------------------------------------------

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
                    MessageBox.Show("The ID that enterd wasn't found as a Tester ID.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ExistingTesterIDBorder.Visibility = Visibility.Collapsed;
                ExistingTesterMainWindowBorder.Visibility = Visibility.Visible;
                TextBox_TesterID.Text = "";
                ListView_TesterTests.ItemsSource = new ObservableCollection<TesterTest>(tester.TestList);
                TesterStatisticsBorder.DataContext = tester.Statistics;
                TextBlock_Statistics_NumOfTestPerWeek.Text = bl.GetTesterNumOfTestForDateWeek(tester, DateTime.Today).ToString();
                return;
            }
        }
        private void MenuItem_Click_UpdateTestResult(object sender, RoutedEventArgs e)
        {
            if (ListView_TesterTests.SelectedIndex == -1 || ListView_TesterTests.SelectedItem == null)
                return;
            TesterTest temp = (TesterTest)ListView_TesterTests.SelectedItem;
            if (temp.IsTesterUpdateStatus)
            {
                MessageBox.Show("You already updated status for this Test!", "Duplicate Block", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (temp.IsTestAborted)
            {
                MessageBox.Show("That test can NOT Updated, because he was Canceld.", "Test Aborted Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (temp.DateOfTest > DateTime.Now)
            {
                MessageBox.Show("Wait after the test for updating results.", "Oops", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            TesterResultUpdateWindow testerResultUpdateWindow = new TesterResultUpdateWindow(temp);
            testerResultUpdateWindow.ShowDialog();
            try
            {
                tester = MainWindow.bl.GetTesterByID(tester.Id);
                this.ListView_TesterTests.ItemsSource = tester.TestList;
                TesterStatisticsBorder.DataContext = tester.Statistics;

            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show("Internal error on Existing Tester Tub at UpdateMenu.\n" + ex.Message);
            }
        }
        private void Button_Click_UpdateTestResultByID(object sender, RoutedEventArgs e)
        {
            GetSerialWindow getSerialWindow = new GetSerialWindow("Update");
            getSerialWindow.ShowDialog();
            if (getSerialWindow.IsClosedByButton)
            {
                try
                {
                    tester = bl.GetTesterByID(tester.Id);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show("Internal error at Button_Click_UpdateTestResultByID on main.\n" + ex.Message, "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                ListView_TesterTests.ItemsSource = tester.TestList;
            }
        }
        private void Button_Click_ViewTesterDetails(object sender, RoutedEventArgs e)
        {
            TesterDetailsWindow testerDetailsWindow = new TesterDetailsWindow(tester, "View");
            testerDetailsWindow.ShowDialog();
        }
        private void ListView_TesterTests_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((ListView)sender).SelectedItem != null)
                MessageBox.Show((((ListView)sender).SelectedItem as TesterTest).ToString(), "Test Details", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //------------------------------------------------------------------------------------------------------------

        //Existing trainee implement
        //------------------------------------------------------------------------------------------------------------
        private void TextBox_TraineeID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                Button_Click_EnterAsExistingTrainee(null, null);
        }
        private void TextBox_ExistingTraineeID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBox_TraineeID.Background != Brushes.White)
                TextBox_TraineeID.Background = Brushes.White;
        }
        private void TextBox_TraineeID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TextBox_TraineeID.Text.All(char.IsDigit))
                TextBox_TraineeID.Background = Brushes.Red;
            else
                TextBox_TraineeID.Background = Brushes.White;
        }
        private void ListView_TraineeTests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
        private void Button_Click_EnterAsExistingTrainee(object sender, RoutedEventArgs e)
        {
            if (TextBox_TraineeID.Text.Length != TextBox_TraineeID.MaxLength || !TextBox_TraineeID.Text.All(char.IsDigit))
            {
                TextBox_TraineeID.Background = Brushes.Red;
                MessageBox.Show("ID must be exactly 9 Digitis, and Digits only.", "Wrong input", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                try
                {
                    trainee = bl.GetTraineeByID(TextBox_TraineeID.Text);
                }
                catch (KeyNotFoundException)
                {
                    MessageBox.Show("The ID that enterd wasn't found as a Trainee ID.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ExistingTraineeIDBorder.Visibility = Visibility.Collapsed;
                ExistingTraineeMainWindowBorder.Visibility = Visibility.Visible;
                TextBox_TraineeID.Text = "";
                ListView_TraineeTests.ItemsSource = new ObservableCollection<TraineeTest>(trainee.TestList);
                ListView_TraineeExistingLicenses.ItemsSource = trainee.ExistingLicenses;
                TraineeStatisticsBorder.DataContext = trainee.Statistics;
                return;
            }
        }
        private void Button_Click_ViewTraineeDetails(object sender, RoutedEventArgs e)
        {
            TraineeDetailsWindow traineeDetailsWindow = new TraineeDetailsWindow(trainee, "View");
            traineeDetailsWindow.ShowDialog();
        }
        private void ListView_TraineeTests_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((ListView)sender).SelectedItem != null)
                MessageBox.Show((((ListView)sender).SelectedItem as TraineeTest).ToString(), "Test Details", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ListView_TraineeExistingLicenses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
        private void Button_Click_TraineeAddTest(object sender, RoutedEventArgs e)
        {
            AddTestWindow testWindow = new AddTestWindow(trainee);
            testWindow.ShowDialog();
            trainee = bl.GetTraineeByID(trainee.Id);
            ListView_TraineeTests.ItemsSource = trainee.TestList;
        }

        //Admin implement
        //------------------------------------------------------------------------------------------------------------
        DateTime lastUpdate;

        private void PassBox_passAdmin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                Button_Click_EnterAsAdmin(null, null);
        }
        private void Button_Click_EnterAsAdmin(object sender, RoutedEventArgs e)
        {
            if (PassBox_passAdmin.Password == adminPass)
            {
                PassBox_passAdmin.Password = "";
                AdminPasswordBorder.Visibility = Visibility.Collapsed;
                AdminMainWindowBorder.Visibility = Visibility.Visible;
                ListView_Configurations.ItemsSource = bl.GetConfig();
                lastUpdate = BL.Configuretion.LastUpdate;
                TextBlock_LastUpdate.DataContext = lastUpdate;
                return;
            }
            MessageBox.Show("Password is not correct. Try again.", "Security Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ListView_Configurations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
        private void MenuItem_Click_SetConfig(object sender, RoutedEventArgs e)
        {
            if (ListView_Configurations.SelectedValue != null)
            {
                KeyValuePair<string, Object> x = (KeyValuePair<string, Object>)(ListView_Configurations.SelectedValue);
                if (x.Key == "Serial Number Test")
                {
                    MessageBox.Show("לא ניתן לעדכן הגדרה זו באופן ידני.", "שגיאת הרשאה", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    return;
                }
                (new SetConfigWindow(x)).ShowDialog();
            }
        }



        void IsNeedToUpdateConfigThreadFunc()
        {
            Action action = IsNeedToUpdateConfig;
            Dispatcher.BeginInvoke(action);
        }
        void IsNeedToUpdateConfig()
        {
            if (lastUpdate < BL.Configuretion.LastUpdate)
            {
                ListView_Configurations.ItemsSource = bl.GetConfig();
                lastUpdate = BL.Configuretion.LastUpdate;
                TextBlock_LastUpdate.DataContext = lastUpdate;
            }
        }
        void IsNeedToUpdateStatisticsThreadFunc()
        {
            Action action = IsNeedToUpdateStatistics;
            Dispatcher.BeginInvoke(action);
        }
        void IsNeedToUpdateStatistics()
        {
            StatisticsGrid.DataContext = new SystemStatistics();
        }





        //private void MenuItem_Click_UpdateTestResult(object sender, RoutedEventArgs e)
        //{
        //    string ErrorList = "";
        //    if (ListView_TesterTests.SelectedIndex == -1)
        //        return;
        //    TesterTest temp = (TesterTest)ListView_TesterTests.SelectedItem;
        //    TesterResultUpdateWindow testerResultUpdateWindow = new TesterResultUpdateWindow(temp);
        //    testerResultUpdateWindow.ShowDialog();
        //    try
        //    {
        //        tester = MainWindow.bl.GetTesterByID(tester.Id);
        //        this.ListView_TesterTests.ItemsSource = tester.TestList;
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        MessageBox.Show("Internal error on Existing Tester Tub at UpdateMenu");
        //    }
        //}

        //------------------------------------------------------------------------------------------------------------


        //implement sort at list views by columns
        //------------------------------------------------------------------------------------------------------------
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = null;
            if (TabControl_Login.SelectedIndex == 0)
            {
                dataView = CollectionViewSource.GetDefaultView(ListView_Configurations.ItemsSource);
            }
            if (TabControl_Login.SelectedIndex == 2)
            {
                dataView = CollectionViewSource.GetDefaultView(ListView_TesterTests.ItemsSource);

            }
            if (TabControl_Login.SelectedIndex == 3)
            {
                dataView = CollectionViewSource.GetDefaultView(ListView_TraineeTests.ItemsSource);
            }
            if (dataView != null)
            {
                dataView.SortDescriptions.Clear();
                SortDescription sd = new SortDescription(sortBy, direction);
                dataView.SortDescriptions.Add(sd);
                dataView.Refresh();
            }
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

        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            TabControl_SelectionChanged(TabControl_Login, null);
        }



        //------------------------------------------------------------------------------------------------------------




        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            (new MainWindow()).Show();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
