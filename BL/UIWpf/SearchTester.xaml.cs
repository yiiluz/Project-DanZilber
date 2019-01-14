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
    public partial class SearchTester : Window
    {      
        public SearchTester()
        {
            
            InitializeComponent();
            this.TestersList.ItemsSource = MainWindow.bl.GetTestersList();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            empty = false;
            textBlock.DataContext = empty;        
        }
        bool empty;
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
        }
        
        private void FirstNameTextChanged(object sender, TextChangedEventArgs e)
        {
            var it = from item in MainWindow.bl.GetTestersList() where ChackIfStringsAreEqual(FirstName.Text,item.FirstName) select item 
                     into g where ChackIfStringsAreEqual(LestName.Text,g.LastName) select g
                     into j where ChackIfStringsAreEqual(ID.Text,j.Id) select j;
            TestersList.ItemsSource = it;
            if(TestersList.Items.Count == 0)
            {
                empty = true;
            }
        }

        private void LestNameTextChanged(object sender, TextChangedEventArgs e)
        {
            var it = from item in MainWindow.bl.GetTestersList()
                     where ChackIfStringsAreEqual(FirstName.Text, item.FirstName)
                     select item
                    into g
                     where ChackIfStringsAreEqual(LestName.Text, g.LastName)
                     select g
                    into j
                     where ChackIfStringsAreEqual(ID.Text, j.Id)
                     select j;
            TestersList.ItemsSource = it;
            if (TestersList.Items.Count == 0)
            {
                empty = true;
            }
        }

        private void IDTextChanged(object sender, TextChangedEventArgs e)
        {
            var it = from item in MainWindow.bl.GetTestersList()
                     where ChackIfStringsAreEqual(FirstName.Text, item.FirstName)
                     select item
                    into g
                     where ChackIfStringsAreEqual(LestName.Text, g.LastName)
                     select g
                    into j
                     where ChackIfStringsAreEqual(ID.Text, j.Id)
                     select j;
            TestersList.ItemsSource = it;
            if (TestersList.Items.Count == 0)
            {
                empty = true;
            }
        }

        private void MenuItem_ClickRemoveTester(object sender, RoutedEventArgs e)
        {
            MainWindow.bl.RemoveTester((TestersList.SelectedItem as Tester).Id);
            MessageBox.Show("The tester has been successfully deleted from the system!", "SearchTester", MessageBoxButton.OK, MessageBoxImage.Information);
            var it = from item in MainWindow.bl.GetTestersList()
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

        private void MenuItem_Click_Information(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(TestersList.SelectedItem.ToString(), "SearchItem", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
