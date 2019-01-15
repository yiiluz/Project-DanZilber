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
    /// Interaction logic for SearchTester.xaml
    /// </summary>

    enum GroupCategorys { All, Seniority, City, MaxDistance }
    public partial class AdminSearchTester : Window
    {
        private List<Tester> mainList = MainWindow.bl.GetTestersList();
        private IEnumerable<IGrouping<string, Tester>> groupedByCity = MainWindow.bl.GetTestersGroupedByCity();
        private IEnumerable<IGrouping<int, Tester>> groupedBySeniority = MainWindow.bl.GetTestersGropedBySeniority();
        private IEnumerable<IGrouping<int, Tester>> groupedByMaxDistance = MainWindow.bl.GetTestersGropedByMaxDistance();

        private List<Tester> listToFilter;

        public AdminSearchTester()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.TestersList.ItemsSource = MainWindow.bl.GetTestersList();
            this.ComboBox_GroupOptions.ItemsSource = Enum.GetValues(typeof(GroupCategorys));
            this.ComboBox_GroupOptions.SelectedIndex = 0;
            ComboBox_GroupNames.IsEnabled = false;
            listToFilter = mainList;
        }

        private bool ChackIfStringsAreEqual(string a, string b)
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
        private void MenuItem_Click_UpdateTesterDetails(object sender, RoutedEventArgs e)
        {
            Tester tester;
            tester = MainWindow.bl.GetTesterByID((TestersList.SelectedItem as Tester).Id);
            TesterDetailsWindow testerDetailsWindow = new TesterDetailsWindow(tester, "Update");
            testerDetailsWindow.ShowDialog();
            mainList = MainWindow.bl.GetTestersList();
            groupedByCity = MainWindow.bl.GetTestersGroupedByCity();
            groupedBySeniority = MainWindow.bl.GetTestersGropedBySeniority();
            groupedByMaxDistance = MainWindow.bl.GetTestersGropedByMaxDistance();
            TestersList.ItemsSource = mainList;
            SearchFilterChanged(this, null);
            
        }

        private void SearchFilterChanged(object sender, TextChangedEventArgs e)
        {
            var it = from item in listToFilter
                     where ChackIfStringsAreEqual(FirstName.Text, item.FirstName)
                     select item
                     into g
                     where ChackIfStringsAreEqual(LestName.Text, g.LastName)
                     select g
                     into j
                     where ChackIfStringsAreEqual(ID.Text, j.Id)
                     select j;
            TestersList.ItemsSource = it;
        }

        private void MenuItem_ClickRemoveTester(object sender, RoutedEventArgs e)
        {
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

                mainList = MainWindow.bl.GetTestersList();
                groupedByCity = MainWindow.bl.GetTestersGroupedByCity();
                groupedBySeniority = MainWindow.bl.GetTestersGropedBySeniority();
                groupedByMaxDistance = MainWindow.bl.GetTestersGropedByMaxDistance();
                listToFilter.Remove((Tester)TestersList.SelectedItem);
                TestersList.ItemsSource = listToFilter;
                SearchFilterChanged(this, null);
            }
        }

        private void MenuItem_Click_Information(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(TestersList.SelectedItem.ToString(), "SearchItem", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComboBox_GroupOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_GroupOptions.SelectedIndex)
            {
                case (int)GroupCategorys.All:
                    ComboBox_GroupNames.IsEnabled = false;
                    ComboBox_GroupNames.SelectedItem = null;
                    listToFilter = mainList;
                    TestersList.ItemsSource = listToFilter;
                    return;
                case (int)GroupCategorys.Seniority:
                    //ComboBox_GroupNames.ItemsSource = groupedBySeniority;
                    int[] keysOfSeniority = (from item in groupedBySeniority select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfSeniority;
                    break;
                case (int)GroupCategorys.City:
                    //ComboBox_GroupNames.ItemsSource = groupedByCity;
                    string[] keysOfCity = (from item in groupedByCity select item.Key).ToArray();
                    ComboBox_GroupNames.ItemsSource = keysOfCity;
                    break;
                case (int)GroupCategorys.MaxDistance:
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
                case (int)GroupCategorys.All:
                    return;
                case (int)GroupCategorys.Seniority:
                    foreach (var item in groupedBySeniority)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (int)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = item.ToList();
                            break;
                        }
                    break;
                case (int)GroupCategorys.City:
                    foreach (var item in groupedByCity)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (string)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = item.ToList();
                            break;
                        }
                    break;
                case (int)GroupCategorys.MaxDistance:
                    foreach (var item in groupedByMaxDistance)
                        if (ComboBox_GroupNames.SelectedItem != null && item.Key == (int)ComboBox_GroupNames.SelectedItem)
                        {
                            listToFilter = item.ToList();
                            break;
                        }
                    break;

            }
            TestersList.ItemsSource = listToFilter;
        }
    }
}
