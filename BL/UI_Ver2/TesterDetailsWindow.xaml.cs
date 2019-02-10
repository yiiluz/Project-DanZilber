using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace UI_Ver2
{
    /// <summary>
    /// Interaction logic for TesterWindow.xaml
    /// </summary>
    public partial class TesterDetailsWindow : Window
    {
        public Tester tester;

        public string operation = "Add";
        OpenFileDialog op;
        bool isImageChanged = false;
        public TesterDetailsWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.FlowDirection = FlowDirection.RightToLeft;
            tester = new BO.Tester();
            this.DataContext = tester;
            InitializeComponent();
            DatePicker_BirthDay.DisplayDate = DateTime.Now.AddYears(-45);
            CombBx_TypeCarToTest.ItemsSource = Enum.GetValues(typeof(BO.CarTypeEnum));
            CombBx_Gender.ItemsSource = Enum.GetValues(typeof(BO.GenderEnum));
            CombBx_TypeCarToTest.SelectedItem = BO.CarTypeEnum.אופנוע;
            CombBx_Gender.SelectedItem = BO.GenderEnum.זכר;
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
            this.FlowDirection = FlowDirection.RightToLeft;
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
            CmbBx_City.Text = t.City;
            CmbBx_City.SelectedItem = t.City;
            CmbBx_Street.Text = t.Street;
            CmbBx_Street.SelectedItem = t.Street;
            try
            {
                TesterImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"..\..\..\TestersImages\" + tester.Id + @".jpg"), UriKind.Absolute));
            }
            catch { }
            switch (oper)
            {
                case "Update":
                    Button_Add.Content = "שמור שינויים";
                    Label_Header.Content = "שנה את הפרטים שברצונך לשנות";
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
                    Button_UploadImage.Visibility = Visibility.Collapsed;

                    foreach (var item in HoursWork.Children.OfType<CheckBox>())//initial checkBoxs
                    {
                        item.IsEnabled = false;
                    }
                    //label_WorkTimes.Content = "Tester Weekly Working Hours And Days:";
                    Button_Add.Visibility = Visibility.Collapsed;
                    Button_Cancel.Visibility = Visibility.Collapsed;
                    Button_OK.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (CmbBx_City.SelectedItem == null || CmbBx_Street.SelectedItem == null)
                return;
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
                MessageBox.Show("עליך למלא את כל השדות כנדרש.", "פעולה נכשלה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (!MainWindow.cities.Exists(x => x == (string)CmbBx_City.SelectedItem) || !MainWindow.streetsGroupedByCity.Find(x => x.Key == (string)CmbBx_City.SelectedItem).ToList().Exists(x => x == (string)CmbBx_Street.SelectedItem))
            {
                MessageBox.Show("קלט הכתובת שגוי.", "פעולה נכשלה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
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
                        MessageBox.Show(ex.Message, "שגיאת כפילויות", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        Close();
                        return;
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        var result = MessageBox.Show(ex.Message + "\nהאם ברצונך לנסות שוב?", "גיל מחוץ לטווח", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
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

                        MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        return;
                    }
                    Close();
                    try
                    {
                        System.IO.File.Copy(op.FileName, @"..\..\..\TestersImages\" + @testerToAdd.Id + @".jpg", true);
                    }
                    catch { }
                    MessageBox.Show("הבוחן התווסף בהצלחה למערכת!", "סטטוס הוספה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

                    break;
                case "Update":
                    try
                    {
                        MainWindow.bl.UpdateTesterDetails(tester);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        return;
                    }

                    MessageBox.Show("פרטי בוחן עודכנו בהצלחה!", "סטטוס עידכון", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    Close();
                    if (isImageChanged)
                    {
                        string destination = @"..\..\..\TestersImages\" + tester.Id + @".jpg";
                        for (int i = 0; i < 100; ++i) //try 100 times to overrite image.
                        {
                            try
                            {
                                System.IO.File.Copy(op.FileName, System.IO.Path.GetFullPath(destination), true);
                                break;
                            }
                            catch { }
                        }
                    }
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
            var result = MessageBox.Show("האם את בטוח שברצונך לבטל?\nהשינויים לא יישמרו.", "בקשת ביטול", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.OK)
                Close();

        }
        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void CmbBx_City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbBx_City.SelectedItem != null)
            {
                CmbBx_Street.IsEnabled = true;
                CmbBx_Street.ItemsSource = MainWindow.streetsGroupedByCity.Find(x => x.Key == (string)CmbBx_City.SelectedItem);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void Button_Click_UploadImage(object sender, RoutedEventArgs e)
        {
            op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                TesterImage.Source = new BitmapImage(new Uri(op.FileName));
                isImageChanged = true;
            }
        }
    }
}
