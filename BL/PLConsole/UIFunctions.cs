using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BL;
namespace PLConsole
{
    class UIFunctions
    {
        private void TestWorkHours(int day, int hourBigen, int hourEnd, Tester s)
        {
            for (int i = hourBigen; i <= hourEnd; i++)
            {
                s.AvailableWorkTime[day, i] = true;
            }
        }
        private Person AddPerson()
        {
            Console.WriteLine("Enter tester ID\n");
            string id = Console.ReadLine();
            Person person;
            if (id.Length == 9 && id.All(char.IsDigit))
            {
                person = new Person(id);
                Console.WriteLine("Enrer first name\n");
                string name = Console.ReadLine();
                if (name.All(char.IsLetter)) { person.FirstName = name; }
                else { throw new FormatException("ERROR! Invalid first name"); }
                Console.WriteLine("Enter lest name\n");
                name = Console.ReadLine();
                if (name.All(char.IsLetter)) { person.LastName = name; }
                else { throw new FormatException("ERROR! Invalid lest name"); }
                Console.WriteLine("Enter phone number\n");
                string number = Console.ReadLine();
                if (number.Length == 10 && number.All(char.IsDigit)) { person.PhoneNumber = number; }
                else { throw new FormatException("ERROR! Invalid phone number"); }
                Console.WriteLine("Enter Gender:\nMale enter 1\n Fmale enter 2\n");
                int gender = Console.Read();
                if (gender == 1) { person.Gender = GenderEnum.Male; }
                else if (gender == 2) { person.Gender = GenderEnum.Female; }
                else { throw new FormatException("Are you not closed on your sexual identity?"); }
                Console.WriteLine("Enter address:\n");
                Console.WriteLine("enter city:\n");
                string City = Console.ReadLine();
                Console.WriteLine("enter street:\n");
                string Street = Console.ReadLine();
                Console.WriteLine("enter building Number:\n");
                string building = Console.ReadLine();
                int building2;
                if (City.All(char.IsLetter) && Street.All(char.IsLetter) && int.TryParse(building, out building2)) { person.Address = new Address(City, Street, building2); }
                else { throw new FormatException("ERROR! invalid address"); }
                Console.WriteLine("Enter Date of Birth:\n");
                DateTime dateOfBirth;
                string dateOfBirth2 = Console.ReadLine();
                if (DateTime.TryParse(dateOfBirth2, out dateOfBirth)) { person.DateOfBirth = dateOfBirth; }
                else { throw new FormatException("ERROR! Invalid birth date"); }
            }
            else { throw new FormatException("ERROR! Invalid ID"); }
            return person;
        }
        public Tester AddTester()
        {
            Person person = new Person(AddPerson());
            Tester tester = new Tester(person.Id);
            tester.FirstName = person.FirstName;
            tester.LastName = person.LastName;
            tester.Gender = person.Gender;
            tester.PhoneNumber = person.PhoneNumber;
            tester.Address = person.Address;
            tester.DateOfBirth = person.DateOfBirth;
            Console.WriteLine("Enter seniority:");
            string num = Console.ReadLine();
            double num2;
            if (double.TryParse(num, out num2)) { tester.Seniority = num2; }
            else { throw new FormatException("ERROR! The input for the trial number, is incorrect"); }
            Console.WriteLine("Insert the maximum distance the tester agrees to come\n");
            num = Console.ReadLine();
            if (double.TryParse(num, out num2)) { tester.MaxDistance = num2; }
            else { throw new FormatException("ERROR! The distance is not correct"); }
            Console.WriteLine("Enter the maximum number of tests the tester agrees to do per week\n");
            string MaxTestsPerWeek = Console.ReadLine();
            int MaxTestsPerWeek2;
            if (int.TryParse(MaxTestsPerWeek, out MaxTestsPerWeek2)) { tester.MaxTestsPerWeek = MaxTestsPerWeek2; }
            else { throw new FormatException("The maximum number of lessons per week was reached"); }
            Console.WriteLine("Enter the type of car in which the tester specializes\n");
            Console.WriteLine("for Motor Cycle enter: 1 \n for Private Car enter: 2 \n for Truck 12 Tons enter: 3 \n for Truck Un limited enter: 4 \n for bus enter: 5 \n");
            int car;
            bool OK = int.TryParse(Console.ReadLine(), out car);
            if (OK)
            {
                switch (car)
                {
                    case 1:
                        tester.TypeCarToTest = CarTypeEnum.MotorCycle;
                        break;
                    case 2:
                        tester.TypeCarToTest = CarTypeEnum.PrivateCar;
                        break;
                    case 3:
                        tester.TypeCarToTest = CarTypeEnum.Truck12Tons;
                        break;
                    case 4:
                        tester.TypeCarToTest = CarTypeEnum.TruckUnlimited;
                        break;
                    case 5:
                        tester.TypeCarToTest = CarTypeEnum.Bus;
                        break;
                    default: throw new FormatException("ERROR! Invalid vehicle selection");
                }
            }
            else
            {
                throw new FormatException("ERROR! Invalid vehicle selection");
            }
            Console.WriteLine("How many days a week does Taster agree to work?");
            int numOfDays, day, hourBigen, hourEnd;
            bool ok, ok1, ok2;
            ok = int.TryParse(Console.ReadLine(), out numOfDays);
            if (ok)
            {
                for (int i = 0; i < numOfDays; i++)
                {
                    Console.WriteLine("Enter the days that the Tester agrees to work \n");
                    Console.WriteLine("For Sunday Press 1 \n for Monday Press 2 \n for Tuesday Press 3 \n" +
                        "for Wednesday Press 4 \n for Thursday Press 5 \n");
                    ok = int.TryParse(Console.ReadLine(), out day);
                    Console.WriteLine("Enter the time that the tester wants to start work \n");
                    ok1 = int.TryParse(Console.ReadLine(), out hourBigen);
                    Console.WriteLine("Enter the time that the tester wants to finish working");
                    ok2 = int.TryParse(Console.ReadLine(), out hourEnd);
                    if (ok && ok1 && ok2 && hourBigen > 8 && hourEnd < 16 && hourEnd > hourBigen)
                    {
                        TestWorkHours(day, hourBigen, hourEnd, tester);
                    }
                }
            }
            return tester;
        }
        public Trainee AddTrainee()
        {
            Person person = new Person(AddPerson());
            Trainee trainee = new Trainee(person.Id);
            trainee.FirstName = person.FirstName;
            trainee.LastName = person.LastName;
            trainee.Gender = person.Gender;
            trainee.PhoneNumber = person.PhoneNumber;
            trainee.Address = person.Address;
            trainee.DateOfBirth = person.DateOfBirth;
            Console.WriteLine("is Already Did Test?\n Enter yes or no \n");
            string yesOrNov = Console.ReadLine();
            if (yesOrNov == "yes")
            {
                Console.WriteLine("Enter the date of the last test \n");
                string lastTest = Console.ReadLine();
                DateTime lastTest2;
                if (DateTime.TryParse(lastTest, out lastTest2)) { trainee.LastTest = lastTest2; }
                else { throw new FormatException("The last test date is invalid \n"); }
            }
            Console.WriteLine("Enter the type of car license the student is studying\n");
            Console.WriteLine("for Motor Cycle enter: 1 \n for Private Car enter: 2 \n for Truck 12 Tons enter: 3 \n for Truck Un limited enter: 4 \n for bus enter: 5 \n");
            int car1;
            bool car = int.TryParse(Console.ReadLine(), out car1);
            if (car)
            {
                switch (car1)
                {
                    case 1:
                        trainee.CurrCarType = CarTypeEnum.MotorCycle;
                        break;
                    case 2:
                        trainee.CurrCarType = CarTypeEnum.PrivateCar;
                        break;
                    case 3:
                        trainee.CurrCarType = CarTypeEnum.Truck12Tons;
                        break;
                    case 4:
                        trainee.CurrCarType = CarTypeEnum.TruckUnlimited;
                        break;
                    case 5:
                        trainee.CurrCarType = CarTypeEnum.Bus;
                        break;
                    default: throw new FormatException("ERROR! Invalid vehicle selection");
                }
                Console.WriteLine("Insert 1 if automatic or 2 if manual: \n");
                car = int.TryParse(Console.ReadLine(), out car1);
                switch (car1)
                {
                    case 1:
                        trainee.CurrGearType = GearboxTypeEnum.auto;
                        break;
                    case 2:
                        trainee.CurrGearType = GearboxTypeEnum.manual;
                        break;
                    default: throw new FormatException("Invalid Gear box selection");
                }
                Console.WriteLine("Enter the number of lessons the trainee has finished \n");
                car = int.TryParse(Console.ReadLine(), out car1);
                if (car) { trainee.NumOfFinishedLessons = car1; }
                else { throw new FormatException("The number of classes is invalid"); }
                Console.WriteLine("Enter the number of tests a trainee has done \n");
                car = int.TryParse(Console.ReadLine(), out car1);
                if (car) { trainee.NumOfTests = car1; }
                else { throw new FormatException("The number of tests is invalid"); }
                Console.WriteLine("Enter the number of licenses the trainee has");
                car = int.TryParse(Console.ReadLine(), out car1);
                if (car)
                {
                    int car2;
                    for (int i = 0; i < car1; i++)
                    {
                        Console.WriteLine("for Motor Cycle enter: 1 \n for Private Car enter: 2 \n for Truck 12 Tons enter: 3 \n for Truck Un limited enter: 4 \n for bus enter: 5 \n");
                        car = int.TryParse(Console.ReadLine(), out car2);
                        if (car)
                        {
                            switch (car2)
                            {
                                case 1:
                                    trainee.ExistingLicenses.Add(CarTypeEnum.MotorCycle);
                                    break;
                                case 2:
                                    trainee.ExistingLicenses.Add(CarTypeEnum.PrivateCar);
                                    break;
                                case 3:
                                    trainee.ExistingLicenses.Add(CarTypeEnum.Truck12Tons);
                                    break;
                                case 4:
                                    trainee.ExistingLicenses.Add(CarTypeEnum.TruckUnlimited);
                                    break;
                                case 5:
                                    trainee.ExistingLicenses.Add(CarTypeEnum.Bus);
                                    break;
                                default: throw new FormatException("ERROR! Invalid vehicle selection");
                            }
                        }
                        else { throw new FormatException("Invalid number of licenses"); }

                    }
                }
            }
            return trainee;
        }
        public Tester UpdateTesterDetails(IBL bL)
        {
            Console.WriteLine("Enter the Tester ID you want to update");
            string id = Console.ReadLine();
            if (id.All(char.IsDigit) && id.Length == 9)
            {
                int index = bL.GetTestersList().FindIndex(x => x.Id == id);
                if (index > -1)
                {
                    Tester tester = new Tester(bL.GetTestersList()[index]);
                    int choice;
                    do
                    {
                        Console.WriteLine("To update an phone number enter: 1 \n To update an address enter: 2 \n To update tester seniority enter: 3 \n" +
                        "Updating the maximum distance that the Tester agrees to come enter: 4 \n To update the maximum number of tests that the Tester agrees to do per week enter: 5 \n" +
                        "To update the types of vehicles that specialize in tasters enter: 6 \n To update working hours of the tester enter: 7 \n Exit enter:8 \n");
                        bool ok = int.TryParse(Console.ReadLine(), out choice);
                        if (ok)
                        {
                            switch (choice)
                            {
                                case 1:
                                    Console.WriteLine("Enter the new phone number: \n");
                                    string phone = Console.ReadLine();
                                    if (phone.Length == 10 && phone.Any(char.IsDigit)) { tester.PhoneNumber = phone; }
                                    else { throw new FormatException("ERROR! the number is illegal"); }
                                    break;
                                case 2:
                                    Console.WriteLine("Enter address:\n");
                                    Console.WriteLine("enter city:\n");
                                    string City = Console.ReadLine();
                                    Console.WriteLine("enter street:\n");
                                    string Street = Console.ReadLine();
                                    Console.WriteLine("enter building Number:\n");
                                    string building = Console.ReadLine();
                                    int building2;
                                    if (City.All(char.IsLetter) && Street.All(char.IsLetter) && int.TryParse(building, out building2)) { tester.Address = new Address(City, Street, building2); }
                                    else { throw new FormatException("ERROR! invalid address"); }
                                    break;
                                case 3:
                                    Console.WriteLine("Enter the number of years of seniority updated \n");
                                    double seniority;
                                    ok = double.TryParse(Console.ReadLine(), out seniority);
                                    if (ok) { tester.Seniority = seniority; }
                                    else { throw new FormatException("The number of years of seniority is not correct!"); }
                                    break;
                                case 4:
                                    Console.WriteLine("Insert the maximum distance that the Tester agrees to come \n");
                                    double distance;
                                    ok = double.TryParse(Console.ReadLine(), out distance);
                                    if (ok) { tester.MaxDistance = distance; }
                                    else { throw new FormatException("ERROR! Invalid distance"); }
                                    break;
                                case 5:
                                    Console.WriteLine("Enter the maximum number of tests that Tester agrees to do per week \n");
                                    int numOfTests;
                                    ok = int.TryParse(Console.ReadLine(), out numOfTests);
                                    if (ok) { tester.MaxTestsPerWeek = numOfTests; }
                                    else { throw new FormatException("The maximum number of tests is invalid"); }
                                    break;
                                case 6:
                                    Console.WriteLine("Enter the new type of car in which the tester specializes\n");
                                    Console.WriteLine("for Motor Cycle enter: 1 \n for Private Car enter: 2 \n for Truck 12 Tons enter: 3 \n for Truck Un limited enter: 4 \n for bus enter: 5 \n");
                                    int car;
                                    ok = int.TryParse(Console.ReadLine(), out car);
                                    if (ok)
                                    {
                                        switch (car)
                                        {
                                            case 1:
                                                tester.TypeCarToTest = CarTypeEnum.MotorCycle;
                                                break;
                                            case 2:
                                                tester.TypeCarToTest = CarTypeEnum.PrivateCar;
                                                break;
                                            case 3:
                                                tester.TypeCarToTest = CarTypeEnum.Truck12Tons;
                                                break;
                                            case 4:
                                                tester.TypeCarToTest = CarTypeEnum.TruckUnlimited;
                                                break;
                                            case 5:
                                                tester.TypeCarToTest = CarTypeEnum.Bus;
                                                break;
                                            default: throw new FormatException("ERROR! Invalid vehicle selection");
                                        }
                                    }
                                    else
                                    {
                                        throw new FormatException("ERROR! Invalid vehicle selection");
                                    }
                                    break;
                                case 7:
                                    Console.WriteLine("How many days a week does Taster agree to work?");
                                    int numOfDays, day, hourBigen, hourEnd;
                                    bool ok1, ok2;
                                    ok = int.TryParse(Console.ReadLine(), out numOfDays);
                                    if (ok)
                                    {
                                        for (int i = 0; i < numOfDays; i++)
                                        {
                                            Console.WriteLine("Enter the days that the Tester agrees to work \n");
                                            Console.WriteLine("For Sunday Press 1 \n for Monday Press 2 \n for Tuesday Press 3 \n" +
                                                "for Wednesday Press 4 \n for Thursday Press 5 \n");
                                            ok = int.TryParse(Console.ReadLine(), out day);
                                            Console.WriteLine("Enter the time that the tester wants to start work \n");
                                            ok1 = int.TryParse(Console.ReadLine(), out hourBigen);
                                            Console.WriteLine("Enter the time that the tester wants to finish working");
                                            ok2 = int.TryParse(Console.ReadLine(), out hourEnd);
                                            if (ok && ok1 && ok2 && hourBigen > 8 && hourEnd < 16 && hourEnd > hourBigen)
                                            {
                                                TestWorkHours(day, hourBigen, hourEnd, tester);
                                            }
                                        }
                                    }
                                    break;
                                default: throw new FormatException("The tester information update selection is invalid");
                            }
                        }
                    } while (choice != 8);
                    return tester;
                }
                else { throw new KeyNotFoundException("There is no tester with this ID in the system"); }
            }
            throw new FormatException("Invalid ID");
        }
        public Trainee UpdateTraineeDetails(IBL bL)
        {
            Console.WriteLine("Enter the Trainee ID you want to update");
            string id = Console.ReadLine();
            if (id.All(char.IsDigit) && id.Length == 9)
            {
                int index = bL.GetTraineeList().FindIndex(x => x.Id == id);
                if (index > -1)
                {
                    Trainee trainee = new Trainee(bL.GetTraineeList()[index]);

                }
                return
        }
        }
    }


