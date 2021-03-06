﻿using BO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace UI_Ver2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string officePass;
        static IBL bl = BO.Factory.GetBLObj();
        private static string adminPass;
        public static int numOfActivatedMainWindow = 0;
        static App()
        {
            
        }
        public static void SetPasswords()
        {
            try
            {
                adminPass = bl.GetConfig()["סיסמת מנהל המערכת"].ToString();
                officePass = bl.GetConfig()["סיסמת ניהול משרדי"].ToString();
            }
            catch
            {
                MessageBox.Show("לא מצליח לטעון סיסמאות", "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                Environment.Exit(Environment.ExitCode);
            }
        }
        public static string AdminPass
        {
            get => adminPass; set
            {
                try
                {
                    bl.SetConfig("סיסמת מנהל המערכת", value);
                    adminPass = value;
                }
                catch
                {
                    MessageBox.Show("לא מצליח לשנות את סיסמת מנהל המערכת!", "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                }
            }

        }
        public static string OfficePass
        {
            get => officePass; set
            {
                try
                {
                    bl.SetConfig("סיסמת ניהול משרדי", value);
                    adminPass = value;
                }
                catch
                {
                    MessageBox.Show("לא מצליח לשנות את סיסמת הניהול המשרדי!", "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                }
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("שגיאה פנימית. " + e.Exception.Message, "שגיאה שלא טופלה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            e.Handled = true;
            if (numOfActivatedMainWindow == 0)
                (new MainWindow()).Show();
        }
    }
}
