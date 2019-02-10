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
    /// Interaction logic for Trainee.xaml
    /// </summary>
    public partial class TraineeDetailsWindow : Window
    {
        public Trainee trainee;
        OpenFileDialog op;
        public string operation = "Add";
        public TraineeDetailsWindow()
        {
            this.FlowDirection = FlowDirection.RightToLeft;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            trainee = new Trainee("");
            this.DataContext = trainee;
            InitializeComponent();
            DatePicker_BirthDay.DisplayDate = DateTime.Now.AddYears(-20);
            CombBx_CurrCar.ItemsSource = Enum.GetValues(typeof(BO.CarTypeEnum));
            CombBx_CurrCar.SelectedItem = BO.CarTypeEnum.אופנוע;
            CombBx_Gender.ItemsSource = Enum.GetValues(typeof(BO.GenderEnum));
            CombBx_Gender.SelectedItem = BO.GenderEnum.זכר;
            CmbBx_City.ItemsSource = MainWindow.cities;
            CmbBx_Street.IsEnabled = false;

        }
        public TraineeDetailsWindow(Trainee t, string oper)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.FlowDirection = FlowDirection.RightToLeft;
            operation = oper;
            trainee = t;
            this.DataContext = trainee;
            InitializeComponent();
            CombBx_CurrCar.ItemsSource = Enum.GetValues(typeof(BO.CarTypeEnum));
            CombBx_Gender.ItemsSource = Enum.GetValues(typeof(BO.GenderEnum));
            CmbBx_City.ItemsSource = MainWindow.cities;
            CmbBx_City.Text = t.City;
            CmbBx_City.SelectedItem = t.City;
            CmbBx_Street.Text = t.Street;
            CmbBx_Street.SelectedItem = t.Street;
            try
            {
                TraineeImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"..\..\TraineesImages\" + @trainee.Id + @".jpg")));
            }
            catch { }
            switch (oper)
            {
                case "Update":
                    TxtBx_ID.IsEnabled = false;
                    Button_Add.Content = "שמור שינויים";
                    Label_Header.Content = "שנה את הפרטים הדורשים שינוי";

                    break;
                case "View":
                    TxtBx_ID.IsEnabled = false;
                    TxtBx_FirstName.IsEnabled = false;
                    TxtBx_LastName.IsEnabled = false;
                    TxtBx_Teacher.IsEnabled = false;
                    TxtBx_School.IsEnabled = false;
                    TxtBx_Phone.IsEnabled = false;
                    TxtBx_NumLessons.IsEnabled = false;
                    //TxtBx_City.IsEnabled = false;
                    CmbBx_City.IsEnabled = false;
                    //TxtBx_Street.IsEnabled = false;
                    CmbBx_Street.IsEnabled = false;
                    TxtBx_BuildNum.IsEnabled = false;
                    DatePicker_BirthDay.IsEnabled = false;
                    CombBx_CurrCar.IsEnabled = false;
                    CombBx_Gender.IsEnabled = false;
                    Butto_UploadImage.Visibility = Visibility.Collapsed;

                    Button_Add.Visibility = Visibility.Collapsed;
                    Button_Cancel.Visibility = Visibility.Collapsed;
                    Button_OK.Visibility = Visibility.Visible;
                    break;
            }
        }
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            int num;
            if (
                (!TxtBx_ID.Text.All(char.IsDigit) || (TxtBx_ID.Text.Length != 9)) ||
                (TxtBx_FirstName.Text.Length == 0 || !TxtBx_FirstName.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                (TxtBx_LastName.Text.Length == 0 || !TxtBx_LastName.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                (TxtBx_School.Text.Length == 0 || !TxtBx_School.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                (TxtBx_Teacher.Text.Length == 0 || !TxtBx_Teacher.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                (!TxtBx_Phone.Text.All(char.IsDigit) || (TxtBx_Phone.Text.Length != 10)) ||
                (!int.TryParse(TxtBx_NumLessons.Text, out num) || (TxtBx_NumLessons.Text.Length == 0)) ||
                (!TxtBx_BuildNum.Text.All(char.IsDigit) || (TxtBx_BuildNum.Text.Length == 0))
                )
            {
                MessageBox.Show("עליך למלאות את כל הפרטים כנדרש.", "פעולה נכשלה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (!MainWindow.cities.Exists(x => x == (string)CmbBx_City.SelectedItem) || !MainWindow.streetsGroupedByCity.Find(x => x.Key == (string)CmbBx_City.SelectedItem).ToList().Exists(x => x == (string)CmbBx_Street.SelectedItem))
            {
                MessageBox.Show("קלט הכתובת שגוי.", "פעולה נכשלה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            switch (operation)
            {
                case "Add":
                    Trainee traineeToAdd = new Trainee(TxtBx_ID.Text, trainee);
                    try
                    {
                        MainWindow.bl.AddTrainee(traineeToAdd);
                    }
                    catch (DuplicateWaitObjectException)
                    {
                        MessageBox.Show("תלמיד עם תעודת זהות זו כבר נמצא במערכת.", "בעיית כפילויות", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        this.Close();
                        return;
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        var result = MessageBox.Show(ex.Message + "\nהאם תרצה לנסות שוב?", "גיל מחוץ לטווח", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
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
                        Close();
                        return;
                    }
                    MessageBox.Show("תלמיד התווסף בהצלחה!", "סטטוס הוספה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    System.IO.File.Copy(op.FileName, @"..\..\TraineesImages\" + traineeToAdd.Id + @".jpg", true);
                    Close();
                    break;
                case "Update":
                    try
                    {
                        MainWindow.bl.UpdateTraineeDetails(trainee);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        Close();
                        return;
                    }
                    MessageBox.Show("פרטי תלמיד עודכנו בהצלחה!", "סטטוס עידכון", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    System.IO.File.Copy(op.FileName, @"..\..\TraineesImages\" + @trainee.Id + @".jpg", true);
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
        private void TxtBx_School_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_School.Text.Length == 0 || !TxtBx_School.Text.All(x => x == ' ' || char.IsLetter(x)))
                TxtBx_School.Background = Brushes.Red;
            else
                TxtBx_School.BorderBrush = Brushes.Green;
        }
        private void TxtBx_School_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_School.Background = Brushes.White;
            TxtBx_School.BorderBrush = Brushes.Gray;
        }
        private void TxtBx_Teacher_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_Teacher.Text.Length == 0 || !TxtBx_Teacher.Text.All(x => x == ' ' || char.IsLetter(x)))
                TxtBx_Teacher.Background = Brushes.Red;
            else
                TxtBx_Teacher.BorderBrush = Brushes.Green;
        }
        private void TxtBx_Teacher_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_Teacher.Background = Brushes.White;
            TxtBx_Teacher.BorderBrush = Brushes.Gray;
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
        private void TxtBx_NumLessons_LostFocus(object sender, RoutedEventArgs e)
        {
            int num;
            if (!int.TryParse(TxtBx_NumLessons.Text, out num) || (TxtBx_NumLessons.Text.Length == 0))
                TxtBx_NumLessons.Background = Brushes.Red;
            else
                TxtBx_NumLessons.BorderBrush = Brushes.Green;
        }
        private void TxtBx_NumLessons_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_NumLessons.Background = Brushes.White;
            TxtBx_NumLessons.BorderBrush = Brushes.Gray;
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

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("האם אתה בטוח שברצונך לבטל את הפעולה?\nלא יתרחש שום שינוי.", "בקשת ביטול", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.OK)
                Close();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //System.Windows.Data.CollectionViewSource traineeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("traineeViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // traineeViewSource.Source = [generic data source]
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
                TraineeImage.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
    }
}
