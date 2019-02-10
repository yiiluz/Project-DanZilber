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
    enum TraineeGroupCategorys { הכל, עיר, בית_ספר, מספר_מבחנים, סוג_רכב}
    /// <summary>
    /// Interaction logic for SearchTrainee.xaml
    /// </summary>
    public partial class OfficeSearchTrainee : Window
    {
        private ObservableCollection<Trainee> mainList =
            new ObservableCollection<Trainee>(MainWindow.bl.GetTraineeList());
        ObservableCollection<IGrouping<string, Trainee>> groupedByCity;
        ObservableCollection<IGrouping<string, Trainee>> groupsBySchool;
        ObservableCollection<IGrouping<CarTypeEnum, Trainee>> groupedByCarType;
        
        ObservableCollection<IGrouping<int, Trainee>> groupedByNumOfTests;
        private ObservableCollection<Trainee> listToFilter;
        public OfficeSearchTrainee()
        {
            InitializeComponent();
            this.TraineeList.ItemsSource = mainList;
            this.ComboBox_GroupOptions.ItemsSource = Enum.GetValues(typeof(TraineeGroupCategorys));
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
        private void MenuItem_Click_UpdateTraineeDetails(object sender, RoutedEventArgs e)
        {
            if (TraineeList.SelectedItem == null)
                return;
            Trainee trainee;
            trainee = MainWindow.bl.GetTraineeByID((TraineeList.SelectedItem as Trainee).Id);
            TraineeDetailsWindow traineeDetailsWindow = new TraineeDetailsWindow(trainee, "Update");
            traineeDetailsWindow.ShowDialog();
            ComboBox_GroupOptions.SelectedIndex = (int)TraineeGroupCategorys.הכל;
            ComboBox_GroupOptions_SelectionChanged(null, null);
            SearchFilterChanged(null, null);
        }

        private void SearchFilterChanged(object sender, TextChangedEventArgs e)
        {
            if (listToFilter != null)
            {
                ObservableCollection<Trainee> it = new ObservableCollection<Trainee>((from item in listToFilter
                                                                                      where CheckIfStringsAreEqual(FirstName.Text, item.FirstName)
                                                                                      select item
                                                                                into g
                                                                                      where CheckIfStringsAreEqual(LestName.Text, g.LastName)
                                                                                      select g
                                                                                into j
                                                                                      where CheckIfStringsAreEqual(ID.Text, j.Id)
                                                                                      select j).ToList());
                TraineeList.ItemsSource = it;
            }
        }
        private void MenuItem_ClickRemoveTrainee(object sender, RoutedEventArgs e)
        {
            if (TraineeList.SelectedItem == null)
                return;
            try
            {
                MainWindow.bl.RemoveTrainee(((Trainee)TraineeList.SelectedItem).Id);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "נתון לא קיים", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            MessageBox.Show("תלמיד בעל מספר ת.ז. " + (((Trainee)TraineeList.SelectedItem).Id) + " נמחק בהצלחה מהמערכת.", "סטטוס מחיקה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            listToFilter.Remove((Trainee)TraineeList.SelectedItem);
            mainList.Remove((Trainee)TraineeList.SelectedItem);
        }
        private void MenuItem_Click_TraineeInformation(object sender, RoutedEventArgs e)
        {
            if (TraineeList.SelectedItem != null)
                MessageBox.Show(TraineeList.SelectedItem.ToString(), "פרטי תלמיד", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private void ComboBox_GroupOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_GroupOptions.SelectedIndex)
            {
                case (int)TraineeGroupCategorys.הכל:
                    ComboBox_GroupNames.IsEnabled = false;
                    ComboBox_GroupNames.SelectedItem = null;
                    mainList = new ObservableCollection<Trainee>(MainWindow.bl.GetTraineeList());
                    listToFilter = mainList;
                    TraineeList.ItemsSource = listToFilter;
                    SearchFilterChanged(null, null);
                    return;
                case (int)TraineeGroupCategorys.עיר:
                    groupedByCity = new ObservableCollection<IGrouping<string, Trainee>>(MainWindow.bl.GetTraineessGroupedByCity());
                    string[] keysOfCity = (from item in groupedByCity select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfCity;
                    break;
                case (int)TraineeGroupCategorys.בית_ספר:
                    groupsBySchool = new ObservableCollection<IGrouping<string, Trainee>>(MainWindow.bl.GetTraineesGroupsBySchool());
                    string[] keysOfSchool = (from item in groupsBySchool select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfSchool;
                    break;
                case (int)TraineeGroupCategorys.מספר_מבחנים:
                    groupedByNumOfTests = new ObservableCollection<IGrouping<int, Trainee>>(MainWindow.bl.GetTraineesGroupedByNumOfTests());
                    int[] keysOfNumOfTests = (from item in groupedByNumOfTests select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfNumOfTests;
                    break;
                case (int)TraineeGroupCategorys.סוג_רכב:
                    groupedByCarType = new ObservableCollection<IGrouping<CarTypeEnum, Trainee>>(MainWindow.bl.GetTraineesGroupedByCarType());
                    CarTypeEnum[] keysOfCarType = (from item in groupedByCarType select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfCarType;
                    break;
            }
            ComboBox_GroupNames.IsEnabled = true;
            ComboBox_GroupNames.SelectedIndex = 0;
        }
        private void ComboBox_GroupNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_GroupOptions.SelectedIndex)
            {
                case (int)TraineeGroupCategorys.הכל:
                    mainList = new ObservableCollection<Trainee>(MainWindow.bl.GetTraineeList());
                    listToFilter = mainList;
                    break;
                case (int)TraineeGroupCategorys.עיר:
                    foreach (var item in groupedByCity)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (string)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Trainee>(item.ToList());
                            break;
                        }
                    break;
                case (int)TraineeGroupCategorys.בית_ספר:
                    foreach (var item in groupsBySchool)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (string)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Trainee>(item.ToList());
                            break;
                        }
                    break;
                case (int)TraineeGroupCategorys.מספר_מבחנים:
                    foreach (var item in groupedByNumOfTests)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (int)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Trainee>(item.ToList());
                            break;
                        }
                    break;
                case (int)TraineeGroupCategorys.סוג_רכב:
                    foreach (var item in groupedByCarType)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (CarTypeEnum)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Trainee>(item.ToList());
                            break;
                        }
                    break;
            }
            TraineeList.ItemsSource = listToFilter;
            if ((FirstName.Text.Length != 0 || LestName.Text.Length != 0 || ID.Text.Length != 0) && listToFilter != null)
                SearchFilterChanged(null, null);
        }

        private void ButtonClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(TraineeList.ItemsSource);
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
