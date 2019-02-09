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
using System.Windows.Shapes;
using BO;
namespace UI_Ver2
{
    /// <summary>
    /// Interaction logic for OfficeTestSearchWindow.xaml
    /// </summary>
    enum HowToGroupTest { ALL, City, Status, typeOfCAR };
    enum Status { Aborted, NonAborted, Passed, NonPassed, UpdateStatus, NonUpdateStatus };
    public partial class OfficeTestSearchWindow : Window
    {
        ObservableCollection<Test> mainList = new ObservableCollection<Test>(BO.Factory.GetBLObj().GetTestsList());
        ObservableCollection<IGrouping<string, Test>> groupedByCity;
        ObservableCollection<IGrouping<CarTypeEnum, Test>> GroupedByCarType;
        ObservableCollection<IGrouping<bool, Test>> GroupByPassedOrNonPassed;
        ObservableCollection<IGrouping<bool, Test>> GroupedByAbortedOrNonAborted;
        ObservableCollection<IGrouping<bool, Test>> GroupedByUpdateStatusOrNonUpdateStatus;
        ObservableCollection<Test> listToFilter;
        public OfficeTestSearchWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            listToFilter = mainList;
            this.TestsList.ItemsSource = listToFilter;
            this.ComboBox_GroupOptions.ItemsSource = Enum.GetValues(typeof(HowToGroupTest));
            this.ComboBox_GroupNames.IsEnabled = false;
        }

        private bool CheckIfStringsAreEqual(string a, string b)
        {
            if (a.Length > b.Length)
                return false;
            int c = Math.Min(a.Length, b.Length);
            a = a.ToLower();
            b = b.ToLower();
            for (int i = 0; i < c; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
        private void SearchFilterChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<Test> it = new ObservableCollection<Test>((from item in listToFilter
                                                                            where CheckIfStringsAreEqual(TestID.Text, item.TestId)
                                                                            select item
                                                                            into g
                                                                            where CheckIfStringsAreEqual(TesterID.Text, g.ExTester.Id)
                                                                            select g
                                                                            into j
                                                                            where CheckIfStringsAreEqual(TraineeID.Text, j.ExTrainee.Id)
                                                                            select j).ToList());
            TestsList.ItemsSource = it;
        }

        private void ComboBox_GroupOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_GroupOptions.SelectedIndex)
            {
                case (int)HowToGroupTest.ALL:
                    mainList = new ObservableCollection<Test>(BO.Factory.GetBLObj().GetTestsList());
                    listToFilter = mainList;
                    ComboBox_GroupOptions.ItemsSource = listToFilter;
                    SearchFilterChanged(null, null);
                    ComboBox_GroupNames.SelectedItem = null;
                    ComboBox_GroupNames.IsEnabled = false;
                    return;
                case (int)HowToGroupTest.City:
                    //renewListTest();
                    groupedByCity = new ObservableCollection<IGrouping<string, Test>>(MainWindow.bl.GetTestsGroupedByCity());
                    string[] keysOfCity = (from item in groupedByCity select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfCity;
                    break;
                case (int)HowToGroupTest.Status:
                    //renewListTest();
                    GroupByPassedOrNonPassed = new ObservableCollection<IGrouping<bool, Test>>(MainWindow.bl.GetTestsGroupedByPassedOrNonPassed());
                    GroupedByUpdateStatusOrNonUpdateStatus = new ObservableCollection<IGrouping<bool, Test>>(MainWindow.bl.GetTestsGroupedByUpdateStatusOrNonUpdateStatus());
                    GroupedByAbortedOrNonAborted = new ObservableCollection<IGrouping<bool, Test>>(MainWindow.bl.GetTestsGroupedByAbortedOrNonAborted());
                    ComboBox_GroupNames.ItemsSource = Enum.GetValues(typeof(Status));
                    break;
                case (int)HowToGroupTest.typeOfCAR:
                    //renewListTest();
                    GroupedByCarType = new ObservableCollection<IGrouping<CarTypeEnum, Test>>(MainWindow.bl.GetTestsGroupedByCarType());
                    CarTypeEnum[] keysOfCars = (from item in GroupedByCarType select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfCars;
                    break;
            }
            ComboBox_GroupNames.SelectedIndex = 0;
            ComboBox_GroupNames.IsEnabled = true;
        }

        private void ComboBox_GroupNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_GroupOptions.SelectedItem != null)
            {
                switch (ComboBox_GroupOptions.SelectedIndex)
                {
                    case (int)HowToGroupTest.City:
                        foreach (var item in groupedByCity)
                            if (ComboBox_GroupNames.SelectedItem != null && item.Key == (string)ComboBox_GroupNames.SelectedItem)
                            {
                                listToFilter = new ObservableCollection<Test>(item.ToList());
                                break;
                            }
                        break;
                    //listToFilter = new ObservableCollection<Test>((from item in groupedByCity
                    //                                               where item.Key == (string)ComboBox_GroupNames.SelectedItem
                    //                                               select item).FirstOrDefault().ToList());
                    //break;
                    case (int)HowToGroupTest.Status:
                        if (ComboBox_GroupNames.SelectedItem != null)
                        {
                            listToFilter = null;
                            switch (ComboBox_GroupNames.SelectedIndex)
                            {
                                case (int)Status.Aborted:
                                    foreach (var item in GroupedByAbortedOrNonAborted)
                                    {
                                        if (item.Key)
                                        {
                                            listToFilter = new ObservableCollection<Test>(item.ToList());
                                            break;
                                        }
                                    }
                                    break;
                                case (int)Status.NonAborted:
                                    foreach (var item in GroupedByAbortedOrNonAborted)
                                    {
                                        if (!item.Key)
                                        {
                                            listToFilter = new ObservableCollection<Test>(item.ToList());
                                            break;
                                        }
                                    }
                                    break;
                                case (int)Status.Passed:
                                    foreach (var item in GroupByPassedOrNonPassed)
                                    {
                                        if (item.Key)
                                        {
                                            listToFilter = new ObservableCollection<Test>(item.ToList());
                                            break;
                                        }
                                    }
                                    break;
                                case (int)Status.NonPassed:
                                    foreach (var item in GroupByPassedOrNonPassed)
                                    {
                                        if (!item.Key)
                                        {
                                            listToFilter = new ObservableCollection<Test>(item.ToList());
                                            break;
                                        }
                                    }
                                    break;
                                case (int)Status.UpdateStatus:
                                    foreach (var item in GroupedByUpdateStatusOrNonUpdateStatus)
                                    {
                                        if (item.Key)
                                        {
                                            listToFilter = new ObservableCollection<Test>(item.ToList());
                                            break;
                                        }
                                    }
                                    break;
                                case (int)Status.NonUpdateStatus:
                                    foreach (var item in GroupedByUpdateStatusOrNonUpdateStatus)
                                    {
                                        if (!item.Key)
                                        {
                                            listToFilter = new ObservableCollection<Test>(item.ToList());
                                            break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case (int)HowToGroupTest.typeOfCAR:
                        foreach (var item in GroupedByCarType)
                            if (ComboBox_GroupNames.SelectedItem != null && item.Key == (CarTypeEnum)ComboBox_GroupNames.SelectedItem)
                            {
                                listToFilter = new ObservableCollection<Test>(item.ToList());
                                break;
                            }
                        break;
                }
                TestsList.ItemsSource = listToFilter;
            }
            if ((TestID.Text.Length != 0 || TesterID.Text.Length != 0 || TraineeID.Text.Length != 0) && listToFilter != null)
                SearchFilterChanged(null, null);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (TestsList.SelectedItem != null)
                MessageBox.Show(TestsList.SelectedItem.ToString(), "OfficeTestSearchWindow", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(TestsList.ItemsSource);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
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

        private void MenuItem_Click_Abort(object sender, RoutedEventArgs e)
        {
            if (TestsList.SelectedItem == null)
                return;
            Test test = MainWindow.bl.GetTestByID((TestsList.SelectedItem as Test).TestId);
            if (test.IsTestAborted)
            {
                MessageBox.Show("The test allready Aborted!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var isWantToAbort = MessageBox.Show("Test details are:\n" + test.ToString() + "Are you sure you want to abort this test?" +
                " This action is not reversible."
                , "Abort Test", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (isWantToAbort == MessageBoxResult.Yes)
            {
                try
                {
                    MainWindow.bl.AbortTest(test.TestId);
                }
                catch (KeyNotFoundException ex)
                {
                    var result = MessageBox.Show("internal error\n" + ex.Message + "\nDo you want to try again?",
                        "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.No)
                        //Close();
                        return;
                }
                MessageBox.Show("The test with id " + test.TestId + " successfuly Aborted", "Operation Status",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}


