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
    /// Interaction logic for TesterWindow.xaml
    /// </summary>
    public partial class TesterDetailsWindow : Window
    {
        public Tester tester;
        public string operation = "Add";
        public TesterDetailsWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tester = new BO.Tester();
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
            CmbBx_City.ItemsSource = MainWindow.cities;
            CmbBx_Street.IsEnabled = false;
        }
        public TesterDetailsWindow(Tester t, string oper)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            operation = oper;
            tester = t;
            this.DataContext = tester;
            InitializeComponent();
            TxtBx_ID.IsEnabled = false;
            foreach (var item in HoursWork.Children.OfType<CheckBox>())//initial checkBoxs
            {
                int row = Grid.GetRow(item);
                int column = Grid.GetColumn(item);
                item.IsChecked = tester.AvailiableWorkTime[row, column];
            }
            CmbBx_City.ItemsSource = MainWindow.cities;
            CmbBx_Street.Text = t.City;
            CmbBx_Street.Text = t.Street;
            switch (oper)
            {
                case "Update":
                    Button_Add.Content = "Update";
                    Label_Header.Content = "Tester's Details. Change the needed values.";
                    CombBx_TypeCarToTest.ItemsSource = Enum.GetValues(typeof(BO.CarTypeEnum));
                    CombBx_Gender.ItemsSource = Enum.GetValues(typeof(BO.GenderEnum));
                    break;
                case "View":
                    TxtBx_ID.IsEnabled = false;
                    TxtBx_FirstName.IsEnabled = false;
                    TxtBx_LastName.IsEnabled = false;
                    TxtBx_Phone.IsEnabled = false;
                    TxtBx_Seniority.IsEnabled = false;
                    TxtBx_MaxTestPerWeek.IsEnabled = false;
                    TxtBx_MaxDistance.IsEnabled = false;
                    CmbBx_City.IsEnabled = false;
                    CmbBx_Street.IsEnabled = false;
                    TxtBx_BuildNum.IsEnabled = false;
                    DatePicker_BirthDay.IsEnabled = false;
                    CombBx_TypeCarToTest.IsEnabled = false;
                    CombBx_Gender.IsEnabled = false;

                    foreach (var item in HoursWork.Children.OfType<CheckBox>())//initial checkBoxs
                    {
                        item.IsEnabled = false;
                    }

                    Label_Header.Content = "Tester's Details:";
                    Label_ID.Content = "Tester ID:";
                    Label_FirstName.Content = "Tester First Name:";
                    Label_LastName.Content = "Tester Last Name:";
                    Label_Seniority.Content = "Tester Seniority:";
                    Label_MaxTestPerWeek.Content = "Tester Max Tests Per Week:";
                    Label_MaxDistance.Content = "Tester Max Distance To Test:";
                    Label_Phone.Content = "Tester Phone Number:";
                    Label_Address.Content = "Tester Address:";
                    Label_BirthDay.Content = "Tester Birth Date:";
                    Label_TypeCarToTest.Content = "Tester Type Car To Test:";
                    Label_Gender.Content = "Tester Gender:";
                    //label_WorkTimes.Content = "Tester Weekly Working Hours And Days:";
                    Button_Add.Visibility = Visibility.Collapsed;
                    Button_Cancel.Visibility = Visibility.Collapsed;
                    Button_OK.Visibility = Visibility.Visible;
                    break;
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
                (!TxtBx_BuildNum.Text.All(char.IsDigit) || (TxtBx_BuildNum.Text.Length == 0)) ||
                (!double.TryParse(TxtBx_MaxDistance.Text, out maxD) || (TxtBx_MaxDistance.Text.Length == 0)) ||
                (!TxtBx_MaxTestPerWeek.Text.All(char.IsDigit) || (TxtBx_MaxTestPerWeek.Text.Length == 0)) ||
                (!TxtBx_Seniority.Text.All(char.IsDigit) || (TxtBx_Seniority.Text.Length == 0))
                )
            {
                MessageBox.Show("You must fill all fields as needed.", "Can't " + operation, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!MainWindow.cities.Exists(x => x == (string)CmbBx_City.SelectedItem) || !MainWindow.streetsGroupedByCity.Find(x => x.Key == (string)CmbBx_City.SelectedItem).ToList().Exists(x => x == (string)CmbBx_Street.SelectedItem))
            {
                MessageBox.Show("Address input was wrong.", "Can't " + operation, MessageBoxButton.OK, MessageBoxImage.Error);
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
                        MessageBox.Show(ex.Message, "Already exist", MessageBoxButton.OK, MessageBoxImage.Error);
                        Close();
                        return;
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        var result = MessageBox.Show(ex.Message + "\nDo you want to try agein?", "Age is Wrong", MessageBoxButton.YesNo, MessageBoxImage.Error);
                        if (MessageBoxResult.No == result)
                        {
                            Close();
                            return;
                        }
                        else
                            return;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Close();
                        return;
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
                        return;
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

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void CmbBx_City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CmbBx_Street.IsEnabled = true;
            CmbBx_Street.ItemsSource = MainWindow.streetsGroupedByCity.Find(x => x.Key == (string)CmbBx_City.SelectedItem);
        }
    }
}
