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

namespace UIWpf
{
    /// <summary>
    /// Interaction logic for GetIDWindow.xaml
    /// </summary>
    public partial class GetIDWindow : Window
    {
        private bool isClosedByButton = false;
        public bool IsClosedByButton { get => isClosedByButton; }
        public GetIDWindow()
        {
            InitializeComponent();
        }

        private void TxtBx_ID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_ID.Background != Brushes.White)
                TxtBx_ID.Background = Brushes.White;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TxtBx_ID.Text.Length != 9)
                TxtBx_ID.Background = Brushes.Red;
            else if (!TxtBx_ID.Text.All(char.IsDigit))
                TxtBx_ID.Background = Brushes.Red;
            else
            {
                isClosedByButton = true;
                Close();
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
    }
}
