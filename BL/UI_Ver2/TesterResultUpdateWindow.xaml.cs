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
            this.FlowDirection = FlowDirection.RightToLeft;
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
                MessageBox.Show("יש למלא את כל השדות!\nשדה הערות הבוחן חייב להכיל לפחות 30 תווים.", "עידכון מבחן", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            Close();
            try
            {
                MainWindow.bl.UpdateTestResult(testResult.TestId, testResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            MessageBox.Show("מבחן עודכן בהצלחה!", "עידכון מבחן", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("האם אתה בטוח שברצונך לבטל?", "ביטול", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) == MessageBoxResult.Yes)
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