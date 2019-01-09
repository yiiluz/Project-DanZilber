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
    /// Interaction logic for TesterWindow.xaml
    /// </summary>
    public partial class TesterDetailsWindow : Window
    {
        public Tester tester;
        public string operation = "Add";
        public TesterDetailsWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tester = new Tester("");
            this.DataContext = tester;
            InitializeComponent();
            CombBx_TypeCarToTest.ItemsSource = Enum.GetValues(typeof(BO.CarTypeEnum));
            CombBx_Gender.ItemsSource = Enum.GetValues(typeof(BO.GenderEnum));
            CombBx_TypeCarToTest.SelectedItem = BO.CarTypeEnum.MotorCycle;
            CombBx_Gender.SelectedItem = BO.GenderEnum.Male;
            foreach (var item in HoursWork.Children.OfType<CheckBox>())//intial checkboxs
            {
                int row = Grid.GetRow(item);
                int column = Grid.GetColumn(item);
                item.IsChecked = tester.AvailiableWorkTime[row, column];
            }
        }
        public TesterDetailsWindow(Tester t)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            operation = "Update";
            tester = t;
            this.DataContext = tester;
            InitializeComponent();
            TxtBx_ID.IsEnabled = false;
            Button_OK.Content = "Update";
            CombBx_TypeCarToTest.ItemsSource = Enum.GetValues(typeof(BO.CarTypeEnum));
            CombBx_Gender.ItemsSource = Enum.GetValues(typeof(BO.GenderEnum));
            foreach (var item in HoursWork.Children.OfType<CheckBox>())//initial checkBoxs
            {
                int row = Grid.GetRow(item);
                int column = Grid.GetColumn(item);
                item.IsChecked = tester.AvailiableWorkTime[row, column];
            }
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            double maxD;
            if (
                (!TxtBx_ID.Text.All(char.IsDigit) || (TxtBx_ID.Text.Length != 9)) ||
                (TxtBx_FirstName.Text.Length == 0 || !TxtBx_FirstName.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                (TxtBx_LastName.Text.Length == 0 || !TxtBx_LastName.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                (!TxtBx_Phone.Text.All(char.IsDigit) || (TxtBx_Phone.Text.Length != 10)) ||
                (TxtBx_Street.Text.Length == 0 || !TxtBx_Street.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                (!TxtBx_BuildNum.Text.All(char.IsDigit) || (TxtBx_BuildNum.Text.Length == 0)) ||
                (!double.TryParse(TxtBx_MaxDistance.Text, out maxD) || (TxtBx_MaxDistance.Text.Length == 0)) ||
                (!TxtBx_MaxTestPerWeek.Text.All(char.IsDigit) || (TxtBx_MaxTestPerWeek.Text.Length == 0)) ||
                (!TxtBx_Seniority.Text.All(char.IsDigit) || (TxtBx_Seniority.Text.Length == 0))
                )
            {
                MessageBox.Show("You must fill all fields as needed.", "Can't " + operation, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            foreach (var item in HoursWork.Children.OfType<CheckBox>())
            {
                int row = Grid.GetRow(item);
                int column = Grid.GetColumn(item);
                tester.AvailiableWorkTime[row, column] = (bool)item.IsChecked;
            }
            switch (operation)
            {
                case "Add":
                    Tester testerToAdd = new Tester(TxtBx_ID.Text, tester);
                    try
                    {
                        MainWindow.bl.AddTester(testerToAdd);
                    }
                    catch (DuplicateWaitObjectException ex)
                    {
                        MessageBox.Show(ex.Message, "Already exist", MessageBoxButton.OK);
                        Close();
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        var result = MessageBox.Show(ex.Message + "\nDo you want to try agein?", "Age is Wrong", MessageBoxButton.YesNo);
                        if (MessageBoxResult.No == result)
                        {
                            Close();
                        }
                        else
                            return;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Close();
                    }
                    MessageBox.Show("Successfuly added tester!", "Add Status", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                    break;
                case "Update":
                    try
                    {
                        MainWindow.bl.UpdateTesterDetails(tester);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Close();
                    }
                    MessageBox.Show("Successfuly Updated tester!", "Update Status", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                    break;
            }
        }

        private void TxtBx_ID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtBx_ID.Text.All(char.IsDigit) || (TxtBx_ID.Text.Length != 9))
                TxtBx_ID.Background = Brushes.Red;
            else
                TxtBx_ID.BorderBrush = Brushes.Green;
        }

        private void TxtBx_ID_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_ID.Background = Brushes.White;
            TxtBx_ID.BorderBrush = Brushes.Gray;
        }

        private void TxtBx_FirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_FirstName.Text.Length == 0 || !TxtBx_FirstName.Text.All(x => x == ' ' || char.IsLetter(x)))
                TxtBx_FirstName.Background = Brushes.Red;
            else
                TxtBx_FirstName.BorderBrush = Brushes.Green;
        }

        private void TxtBx_FirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_FirstName.Background = Brushes.White;
            TxtBx_FirstName.BorderBrush = Brushes.Gray;
        }

        private void TxtBx_LastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_LastName.Text.Length == 0 || !TxtBx_LastName.Text.All(x => x == ' ' || char.IsLetter(x)))
                TxtBx_LastName.Background = Brushes.Red;
            else
                TxtBx_LastName.BorderBrush = Brushes.Green;
        }

        private void TxtBx_LastName_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_LastName.Background = Brushes.White;
            TxtBx_LastName.BorderBrush = Brushes.Gray;
        }

        private void TxtBx_Phone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtBx_Phone.Text.All(char.IsDigit) || (TxtBx_Phone.Text.Length != 10))
                TxtBx_Phone.Background = Brushes.Red;
            else
                TxtBx_Phone.BorderBrush = Brushes.Green;
        }

        private void TxtBx_Phone_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_Phone.Background = Brushes.White;
            TxtBx_Phone.BorderBrush = Brushes.Gray;
        }

        private void TxtBx_City_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_City.Text.Length == 0 || !TxtBx_City.Text.All(x => x == ' ' || char.IsLetter(x)))
                TxtBx_City.Background = Brushes.Red;
            else
                TxtBx_City.BorderBrush = Brushes.Green;
        }

        private void TxtBx_City_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_City.Background = Brushes.White;
            TxtBx_City.BorderBrush = Brushes.Gray;
        }

        private void TxtBx_Street_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_Street.Text.Length == 0 || !TxtBx_Street.Text.All(x => x == ' ' || char.IsLetter(x)))
                TxtBx_Street.Background = Brushes.Red;
            else
                TxtBx_Street.BorderBrush = Brushes.Green;
        }

        private void TxtBx_Street_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_Street.Background = Brushes.White;
            TxtBx_Street.BorderBrush = Brushes.Gray;
        }

        private void TxtBx_BuildNum_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtBx_BuildNum.Text.All(char.IsDigit) || (TxtBx_BuildNum.Text.Length == 0))
                TxtBx_BuildNum.Background = Brushes.Red;
            else
                TxtBx_BuildNum.BorderBrush = Brushes.Green;
        }

        private void TxtBx_BuildNum_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_BuildNum.Background = Brushes.White;
            TxtBx_BuildNum.BorderBrush = Brushes.Gray;
        }



        private void TxtBx_Seniority_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtBx_Seniority.Text.All(char.IsDigit) || (TxtBx_Seniority.Text.Length == 0))
                TxtBx_Seniority.Background = Brushes.Red;
            else
                TxtBx_Seniority.BorderBrush = Brushes.Green;
        }

        private void TxtBx_Seniority_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_Seniority.Background = Brushes.White;
            TxtBx_Seniority.BorderBrush = Brushes.Gray;
        }

        private void TxtBx_MaxTestPerWeek_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!TxtBx_MaxTestPerWeek.Text.All(char.IsDigit) || (TxtBx_MaxTestPerWeek.Text.Length == 0))
                TxtBx_MaxTestPerWeek.Background = Brushes.Red;
            else
                TxtBx_MaxTestPerWeek.BorderBrush = Brushes.Green;
        }

        private void TxtBx_MaxTestPerWeek_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_MaxTestPerWeek.Background = Brushes.White;
            TxtBx_MaxTestPerWeek.BorderBrush = Brushes.Gray;
        }

        private void TxtBx_MaxDistance_LostFocus(object sender, RoutedEventArgs e)
        {
            double maxD;
            if (!double.TryParse(TxtBx_MaxDistance.Text, out maxD) || (TxtBx_MaxDistance.Text.Length == 0))
                TxtBx_MaxDistance.Background = Brushes.Red;
            else
                TxtBx_MaxDistance.BorderBrush = Brushes.Green;
        }

        private void TxtBx_MaxDistance_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_MaxDistance.Background = Brushes.White;
            TxtBx_MaxDistance.BorderBrush = Brushes.Gray;
        }
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to cancel? Tester details that changed will be Lost.", "Cancel Request", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
                Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
