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
using BO;
namespace UIWpf
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminTesterWindow : Window
    {
        public AdminTesterWindow()
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

        private void Button_Click_AddTester(object sender, RoutedEventArgs e)
        {
            TesterDetailsWindow testerDetailsWindow = new TesterDetailsWindow();
            testerDetailsWindow.ShowDialog();
        }

        private void Button_Click_UpdateTester(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            if (getIDWindow.IsClosedByButton)
            {
                Tester tester;
                try
                {
                    tester = MainWindow.bl.GetTesterByID(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                TesterDetailsWindow testerDetailsWindow = new TesterDetailsWindow(tester, "Update");
                testerDetailsWindow.ShowDialog();
            }
        }

        private void Button_Click_RemoveTester(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            List<TesterTest> abortedTests;
            if (getIDWindow.IsClosedByButton)
            {
                try
                {
                    abortedTests = MainWindow.bl.RemoveTester(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string aborted = "";
                foreach (var item in abortedTests)
                    aborted += "Test Serial: " + item.TestId + ". Date: " + item.DateOfTest.ToShortDateString() + ". Hour: " + item.HourOfTest + ":00.\n";
                MessageBox.Show("Tester with ID " + getIDWindow.TxtBx_ID.Text + " successfuly deleted.\n"
                    + "Aborted Tests:\n" + aborted, "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_ViewTestersLists(object sender, RoutedEventArgs e)
        {
            AdminViewTesterListWindow adminViewTesterListWindow = new AdminViewTesterListWindow();
            adminViewTesterListWindow.ShowDialog();
        }
        private void Button_Click_SearchTesterByName(object sender, RoutedEventArgs e)
        {
            AdminSearchTester searchTester = new AdminSearchTester();
            searchTester.ShowDialog();
        }
    }
}
