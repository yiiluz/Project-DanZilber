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
    /// Interaction logic for TesterResultUpdateWindow.xaml
    /// </summary>
    public partial class TesterResultUpdateWindow : Window
    {
        private TesterTest testResult;
        public TesterResultUpdateWindow(TesterTest test)
        {
            InitializeComponent();
            this.testResult = test;
            this.DataContext = testResult;
        }
        private bool MissingDetails()
        {
            if (DistanceKeeping1.IsChecked == DistanceKeeping2.IsChecked) { return false; }
            if (ReverseParking1.IsChecked == ReverseParking2.IsChecked) { return false; }
            if (MirrorsCheck1.IsChecked == MirrorsChec2.IsChecked) { return false; }
            if (CorrectSpeed1.IsChecked == CorrectSpeed2.IsChecked) { return false; }
            if (IsPassed1.IsChecked == IsPassed2.IsChecked) { return false; }
            if (Signals1.IsChecked == Signals2.IsChecked) { return false; }
            return true;
        }
        private void ClickUpdate(object sender, RoutedEventArgs e)
        {
            if (MissingDetails() == false)
            {
                MessageBox.Show("All details must be entered!","Test Update", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Close();
            MessageBox.Show("Test successfully updated!", "Test Update",MessageBoxButton.OK, MessageBoxImage.Information);
            MainWindow.bl.UpdateTestResult(testResult.TestId, testResult);
        }
    }
}