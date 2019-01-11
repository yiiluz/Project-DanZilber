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
    /// Interaction logic for TraineeViewTestsWindow.xaml
    /// </summary>
    public partial class TraineeViewTestsWindow : Window
    {
        public TraineeViewTestsWindow(Trainee trainee)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.DataContext = trainee;
        }

        private void TestsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show((TestsList.SelectedItem as TraineeTest).ToString(), "Test Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
