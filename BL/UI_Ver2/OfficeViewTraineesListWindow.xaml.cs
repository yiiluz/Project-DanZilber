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
    /// Interaction logic for AdminViewTraineesListWindow.xaml
    /// </summary>
    public partial class OfficeViewTraineesListWindow : Window
    {
        List<Trainee> traineeList = MainWindow.bl.GetTraineeList();
        public OfficeViewTraineesListWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.DataContext = traineeList;
            InitializeComponent();
        }

        private void TestsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show((ListBox_TraineesList.SelectedItem).ToString(), "Trainee Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
             



        private void MenuItem_Click_UpdateTrainee(object sender, RoutedEventArgs e)
        {
            TraineeDetailsWindow traineeDetailsWindow = new TraineeDetailsWindow((Trainee)ListBox_TraineesList.SelectedItem, "Update");
            traineeDetailsWindow.ShowDialog();
        }

        private void MenuItem_Click_RemoveTrainee(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.bl.RemoveTrainee(((Trainee)(ListBox_TraineesList.SelectedItem)).Id);
                traineeList = MainWindow.bl.GetTraineeList();
                ListBox_TraineesList.ItemsSource = traineeList;
                MessageBox.Show("The trainee has been successfully deleted from the system", "AdminViewTraineeListWindow", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(KeyNotFoundException ee)
            {
                MessageBox.Show(ee.Message, "AdminViewTraineesList", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_Click_AddTestToTrainee(object sender, RoutedEventArgs e)
        {
            AddTestWindow testWindow = new AddTestWindow();
            testWindow.TxtBx_ID.Text = (ListBox_TraineesList.SelectedItem as Trainee).Id;
            testWindow.TxtBx_ID.IsEnabled = false;
            testWindow.TxtBx_City.Text = (ListBox_TraineesList.SelectedItem as Trainee).City;
            testWindow.TxtBx_Street.Text = (ListBox_TraineesList.SelectedItem as Trainee).Street;
            testWindow.TxtBx_BuildNum.Text = (ListBox_TraineesList.SelectedItem as Trainee).BuildingNumber.ToString();
            testWindow.ShowDialog();
        }
    }
}
