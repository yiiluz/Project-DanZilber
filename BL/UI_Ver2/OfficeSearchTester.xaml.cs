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

    enum TesterGroupCategorys { הכל, שנות_וותק, עיר_מגורים, מרחק_מקסימלי_ממבחן, התמחות }
    public partial class OfficeSearchTester : Window
    {
        private ObservableCollection<Tester> mainList =
            new ObservableCollection<Tester>(MainWindow.bl.GetTestersList());
        ObservableCollection<IGrouping<int, Tester>> groupedBySeniority;
        ObservableCollection<IGrouping<string, Tester>> groupedByCity;
        ObservableCollection<IGrouping<int, Tester>> groupedByMaxDistance;
        ObservableCollection<IGrouping<CarTypeEnum, Tester>> groupedBySpecialization;
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
            ComboBox_GroupOptions.SelectedIndex = (int)TesterGroupCategorys.הכל;
            ComboBox_GroupOptions_SelectionChanged(null, null);
            SearchFilterChanged(null, null);
        }
        private void MenuItem_ClickRemoveTester(object sender, RoutedEventArgs e)
        {
            if ((Tester)TestersList.SelectedItem == null)
                return;
            if (MessageBox.Show("האם אתה בטוח שברצונך למחוק בוחן זה מהמערכת? פעולה זו אינה הפיכה!", "מחיקת בוחן", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) == MessageBoxResult.Yes)
            {
                List<TesterTest> abortedTests;
                try
                {
                    abortedTests = MainWindow.bl.RemoveTester(((Tester)TestersList.SelectedItem).Id);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "נתון לא קיים", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    return;
                }
                string aborted = "";
                foreach (var item in abortedTests)
                    aborted += "מספר מבחן: " + item.TestId + ". תאריך המבחן: " + item.DateOfTest.ToShortDateString() + ". שעת התחלה: " + item.HourOfTest + ":00.\n";
                MessageBox.Show("בוחן בעל ת.ז. " + (((Tester)TestersList.SelectedItem).Id) + " נמחק בהצלחה מהמערכת!.\n"
                    + "רשימת הטסטים שהתבטלו בעקבות המחיקה:\n" + aborted, "סטטוס מחיקה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                listToFilter.Remove((Tester)TestersList.SelectedItem);
                mainList.Remove((Tester)TestersList.SelectedItem);
                SearchFilterChanged(null, null);
            }
        }
        private void MenuItem_Click_InformationOfTester(object sender, RoutedEventArgs e)
        {
            if (TestersList.SelectedItem != null)
                MessageBox.Show(TestersList.SelectedItem.ToString(), "פרטי בוחן", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private void ButtonClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ComboBox_GroupOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_GroupOptions.SelectedIndex)
            {
                case (int)TesterGroupCategorys.הכל:
                    ComboBox_GroupNames.IsEnabled = false;
                    ComboBox_GroupNames.SelectedItem = null;
                    listToFilter = mainList;
                    TestersList.ItemsSource = listToFilter;
                    return;
                case (int)TesterGroupCategorys.שנות_וותק:
                    groupedBySeniority = new ObservableCollection<IGrouping<int, Tester>>(MainWindow.bl.GetTestersGropedBySeniority());
                    int[] keysOfSeniority = (from item in groupedBySeniority select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfSeniority;
                    break;
                case (int)TesterGroupCategorys.עיר_מגורים:
                    groupedByCity = new ObservableCollection<IGrouping<string, Tester>>(MainWindow.bl.GetTestersGroupedByCity());
                    string[] keysOfCity = (from item in groupedByCity select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfCity;
                    break;
                case (int)TesterGroupCategorys.מרחק_מקסימלי_ממבחן:
                    groupedByMaxDistance = new ObservableCollection<IGrouping<int, Tester>>(MainWindow.bl.GetTestersGropedByMaxDistance());
                    int[] keysOfMaxDistance = (from item in groupedByMaxDistance select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfMaxDistance;
                    break;
                case (int)TesterGroupCategorys.התמחות:
                    groupedBySpecialization = new ObservableCollection<IGrouping<CarTypeEnum, Tester>>(MainWindow.bl.GetTestersGrupedBySpecialization());
                    CarTypeEnum[] keysOfSpecialization = (from item in groupedBySpecialization select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfSpecialization;
                    break;
            }
            ComboBox_GroupNames.IsEnabled = true;
            ComboBox_GroupNames.SelectedIndex = 0;
        }
        private void ComboBox_GroupNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_GroupOptions.SelectedIndex)
            {
                case (int)TesterGroupCategorys.הכל:
                    ComboBox_GroupNames.IsEnabled = false;
                    ComboBox_GroupNames.SelectedItem = null;
                    mainList = new ObservableCollection<Tester>(MainWindow.bl.GetTestersList());
                    listToFilter = mainList;
                    SearchFilterChanged(null, null);
                    break;
                case (int)TesterGroupCategorys.שנות_וותק:
                    foreach (var item in groupedBySeniority)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (int)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Tester>(item.ToList());
                            break;
                        }
                    break;
                case (int)TesterGroupCategorys.עיר_מגורים:
                    foreach (var item in groupedByCity)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (string)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Tester>(item.ToList());
                            break;
                        }
                    break;
                case (int)TesterGroupCategorys.מרחק_מקסימלי_ממבחן:
                    foreach (var item in groupedByMaxDistance)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (int)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Tester>(item.ToList());
                            break;
                        }
                    break;
                case (int)TesterGroupCategorys.התמחות:
                    foreach (var item in groupedBySpecialization)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (CarTypeEnum)ComboBox_GroupNames.SelectedItem)
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
