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
    /// Interaction logic for GetIDWindow.xaml
    /// </summary>
    public partial class GetIDWindow : Window
    {
        Window parent;
        string type;
        bool isClosedByButton = false;
        public bool IsClosedByButton { get => isClosedByButton; set => isClosedByButton = value; }

        public GetIDWindow(Window parent = null, string type = "")
        {
            this.parent = parent;
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
                MessageBox.Show("ID must be exactly 9 Digitis, and Digits only.", "Wrong input", MessageBoxButton.OK, MessageBoxImage.Information);
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
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        TesterWindow testerWindow = new TesterWindow(tester);
                        testerWindow.Show();
                        Close();
                        parent.Close();
                        break;
                    case "Trainee":
                        Trainee trainee;
                        try
                        {
                            trainee = MainWindow.bl.GetTraineeByID(TxtBx_ID.Text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        TraineeWindow traineeWindow = new TraineeWindow(trainee);
                        traineeWindow.Show();
                        Close();
                        parent.Close();
                        break;
                    case "":
                        isClosedByButton = true;
                        Close();
                        break;
                    default:
                        MessageBox.Show("Internal error. file GetIDWindow, case not exist on switch", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
