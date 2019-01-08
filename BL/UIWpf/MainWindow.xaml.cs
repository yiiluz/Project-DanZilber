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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;

namespace UIWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static IBL bl = BL.Factory.GetBLObj();
        public MainWindow()
        {
            InitializeComponent();
        }

        //********************************************************************************************************************
        private void Button_Click_AddTrainee(object sender, RoutedEventArgs e)
        {
            TraineeWindow traineeWindow = new TraineeWindow();
            traineeWindow.ShowDialog();
        }

        private void Button_Click_PrintTraineeByID(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            if (getIDWindow.IsClosedByButton)
            {
                Trainee traine = null;
                try
                {
                    traine = bl.GetTraineeByID(getIDWindow.TxtBx_ID.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show(traine.ToString());
            }
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
                    traine = bl.GetTraineeByID(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                TraineeWindow traineeWindow = new TraineeWindow(traine);
                traineeWindow.ShowDialog();
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
                    bl.RemoveTrainee(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Trainee with ID " + getIDWindow.TxtBx_ID.Text + " successfuly deleted.", "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //*********************************************************************************************************************

        private void Button_Click_AddTester(object sender, RoutedEventArgs e)
        {
            TesterWindow testerWindow = new TesterWindow();
            testerWindow.ShowDialog();
        }

        private void Button_Click_PrintTesterByID(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            if (getIDWindow.IsClosedByButton)
            {
                Tester tester = null;
                try
                {
                    tester = bl.GetTesterByID(getIDWindow.TxtBx_ID.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show(tester.ToString());
            }
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
                    tester = bl.GetTesterByID(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                TesterWindow testerWindow = new TesterWindow(tester);
                testerWindow.ShowDialog();
            }
        }

        private void Button_Click_RemoveTester(object sender, RoutedEventArgs e)
        {
            GetIDWindow getIDWindow = new GetIDWindow();
            getIDWindow.ShowDialog();
            if (getIDWindow.IsClosedByButton)
            {
                try
                {
                    bl.RemoveTester(getIDWindow.TxtBx_ID.Text);
                }
                catch (KeyNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "ID not Exist", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Tester with ID " + getIDWindow.TxtBx_ID.Text + " successfuly deleted.", "Delete Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_AddTest(object sender, RoutedEventArgs e)
        {
            TestWindow testWindow = new TestWindow();
            testWindow.ShowDialog();
        }

        //*********************************************************************************************
    }
}
