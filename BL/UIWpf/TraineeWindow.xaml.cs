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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class TraineeWindow : Window
    {
        Trainee trainee;
        public TraineeWindow(Trainee trainee)
        {
            this.trainee = trainee;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void Button_Back_MouseEnter(object sender, MouseEventArgs e)
        {
            this.BackImage.Height += 5;
            this.BackImage.Width += 5;
        }

        private void Button_Back_MouseLeave(object sender, MouseEventArgs e)
        {
            this.BackImage.Height -= 5;
            this.BackImage.Width -= 5;
        }

        private void Button_Click_AddTest(object sender, RoutedEventArgs e)
        {
            AddTestWindow testWindow = new AddTestWindow();
            testWindow.TxtBx_ID.Text = trainee.Id;
            testWindow.TxtBx_ID.IsEnabled = false;
            testWindow.TxtBx_City.Text = trainee.Address.City;
            testWindow.TxtBx_Street.Text = trainee.Address.Street;
            testWindow.TxtBx_BuildNum.Text = trainee.Address.BuildingNumber.ToString();
            testWindow.ShowDialog();
            trainee = MainWindow.bl.GetTraineeByID(trainee.Id);
        }

        private void Button_Click_ViewTest(object sender, RoutedEventArgs e)
        {
            if (trainee.TestList.Count == 0)
            {
                MessageBox.Show("There is no Tests to show yet.", "Nothing to show", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                TraineeViewTestsWindow traineeViewTestsWindow = new TraineeViewTestsWindow(trainee);
                traineeViewTestsWindow.ShowDialog();
            }
        }

        private void Button_Click_ViewTraineeDetails(object sender, RoutedEventArgs e)
        {
            TraineeDetailsWindow traineeDetailsWindow = new TraineeDetailsWindow(trainee, "View");
            traineeDetailsWindow.ShowDialog();
        }

        private void Button_Click_ViewLicence(object sender, RoutedEventArgs e)
        {
            string existingLicenses = "";
            try
            {
                existingLicenses = MainWindow.bl.GetStringOfTraineeLicenses(trainee.Id);
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("internal error on view licenses func", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (existingLicenses != "")
                MessageBox.Show("Your Licenses:\n" + existingLicenses, "Licenses", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("You have no licenses yet.", "Nothing To Show", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
