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
    public partial class AdminTraineeWindow : Window
    {
        public AdminTraineeWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            this.Close();
            adminWindow.Show();
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

        private void Button_Click_AddTrainee(object sender, RoutedEventArgs e)
        {
            TraineeDetailsWindow traineeDetailsWindow = new TraineeDetailsWindow();
            traineeDetailsWindow.ShowDialog();
        }

        private void Button_Click_UpdateTrainee(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            if (getIDWindow.IsClosedByButton)
            {
                Trainee traine;
                try
                {
                    traine = MainWindow.bl.GetTraineeByID(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                TraineeDetailsWindow traineeDetailsWindow = new TraineeDetailsWindow(traine, "Update");
                traineeDetailsWindow.ShowDialog();
            }
        }

        private void Button_Click_RemoveTrainee(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            if (getIDWindow.IsClosedByButton)
            {
                try
                {
                    MainWindow.bl.RemoveTrainee(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Trainee with ID " + getIDWindow.TxtBx_ID.Text + " successfuly deleted.", "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_ViewAllTrainees(object sender, RoutedEventArgs e)
        {
            AdminViewTraineesListWindow adminViewTraineesListWindow = new AdminViewTraineesListWindow();
            adminViewTraineesListWindow.ShowDialog();
        }

        private void Button_Click_SearchTrainee(object sender, RoutedEventArgs e)
        {
            SearchTrainee searchTrainee = new SearchTrainee();
            searchTrainee.ShowDialog();
        }
    }
}
