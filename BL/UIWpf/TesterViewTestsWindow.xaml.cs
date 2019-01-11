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
    /// Interaction logic for TesterViewTestsWindow.xaml
    /// </summary>
    public partial class TesterViewTestsWindow : Window
    {
        public TesterViewTestsWindow(Tester tester)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.DataContext = tester;
        }

        private void TestsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show((TestsList.SelectedItem as TesterTest).ToString(), "Test Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
