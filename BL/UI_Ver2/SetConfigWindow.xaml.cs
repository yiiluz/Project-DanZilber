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
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            configurationName = conf.Key;
            InitializeComponent();
            TextBlock_ConfigName.Text = "Cange " + configurationName + ":";
            TextBox_OldValue.Text = conf.Value.ToString();
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to cancel?\nThe configuration hasn't changed.", "Operation Status", MessageBoxButton.YesNo, MessageBoxImage.Question))
                Close();
        }

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            int temp;
            if (!int.TryParse(TextBox_NewValue.Text, out temp))
            {
                MessageBox.Show("The new value must be Integer number only!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                MainWindow.bl.SetConfig(configurationName, TextBox_NewValue.Text);
                Close();
                MessageBox.Show(configurationName + " Successfuly Updated!", "Update Status", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            catch (AccessViolationException)
            {
                MessageBox.Show("This Configuration is NOT Writeable!");
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show("Internal Error. Something went wrong.\n" + ex.Message);
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
