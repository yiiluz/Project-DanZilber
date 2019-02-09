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
    /// Interaction logic for SetConfigWindow.xaml
    /// </summary>
    public partial class SetConfigWindow : Window
    {
        string configurationName;
        public SetConfigWindow(KeyValuePair<string, Object> conf)
        {
            this.FlowDirection = FlowDirection.RightToLeft;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            configurationName = conf.Key;
            InitializeComponent();
            TextBlock_ConfigName.Text = "שנה את " + configurationName + " :";
            TextBox_OldValue.Text = conf.Value.ToString();
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("האם את/ה בטוח/ה שברצונך לבטל?\nשום שינוי לא התרחש.", "סטטוס הפעולה", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign))
                Close();
        }

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            int temp;
            if (!int.TryParse(TextBox_NewValue.Text, out temp))
            {
                MessageBox.Show("הערך החדש יהיה מספר שלם בלבד!", "שגיאה בקלט", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            try
            {
                MainWindow.bl.SetConfig(configurationName, TextBox_NewValue.Text);
                Close();
                MessageBox.Show(configurationName + " עודכן בצהלחה!", "סטטוס פעולה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            catch (AccessViolationException)
            {
                MessageBox.Show("ההגדרה הזו אינה ניתנת לשינוי!", "סטטוס פעולה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show("התרחשה שגיאה פנימית.\nפרטים נוספים: " + ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
