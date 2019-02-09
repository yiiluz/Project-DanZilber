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
    /// Interaction logic for SearchTester.xaml
    /// </summary>

    enum TesterGroupCategorys { All, Seniority, City, MaxDistance }
    public partial class OfficeSearchTester : Window
    {
        private ObservableCollection<Tester> mainList =
            new ObservableCollection<Tester>(MainWindow.bl.GetTestersList());
        ObservableCollection<IGrouping<int, Tester>> groupedBySeniority;
        ObservableCollection<IGrouping<string, Tester>> groupedByCity;
        ObservableCollection<IGrouping<int, Tester>> groupedByMaxDistance;
        private ObservableCollection<Tester> listToFilter;

        public OfficeSearchTester()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.TestersList.ItemsSource = mainList;
            this.ComboBox_GroupOptions.ItemsSource = Enum.GetValues(typeof(TesterGroupCategorys));
            //this.ComboBox_GroupOptions.SelectedIndex = 0;
            ComboBox_GroupNames.IsEnabled = false;
            listToFilter = mainList;
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
            if (listToFilter != null)
            {
                ObservableCollection<Tester> it = new ObservableCollection<Tester>((from item in listToFilter
                                                                                    where CheckIfStringsAreEqual(FirstName.Text, item.FirstName)
                                                                                    select item
                                                                                into g
                                                                                    where CheckIfStringsAreEqual(LestName.Text, g.LastName)
                                                                                    select g
                                                                                into j
                                                                                    where CheckIfStringsAreEqual(ID.Text, j.Id)
                                                                                    select j).ToList());
                TestersList.ItemsSource = it;
            }
        }

        private void MenuItem_Click_UpdateTesterDetails(object sender, RoutedEventArgs e)
        {
            if (TestersList.SelectedItem == null)
                return;
            Tester tester;
            tester = MainWindow.bl.GetTesterByID((TestersList.SelectedItem as Tester).Id);
            TesterDetailsWindow testerDetailsWindow = new TesterDetailsWindow(tester, "Update");
            testerDetailsWindow.ShowDialog();
            ComboBox_GroupOptions.SelectedIndex = (int)TraineeGroupCategorys.All;
            ComboBox_GroupOptions_SelectionChanged(null, null);
            SearchFilterChanged(null, null);
        }
        private void MenuItem_ClickRemoveTester(object sender, RoutedEventArgs e)
        {
            if ((Tester)TestersList.SelectedItem == null)
                return;
            if (MessageBox.Show("Are yoe sure you want to delete this Tester?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                List<TesterTest> abortedTests;
                try
                {
                    abortedTests = MainWindow.bl.RemoveTester(((Tester)TestersList.SelectedItem).Id);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string aborted = "";
                foreach (var item in abortedTests)
                    aborted += "Test Serial: " + item.TestId + ". Date: " + item.DateOfTest.ToShortDateString() + ". Hour: " + item.HourOfTest + ":00.\n";
                MessageBox.Show("Tester with ID " + (((Tester)TestersList.SelectedItem).Id) + " successfuly deleted.\n"
                    + "Aborted Tests:\n" + aborted, "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);
                listToFilter.Remove((Tester)TestersList.SelectedItem);
                mainList.Remove((Tester)TestersList.SelectedItem);
                SearchFilterChanged(null, null);
            }
        }
        private void MenuItem_Click_Information(object sender, RoutedEventArgs e)
        {
            if (TestersList.SelectedItem != null)
                MessageBox.Show(TestersList.SelectedItem.ToString(), "SearchItem", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ComboBox_GroupOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_GroupOptions.SelectedIndex)
            {
                case (int)TesterGroupCategorys.All:
                    ComboBox_GroupNames.IsEnabled = false;
                    ComboBox_GroupNames.SelectedItem = null;
                    listToFilter = mainList;
                    TestersList.ItemsSource = listToFilter;
                    return;
                case (int)TesterGroupCategorys.Seniority:
                    groupedBySeniority = new ObservableCollection<IGrouping<int, Tester>>(MainWindow.bl.GetTestersGropedBySeniority());
                    //ComboBox_GroupNames.ItemsSource = groupedBySeniority;
                    int[] keysOfSeniority = (from item in groupedBySeniority select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfSeniority;
                    break;
                case (int)TesterGroupCategorys.City:
                    groupedByCity = new ObservableCollection<IGrouping<string, Tester>>(MainWindow.bl.GetTestersGroupedByCity());
                    //ComboBox_GroupNames.ItemsSource = groupedByCity;
                    string[] keysOfCity = (from item in groupedByCity select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfCity;
                    break;
                case (int)TesterGroupCategorys.MaxDistance:
                    groupedByMaxDistance = new ObservableCollection<IGrouping<int, Tester>>(MainWindow.bl.GetTestersGropedByMaxDistance());
                    //ComboBox_GroupNames.ItemsSource = groupedByMaxDistance;
                    int[] keysOfMaxDistance = (from item in groupedByMaxDistance select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfMaxDistance;
                    break;
            }
            ComboBox_GroupNames.IsEnabled = true;
            ComboBox_GroupNames.SelectedIndex = 0;
        }
        private void ComboBox_GroupNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_GroupOptions.SelectedIndex)
            {
                case (int)TesterGroupCategorys.All:
                    ComboBox_GroupNames.IsEnabled = false;
                    ComboBox_GroupNames.SelectedItem = null;
                    mainList = new ObservableCollection<Tester>(MainWindow.bl.GetTestersList());
                    listToFilter = mainList;
                    SearchFilterChanged(null, null);
                    break;
                case (int)TesterGroupCategorys.Seniority:
                    foreach (var item in groupedBySeniority)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (int)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Tester>(item.ToList());
                            break;
                        }
                    break;
                case (int)TesterGroupCategorys.City:
                    foreach (var item in groupedByCity)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (string)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Tester>(item.ToList());
                            break;
                        }
                    break;
                case (int)TesterGroupCategorys.MaxDistance:
                    foreach (var item in groupedByMaxDistance)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (int)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Tester>(item.ToList());
                            break;
                        }
                    break;
            }
            TestersList.ItemsSource = listToFilter;
            if ((FirstName.Text.Length != 0 || LestName.Text.Length != 0 || ID.Text.Length != 0) && listToFilter != null)
                SearchFilterChanged(null, null);
        }


        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(TestersList.ItemsSource);
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
