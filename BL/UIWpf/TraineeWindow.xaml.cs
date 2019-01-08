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
    /// Interaction logic for Trainee.xaml
    /// </summary>
    public partial class TraineeWindow : Window
    {
        public Trainee trainee;
        public string operation = "Add";
        public TraineeWindow()
        {
            trainee = new Trainee("");
            this.DataContext = trainee;
            InitializeComponent();
            CombBx_CurrCar.ItemsSource = Enum.GetValues(typeof(BO.CarTypeEnum));
            CombBx_CurrCar.SelectedItem = BO.CarTypeEnum.MotorCycle;
            CombBx_Gender.ItemsSource = Enum.GetValues(typeof(BO.GenderEnum));
            CombBx_Gender.SelectedItem = BO.GenderEnum.Male;
            LstBx_ExistLisn.ItemsSource = Enum.GetValues(typeof(BO.CarTypeEnum));
        }
        public TraineeWindow(Trainee t)
        {
            operation = "Update";
            trainee = t;
            this.DataContext = trainee;
            InitializeComponent();
            TxtBx_ID.IsEnabled = false;
            Button_Add.Content = "Update";
            CombBx_CurrCar.ItemsSource = Enum.GetValues(typeof(BO.CarTypeEnum));
            CombBx_Gender.ItemsSource = Enum.GetValues(typeof(BO.GenderEnum));
            LstBx_ExistLisn.ItemsSource = Enum.GetValues(typeof(BO.CarTypeEnum));
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
                (!int.TryParse(TxtBx_NumTests.Text, out num) || (TxtBx_NumTests.Text.Length == 0)) ||
                (TxtBx_Street.Text.Length == 0 || !TxtBx_Street.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                (!TxtBx_BuildNum.Text.All(char.IsDigit) || (TxtBx_BuildNum.Text.Length == 0))
                )
            {
                MessageBox.Show("You must fill all fields as needed.", "Can't " + operation, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            trainee.IsAlreadyDidTest = trainee.LastTest.ToShortDateString() != "01/01/0001" && trainee.LastTest < DateTime.Now;
            foreach (var item in LstBx_ExistLisn.SelectedItems)
                if (!trainee.ExistingLicenses.Exists(x => x == (CarTypeEnum)item))
                    trainee.ExistingLicenses.Add((CarTypeEnum)item);
            switch (operation)
            {
                case "Add":
                    Trainee traineeToAdd = new Trainee(TxtBx_ID.Text, trainee);
                    try
                    {
                        MainWindow.bl.AddTrainee(traineeToAdd);
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
                    MessageBox.Show("Successfuly added trainee!", "Add Status", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                    break;
                case "Update":
                    try
                    {
                        MainWindow.bl.UpdateTraineeDetails(trainee);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Close();
                    }
                    MessageBox.Show("Successfuly Updated trainee!", "Update Status", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void TxtBx_NumTests_LostFocus(object sender, RoutedEventArgs e)
        {
            int num;
            if (!int.TryParse(TxtBx_NumTests.Text, out num) || (TxtBx_NumTests.Text.Length == 0))
                TxtBx_NumTests.Background = Brushes.Red;
            else
                TxtBx_NumTests.BorderBrush = Brushes.Green;
        }

        private void TxtBx_NumTests_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBx_NumTests.Background = Brushes.White;
            TxtBx_NumTests.BorderBrush = Brushes.Gray;
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

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to cancel? Trainee deteils that changed will be Lost.", "Cancel Request", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
                Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //System.Windows.Data.CollectionViewSource traineeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("traineeViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // traineeViewSource.Source = [generic data source]
        }
    }
}
