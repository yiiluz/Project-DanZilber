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
    /// Interaction logic for TesterResultUpdateWindow.xaml
    /// </summary>
    public partial class TesterResultUpdateWindow : Window
    {
        private TesterTest testResult;
        public TesterResultUpdateWindow(TesterTest test)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.testResult = test;
            this.DataContext = testResult;
        }
        private bool IsMissingDetails()
        {
            if (DistanceKeeping1.IsChecked == DistanceKeeping2.IsChecked) { return false; }
            if (ReverseParking1.IsChecked == ReverseParking2.IsChecked) { return false; }
            if (MirrorsCheck1.IsChecked == MirrorsChec2.IsChecked) { return false; }
            if (CorrectSpeed1.IsChecked == CorrectSpeed2.IsChecked) { return false; }
            if (IsPassed1.IsChecked == IsPassed2.IsChecked) { return false; }
            if (Signals1.IsChecked == Signals2.IsChecked) { return false; }
            if (TesterNotes.Text.Length < 30) { return false; }
            return true;
        }
        private void ClickUpdate(object sender, RoutedEventArgs e)
        {
            if (IsMissingDetails() == false)
            {
                MessageBox.Show("All details must be entered! Payattention, Notes must be unlist 30 characters", "Test Update", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Close();
            try
            {
                MainWindow.bl.UpdateTestResult(testResult.TestId, testResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Test successfully updated!", "Test Update", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Cancel?", "Cancel Option", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}