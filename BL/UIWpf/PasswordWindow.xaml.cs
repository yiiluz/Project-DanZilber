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
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private string password = "Project5779";
        private MainWindow main;
        public PasswordWindow(MainWindow main)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.main = main;
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password == password)
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                main.Close();
                Close();
            }
            else
            {
                MessageBox.Show("Wrong Password! Try agein", "Security Alarm", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void Button_OK_MouseEnter(object sender, MouseEventArgs e)
        {
            Button_OK.FontSize += 4;
        }

        private void Button_OK_MouseLeave(object sender, MouseEventArgs e)
        {
            Button_OK.FontSize -= 4;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            LockImage.Height += 15;
            LockImage.Width += 15;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            LockImage.Height -= 15;
            LockImage.Width -= 15;
        }
    }
}
