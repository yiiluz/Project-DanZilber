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
    enum TraineeGroupCategorys { All, City, School, Teacher, NumOfTests }
    /// <summary>
    /// Interaction logic for SearchTrainee.xaml
    /// </summary>
    public partial class OfficeSearchTrainee : Window
    {
        private ObservableCollection<Trainee> mainList =
            new ObservableCollection<Trainee>(MainWindow.bl.GetTraineeList());
        ObservableCollection<IGrouping<string, Trainee>> groupedByCity;
        ObservableCollection<IGrouping<string, Trainee>> groupsBySchool;
        ObservableCollection<IGrouping<string, Trainee>> groupsByTeacher;
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
            Trainee trainee;
            trainee = MainWindow.bl.GetTraineeByID((TraineeList.SelectedItem as Trainee).Id);
            TraineeDetailsWindow traineeDetailsWindow = new TraineeDetailsWindow(trainee, "Update");
            traineeDetailsWindow.ShowDialog();
        }

        private void SearchFilterChanged(object sender, TextChangedEventArgs e)
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
        private void MenuItem_ClickRemoveTrainee(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.RemoveTrainee(((Trainee)TraineeList.SelectedItem).Id);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Trainee with ID " + (((Trainee)TraineeList.SelectedItem).Id) + " successfuly deleted.", "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);
            var it = from item in MainWindow.bl.GetTraineeList()
                     where CheckIfStringsAreEqual(FirstName.Text, item.FirstName)
                     select item
                  into g
                     where CheckIfStringsAreEqual(LestName.Text, g.LastName)
                     select g
                  into j
                     where CheckIfStringsAreEqual(ID.Text, j.Id)
                     select j;
            TraineeList.ItemsSource = it;
        }
        private void MenuItem_Click_Information(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(TraineeList.SelectedItem.ToString(), "Trainee Details", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ComboBox_GroupOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_GroupOptions.SelectedIndex)
            {
                case (int)TraineeGroupCategorys.All:
                    ComboBox_GroupNames.IsEnabled = false;
                    ComboBox_GroupNames.SelectedItem = null;
                    listToFilter = mainList;
                    TraineeList.ItemsSource = listToFilter;
                    return;
                case (int)TraineeGroupCategorys.City:
                    groupedByCity = new ObservableCollection<IGrouping<string, Trainee>>(MainWindow.bl.GetTraineessGroupedByCity());
                    //ComboBox_GroupNames.ItemsSource = groupedByCity;
                    string[] keysOfCity = (from item in groupedByCity select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfCity;
                    break;
                case (int)TraineeGroupCategorys.School:
                    groupsBySchool = new ObservableCollection<IGrouping<string, Trainee>>(MainWindow.bl.GetTraineesGroupsBySchool());
                    //ComboBox_GroupNames.ItemsSource = groupedBySeniority;
                    string[] keysOfSchool = (from item in groupsBySchool select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfSchool;
                    break;

                case (int)TraineeGroupCategorys.Teacher:
                    groupsByTeacher = new ObservableCollection<IGrouping<string, Trainee>>(MainWindow.bl.GetTraineesGroupsByTeacher());
                    //ComboBox_GroupNames.ItemsSource = groupedByMaxDistance;
                    string[] keysOfMaxTeacher = (from item in groupsByTeacher select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfMaxTeacher;
                    break;
                case (int)TraineeGroupCategorys.NumOfTests:
                    groupedByNumOfTests = new ObservableCollection<IGrouping<int, Trainee>>(MainWindow.bl.GetTraineesGroupedByNumOfTests());
                    //ComboBox_GroupNames.ItemsSource = groupedByMaxDistance;
                    int[] keysOfMaxNumOfTests = (from item in groupedByNumOfTests select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfMaxNumOfTests;
                    break;
            }
            ComboBox_GroupNames.IsEnabled = true;
            ComboBox_GroupNames.SelectedIndex = 0;
        }
        private void ComboBox_GroupNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_GroupOptions.SelectedIndex)
            {
                case (int)TraineeGroupCategorys.All:
                    mainList = new ObservableCollection<Trainee>(MainWindow.bl.GetTraineeList());
                    listToFilter = mainList;
                    break;
                case (int)TraineeGroupCategorys.City:
                    foreach (var item in groupedByCity)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (string)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Trainee>(item.ToList());
                            break;
                        }
                    break;
                case (int)TraineeGroupCategorys.School:
                    foreach (var item in groupsBySchool)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (string)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Trainee>(item.ToList());
                            break;
                        }
                    break;
                case (int)TraineeGroupCategorys.Teacher:
                    foreach (var item in groupsByTeacher)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (string)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Trainee>(item.ToList());
                            break;
                        }
                    break;
                case (int)TraineeGroupCategorys.NumOfTests:
                    foreach (var item in groupedByNumOfTests)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (int)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<Trainee>(item.ToList());
                            break;
                        }
                    break;
            }
            TraineeList.ItemsSource = listToFilter;
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
    }
}
