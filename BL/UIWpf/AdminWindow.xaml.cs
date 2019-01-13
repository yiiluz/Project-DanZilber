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

namespace UIWpf
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
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

        private void Button_Tester_Click(object sender, RoutedEventArgs e)
        {
            AdminTesterWindow adminTesterWindow = new AdminTesterWindow();
            adminTesterWindow.Show();
            this.Close();
        }

        private void Button_Trainee_Click(object sender, RoutedEventArgs e)
        {
            AdminTraineeWindow adminTraineeWindow = new AdminTraineeWindow();
            adminTraineeWindow.Show();
            this.Close();
        }

        private void Button_Test_Click(object sender, RoutedEventArgs e)
        {
            AdminTestWindow adminTestWindow = new AdminTestWindow();
            adminTestWindow.Show();
            this.Close();
        }

        private void Button_Click_ChangePassword(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow(this, true);
            passwordWindow.ShowDialog();
        }
    }
}
