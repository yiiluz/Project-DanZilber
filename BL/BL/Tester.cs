using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Tester : Person
    {
        private double seniority;
        private double maxDistance;
        private int maxTestsPerWeek;
        private CarTypeEnum typeCarToTest;
        private bool[,] availableWorkTime = new bool[5, 6];
        private List<TesterTest> testList = new List<TesterTest>();

        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="id"></param>
        public Tester(string id) : base(id)
        {
            for (int i = 0; i < 5; ++i)
                for (int j = 0; j < 6; ++j)
                    AvailableWorkTime[i, j] = false;
        }
        //public Tester(DO.Tester other) : base(other.Id)
        //{
        //    LastName = other.LastName;
        //    FirstName = other.FirstName;
        //    PhoneNumber = other.PhoneNumber;
        //    Gender = (GenderEnum)other.Gender;
        //    Address = new Address(other.Address.City, other.Address.Street, other.Address.BuildingNumber);
        //    DateOfBirth = other.DateOfBirth;
        //    Seniority = other.Seniority;
        //    MaxDistance = other.MaxDistance;
        //    MaxTestsPerWeek = other.MaxTestsPerWeek;
        //    TypeCarToTest = (CarTypeEnum)other.TypeCarToTest;
        //    AvailableWorkTime = other.AvailableWorkTime;
        //    //foreach (var item in other.TestList)
        //    //    TestList.Add(new TesterTest(item));
        //}
        public Tester(Tester other) : base(other.Id)
        {
            LastName = other.LastName;
            FirstName = other.FirstName;
            PhoneNumber = other.PhoneNumber;
            Gender = other.Gender;
            Address = new Address(other.Address);
            DateOfBirth = other.DateOfBirth;
            Seniority = other.Seniority;
            MaxDistance = other.MaxDistance;
            MaxTestsPerWeek = other.MaxTestsPerWeek;
            TypeCarToTest = other.TypeCarToTest;
            AvailableWorkTime = other.AvailableWorkTime;
            foreach (var item in other.TestList)
                TestList.Add(item);
        }
        public Tester(string id, Tester other) : base(id)
        {
            LastName = other.LastName;
            FirstName = other.FirstName;
            PhoneNumber = other.PhoneNumber;
            Gender = other.Gender;
            Address = new Address(other.Address);
            DateOfBirth = other.DateOfBirth;
            Seniority = other.Seniority;
            MaxDistance = other.MaxDistance;
            MaxTestsPerWeek = other.MaxTestsPerWeek;
            TypeCarToTest = other.TypeCarToTest;
            AvailableWorkTime = other.AvailableWorkTime;
            foreach (var item in other.TestList)
                TestList.Add(item);
        }
        /// <summary>
        /// Get DateTime & hour, and returns true if tester availiable at this time.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public bool IsAvailiableOnDate(DateTime date, int hour)
        {
            bool flag = true;
            foreach (var item in TestList)
            {
                if (item.DateOfTest == date && item.HourOfTest == hour || GetNumOfTestForSpecificWeek(date) + 1 > MaxTestsPerWeek || AvailableWorkTime[(int)date.DayOfWeek, hour - 9])
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }
        /// <summary>
        /// Get num of tests for week
        /// </summary>
        /// <param name="DateTime of wanted week"
        /// <returns>number of tests</returns>
        public int GetNumOfTestForSpecificWeek(DateTime a)
        {
            int num = 0;
            foreach (var item in TestList)
                if (item.DateOfTest.AddDays(-(int)item.DateOfTest.DayOfWeek) == a.AddDays(-(int)a.DayOfWeek))
                    num++;
            return num;
        }
        public int GetClosetHour(DateTime date, int hour)
        {
            if (IsAvailiableOnDate(date, hour))
                return hour;
            //set matrix of bool that contain the available times of the date week
            bool[] temp = new bool[6];
            for (int i = 0; i < 6; ++i)
                temp[i] = AvailableWorkTime[(int)date.DayOfWeek, i];
            foreach (var x in TestList)
            {
                for (int j = 0; j < 6; ++j)
                {
                    if (!IsAvailiableOnDate(date, j + 9))
                            temp[j] = false;
                }
            }
            hour -= 9;
            for (int i = hour; i < 6; ++i)
                if (temp[i])
                    return i + 9;
            for (int i = hour -1; i <= 0; --i)
                if (temp[i])
                    return i + 9;
            return -1;//if day is full
        }
        public double Seniority { get => seniority; set => seniority = value; }
        public double MaxDistance { get => maxDistance; set => maxDistance = value; }
        public int MaxTestsPerWeek { get => maxTestsPerWeek; set => maxTestsPerWeek = value; }
        public CarTypeEnum TypeCarToTest { get => typeCarToTest; set => typeCarToTest = value; }
        public bool[,] AvailableWorkTime { get => availableWorkTime; set => availableWorkTime = value; }
        public List<TesterTest> TestList { get => testList; set => testList = value; }


        /// <summary>
        /// overide ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string tmp = "Tester name: " + FirstName + " " + LastName + ".\nID: " + Id + ".\nGender: " + Gender + ".\nDate Of Birth: " + DateOfBirth.ToShortDateString() +
                ".\nPhone number: " + PhoneNumber + ".\nAddress: " + Address + "Seniority: " + Seniority + ".\nType of car: " + TypeCarToTest +
                ".\nMax tests per week: " + MaxTestsPerWeek + ".\nMax distance for test: " + MaxDistance + ".\n";
            return tmp;
        }
    }
}
