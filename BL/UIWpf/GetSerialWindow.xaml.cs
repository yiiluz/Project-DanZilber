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
    /// Interaction logic for GetSerialWindow.xaml
    /// </summary>
    public partial class GetSerialWindow : Window
    {

        Window parent;
        string oper;
        bool isClosedByButton = false;
        public bool IsClosedByButton { get => isClosedByButton; set => isClosedByButton = value; }

        public GetSerialWindow(Window parent = null, string oper = "")
        {
            this.parent = parent;
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
                        var isWantToAbort = MessageBox.Show("Test details are:\n" + test.ToString() + "Are you sure you want to abort this test?" +
                            " This action is not reversible."
                            , "Abort Test", MessageBoxButton.YesNo, MessageBoxImage.Information);
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
                    default:
                        MessageBox.Show("Internal error. file GetIDWindow, case not exist on switch", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        isClosedByButton = false;
                        break;
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
            CarImage.Height += 15;
            CarImage.Width += 15;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            CarImage.Height -= 15;
            CarImage.Width -= 15;
        }
    }
}

