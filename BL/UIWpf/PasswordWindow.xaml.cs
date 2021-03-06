﻿using System;
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
        private static string password = "1111";
        private Window main;
        private bool toChangePass;
        public PasswordWindow(Window main, bool ChangePassword = false)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.main = main;
            InitializeComponent();
            toChangePass = ChangePassword;
            if (toChangePass)
            {
                LableOfInput.Content = "Enter The New Password:";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (toChangePass)
            {
                password = passwordBox.Password;
                MessageBox.Show("Password changed successfuly!", "Password Changing", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
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

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
