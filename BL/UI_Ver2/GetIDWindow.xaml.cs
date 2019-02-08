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
    /// Interaction logic for GetIDWindow.xaml
    /// </summary>
    public partial class GetIDWindow : Window
    {
        string type;
        bool isClosedByButton = false;
        public bool IsClosedByButton { get => isClosedByButton; set => isClosedByButton = value; }

        public GetIDWindow(string type = "")
        {
            this.FlowDirection = FlowDirection.RightToLeft;
            this.type = type;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void TxtBx_ID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_ID.Background != Brushes.White)
                TxtBx_ID.Background = Brushes.White;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TxtBx_ID.Text.Length != TxtBx_ID.MaxLength || !TxtBx_ID.Text.All(char.IsDigit))
            {
                TxtBx_ID.Background = Brushes.Red;
                MessageBox.Show("מספר תעודת הזהות צריך להיות 9 ספרות בדיוק.", "קלט שגוי", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            else
            {
                switch (this.type)
                {
                    case "Tester":
                        Tester tester;
                        try
                        {
                            tester = MainWindow.bl.GetTesterByID(TxtBx_ID.Text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            return;
                        }
                        Close();
                        break;
                    case "Trainee":
                        Trainee trainee;
                        try
                        {
                            trainee = MainWindow.bl.GetTraineeByID(TxtBx_ID.Text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            return;
                        }
                        Close();
                        break;
                    case "":
                        isClosedByButton = true;
                        Close();
                        break;
                    default:
                        MessageBox.Show("שגיאה פנימית. case לא קיים", "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
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
            UserImage.Height += 15;
            UserImage.Width += 15;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            UserImage.Height -= 15;
            UserImage.Width -= 15;
        }

        private void TxtBx_ID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)

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
