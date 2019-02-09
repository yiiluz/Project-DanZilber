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

namespace UI_Ver2
{
    /// <summary>
    /// Interaction logic for AdminChangePasswordWindow.xaml
    /// </summary>
    public partial class AdminChangePasswordWindow : Window
    {
        public AdminChangePasswordWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Button_Click_OK(object sender, RoutedEventArgs e)
        {
            bool isNewPassEqual = true;
            switch (CmbBx_PasswordToChange.SelectedIndex)
            {
                case 0:
                    if (TxtBx_OldPassword.Text != App.AdminPass)
                    {
                        MessageBox.Show("הסיסמה הנוכחית שהזנת שגויה. נסה שנית.", "שינוי סיסמה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                        return;
                    }
                    if (TxtBx_NewPassword.Text == TxtBx_NewPasswordVerify.Text)
                    {
                        App.AdminPass = TxtBx_NewPassword.Text;
                        MessageBox.Show("סיסמת חלון מנהל המערכת שונתה בהצלחה!", "שינוי סיסמה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                        Close();
                    }
                    else
                        isNewPassEqual = false;
                    break;
                case 1:
                    if (TxtBx_OldPassword.Text != App.OfficePass)
                    {
                        MessageBox.Show("הסיסמה הנוכחית שהזנת שגויה. נסה שנית.", "שינוי סיסמה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                        return;
                    }
                    if (TxtBx_NewPassword.Text == TxtBx_NewPasswordVerify.Text)
                    {
                        App.OfficePass = TxtBx_NewPassword.Text;
                        MessageBox.Show("סיסמת חלון הניהול משרדי שונתה בהצלחה!", "שינוי סיסמה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                        Close();
                    }
                    else
                        isNewPassEqual = false;
                    break;
            }
            if (!isNewPassEqual)
                MessageBox.Show("הסיסמאות החדשות שהוזנו אינן תואמות! נסה שנית.", "שינוי סיסמה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("האם אתה בטוח שברצונך לבטל? שום שינוי לא יתרחש.", "שינוי סיסמה", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading))
                Close();
        }
    }
}
