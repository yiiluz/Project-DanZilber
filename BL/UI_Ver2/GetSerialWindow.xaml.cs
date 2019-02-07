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
    /// Interaction logic for GetSerialWindow.xaml
    /// </summary>
    public partial class GetSerialWindow : Window
    {
        string oper;
        bool isClosedByButton = false;
        public bool IsClosedByButton { get => isClosedByButton; set => isClosedByButton = value; }

        public GetSerialWindow(string oper = "")
        {
            this.oper = oper;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void TxtBx_ID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_Serial.Background != Brushes.White)
                TxtBx_Serial.Background = Brushes.White;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TxtBx_Serial.Text.Length != TxtBx_Serial.MaxLength || !TxtBx_Serial.Text.All(char.IsDigit))
            {
                TxtBx_Serial.Background = Brushes.Red;
                MessageBox.Show("ID must be exactly 9 Digitis, and Digits only.", "Wrong input", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Test test;
                switch (this.oper)
                {
                    case "View":
                        try
                        {
                            test = MainWindow.bl.GetTestByID(TxtBx_Serial.Text);
                        }
                        catch (Exception ex)
                        {
                            var result = MessageBox.Show(ex.Message + "\nDo you want to try again?", "Error", 
                                MessageBoxButton.YesNo, MessageBoxImage.Error);
                            if (result == MessageBoxResult.No)
                                Close();
                            return;
                        }
                        Close();
                        MessageBox.Show(test.ToString(), "Test Details", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Abort":
                        try
                        {
                            test = MainWindow.bl.GetTestByID(TxtBx_Serial.Text);
                        }
                        catch (Exception ex)
                        {
                            var result = MessageBox.Show(ex.Message + "\nDo you want to try again?", "Error",
                                MessageBoxButton.YesNo, MessageBoxImage.Error);
                            if (result == MessageBoxResult.No)
                                Close();
                            return;
                        }
                        Close();
                        if (test.IsTestAborted)
                        {
                            MessageBox.Show("The test allready Aborted!", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        var isWantToAbort = MessageBox.Show("Test details are:\n" + test.ToString() + "Are you sure you want to abort this test?" +
                            " This action is not reversible."
                            , "Abort Test", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (isWantToAbort == MessageBoxResult.Yes)
                        {
                            try
                            {
                                MainWindow.bl.AbortTest(test.TestId);
                            }
                            catch (KeyNotFoundException ex)
                            {
                                var result = MessageBox.Show("internal error\n" + ex.Message + "\nDo you want to try again?",
                                    "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                                if (result == MessageBoxResult.No)
                                    Close();
                                return;
                            }
                            MessageBox.Show("The test with id " + test.TestId + " successfuly Aborted", "Operation Status",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case "Update":
                        try
                        {
                            test = MainWindow.bl.GetTestByID(TxtBx_Serial.Text);
                        }
                        catch (KeyNotFoundException ex)////////////////////////////////////////////
                        {
                            var result = MessageBox.Show(ex.Message + "\nDo you want to try again?", "Error",
                                MessageBoxButton.YesNo, MessageBoxImage.Error);
                            if (result == MessageBoxResult.No)
                                Close();
                            return;
                        }
                        Close();
                        TesterResultUpdateWindow testerResultUpdateWindow = new TesterResultUpdateWindow(new TesterTest(test));
                        testerResultUpdateWindow.ShowDialog();
                        isClosedByButton = true;
                        break;
                    default:
                        MessageBox.Show("Internal error. file GetIDWindow, case not exist on switch", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        isClosedByButton = false;
                        break;
                }

            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).FontSize += 2;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).FontSize -= 2;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            CarImage.Height += 15;
            CarImage.Width += 15;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            CarImage.Height -= 15;
            CarImage.Width -= 15;
        }

        private void TxtBx_Serial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, new RoutedEventArgs());
            }
        }
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

