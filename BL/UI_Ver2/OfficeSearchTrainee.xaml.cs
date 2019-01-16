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
namespace UI_Ver2
{
    /// <summary>
    /// Interaction logic for SearchTrainee.xaml
    /// </summary>
    public partial class OfficeSearchTrainee : Window
    {
        public OfficeSearchTrainee()
        {
            InitializeComponent();
            this.TraineeList.ItemsSource = MainWindow.bl.GetTraineeList();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
         private bool ChackIfStringsAreEqual(string a, string b)
        {
            int c = Math.Min(a.Length, b.Length);
            a = a.ToLower();
            b = b.ToLower();
            for (int i = 0; i <c; i++)
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

        private void FirstNameTextChanged(object sender, TextChangedEventArgs e)
        {
            var it = from item in MainWindow.bl.GetTraineeList()
                     where ChackIfStringsAreEqual(FirstName.Text, item.FirstName)
                     select item
                     into g
                     where ChackIfStringsAreEqual(LestName.Text, g.LastName)
                     select g
                     into j
                     where ChackIfStringsAreEqual(ID.Text, j.Id)
                     select j;
           TraineeList.ItemsSource = it;
        }

        private void LestNameTextChanged(object sender, TextChangedEventArgs e)
        {
            var it = from item in MainWindow.bl.GetTraineeList()
                     where ChackIfStringsAreEqual(FirstName.Text, item.FirstName)
                     select item
                    into g
                     where ChackIfStringsAreEqual(LestName.Text, g.LastName)
                     select g
                    into j
                     where ChackIfStringsAreEqual(ID.Text, j.Id)
                     select j;
            TraineeList.ItemsSource = it;
        }

        private void IDTextChanged(object sender, TextChangedEventArgs e)
        {
            var it = from item in MainWindow.bl.GetTraineeList()
                     where ChackIfStringsAreEqual(FirstName.Text, item.FirstName)
                     select item
                    into g
                     where ChackIfStringsAreEqual(LestName.Text, g.LastName)
                     select g
                    into j
                     where ChackIfStringsAreEqual(ID.Text, j.Id)
                     select j;
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
                     where ChackIfStringsAreEqual(FirstName.Text, item.FirstName)
                     select item
                  into g
                     where ChackIfStringsAreEqual(LestName.Text, g.LastName)
                     select g
                  into j
                     where ChackIfStringsAreEqual(ID.Text, j.Id)
                     select j;
            TraineeList.ItemsSource = it;
        }

        private void MenuItem_Click_Information(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(TraineeList.SelectedItem.ToString(), "Trainee Details", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void buttonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
