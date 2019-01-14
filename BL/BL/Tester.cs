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

        //Ctors
        //--------------------------------------------------------------------------------------------------------
        public Tester() : base("") { }
        public Tester(string id) : base(id)
        {
            for (int i = 0; i < 5; ++i)
                for (int j = 0; j < 6; ++j)
                    AvailiableWorkTime[i, j] = false;
        }
        private void Days(int day, string t)
        {
            switch (day)
            {
                case 0:
                    t += "SunDay: ";
                    break;
                case 1:
                    t += "Monday: ";
                    break;
                case 2:
                    t += "Tuesday: ";
                    break;
                case 3:
                    t += "Wednesday: ";
                    break;
                case 4:
                    t += "Thursday: ";
                    break;
                default:
                    break;
            }
        }
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
            AvailiableWorkTime = other.AvailiableWorkTime;
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

            AvailiableWorkTime = other.AvailiableWorkTime;
            foreach (var item in other.TestList)
                TestList.Add(item);
        }
        //--------------------------------------------------------------------------------------------------------

        //help functions
        //--------------------------------------------------------------------------------------------------------
        public int GetNumOfTestForSpecificWeek(DateTime a)
        {
            int num = 0;
            foreach (var item in TestList)
                if (item.DateOfTest.AddDays(-(int)item.DateOfTest.DayOfWeek) == a.AddDays(-(int)a.DayOfWeek) && !item.IsTestAborted)
                    num++;
            return num;
        }
        public List<int> GetClosetAvailiableHour(DateTime date, int hour)
        {
            if (!IsAvailiableOnDateAndHour(date))
                return new List<int>();
            bool[] temp = new bool[6];
            //set matrix of bool that contain the available times of the date week
            for (int i = 0; i < 6; ++i)
                temp[i] = AvailiableWorkTime[(int)date.DayOfWeek, i];
            foreach (var x in TestList)
            {
                if (x.DateOfTest == date && !x.IsTestAborted)
                    temp[x.HourOfTest - 9] = false;
            }
            hour -= 9;
            List<int> AvailiableHours = new List<int>();
            for (int t = 0; t < 6; ++t)
            {
                if (hour + t < 6)
                    if (temp[hour + t])
                        AvailiableHours.Add(hour + t + 9);
                if (hour - t >= 0)
                    if (temp[hour - t])
                        AvailiableHours.Add(hour - t + 9);
            }
            AvailiableHours.Distinct();
            AvailiableHours.Sort();
            return AvailiableHours;//if day is full
        }
        public bool IsAvailiableOnDateAndHour(DateTime date, int hour = -1)
        {
            if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday || GetNumOfTestForSpecificWeek(date) + 1 > MaxTestsPerWeek)
                return false;
            bool isWorkUnlistOneHourOnThisDate = false;
            bool[] tmp = new bool[6];
            for (int i = 0; i < 6; ++i)
            {
                tmp[i] = AvailiableWorkTime[(int)date.DayOfWeek, i];
                if (tmp[i])
                    isWorkUnlistOneHourOnThisDate = true;
            }
            if (isWorkUnlistOneHourOnThisDate)
            {
                foreach (var item in TestList)
                {
                    if (item.DateOfTest == date && !item.IsTestAborted)
                    {
                        tmp[item.HourOfTest - 9] = false;
                    }
                }
                if (hour == -1) //if try to select by date
                {
                    for (int i = 0; i < 6; ++i) //if after filtering there is availiable hour
                    {
                        if (tmp[i])
                            return true;
                    }
                }
                else
                    return tmp[hour - 9];
            }
            return false;//if tester cant work at this day and hour
        }
        public bool IsTestersWorkAtSpesificHour(int hour)
        {
            for (int i = 0; i < 5; ++i)
            {
                if (AvailiableWorkTime[i, hour - 9])
                {
                    return true;
                }
            }
            return false;
        }
        //--------------------------------------------------------------------------------------------------------

        
        //propertys
        //--------------------------------------------------------------------------------------------------------
        public double Seniority { get => seniority; set => seniority = value; }
        public double MaxDistance { get => maxDistance; set => maxDistance = value; }
        public int MaxTestsPerWeek { get => maxTestsPerWeek; set => maxTestsPerWeek = value; }
        public CarTypeEnum TypeCarToTest { get => typeCarToTest; set => typeCarToTest = value; }
        public bool[,] AvailiableWorkTime { get => availableWorkTime; set => availableWorkTime = value; }
        public List<TesterTest> TestList { get => testList; set => testList = value; }
        //--------------------------------------------------------------------------------------------------------

        /// <summary>
        /// overide ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string tmp = "Tester name: " + FirstName + " " + LastName + ".\nID: " + Id + ".\nGender: " + Gender + ".\nDate Of Birth: " + DateOfBirth.ToShortDateString() +
                ".\nPhone number: " + PhoneNumber + ".\nAddress: " + Address + "Seniority: " + Seniority + ".\nType of car: " + TypeCarToTest +
                ".\nMax tests per week: " + MaxTestsPerWeek + ".\nMax distance for test: " + MaxDistance + ".\nDays and working hours:\n";
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        tmp += "SunDay: ";
                        break;
                    case 1:
                        tmp += "Monday: ";
                        break;
                    case 2:
                        tmp += "Tuesday: ";
                        break;
                    case 3:
                        tmp += "Wednesday: ";
                        break;
                    case 4:
                        tmp += "Thursday: ";
                        break;
                    default:
                        break;
                }
                for (int j = 0; j < 6; j++)
                {
                    if (availableWorkTime[i, j]) { int x = j + 9; tmp += x + "  "; }

                }
                tmp += "\n";
            }
            return tmp;
        }
    }
}
