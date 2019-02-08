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
            this.FlowDirection = FlowDirection.RightToLeft;
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
                MessageBox.Show("מספר מבחן צריך להיות בעל 8 ספרות בדיוק.", "קלט שגוי", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
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
                            var result = MessageBox.Show(ex.Message + "\nהאם ברצונך לנסות שוב?", "שגיאה", 
                                MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            if (result == MessageBoxResult.No)
                                Close();
                            return;
                        }
                        Close();
                        MessageBox.Show(test.ToString(), "פרטי המבחן", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        break;
                    case "Abort":
                        try
                        {
                            test = MainWindow.bl.GetTestByID(TxtBx_Serial.Text);
                        }
                        catch (Exception ex)
                        {
                            var result = MessageBox.Show(ex.Message + "\nהאם ברצונך לנסות שוב?", "שגיאה",
                                MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            if (result == MessageBoxResult.No)
                                Close();
                            return;
                        }
                        Close();
                        if (test.IsTestAborted)
                        {
                            MessageBox.Show("מבחן זה כבר בוטל!", "התראת כפילות",
                                MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            return;
                        }
                        var isWantToAbort = MessageBox.Show("פרטי המבחן הם:\n" + test.ToString() + "האם אתה בטוח שברצונך לבטל מבחן זה?\n" +
                            "פעולה זו אינה הפיכה.", "ביטול מבחן", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        if (isWantToAbort == MessageBoxResult.Yes)
                        {
                            try
                            {
                                MainWindow.bl.AbortTest(test.TestId);
                            }
                            catch (KeyNotFoundException ex)
                            {
                                var result = MessageBox.Show("שגיאה פנימית\n" + ex.Message + "\nהאם אתה רוצה לנסות שוב?",
                                    "Error", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                                if (result == MessageBoxResult.No)
                                    Close();
                                return;
                            }
                            MessageBox.Show("מבחן מספר " + test.TestId + " בוטל בהצלחה.", "סטטוס פעולה",
                                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        }
                        break;
                    case "Update":
                        try
                        {
                            test = MainWindow.bl.GetTestByID(TxtBx_Serial.Text);
                        }
                        catch (KeyNotFoundException ex)////////////////////////////////////////////
                        {
                            var result = MessageBox.Show(ex.Message + "\nהאם ברצונך לנסות שוב?", "שגיאה",
                                MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
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
                        MessageBox.Show("שגיאה פנימית בחלונית GetSerialWindow בגלל case שאינו קיים.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error
                            , MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
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

