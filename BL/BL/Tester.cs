using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Tester : Person
    {
        private int seniority;
        private int maxDistance;
        private int maxTestsPerWeek;
        private CarTypeEnum typeCarToTest;
        private bool[,] availableWorkTime = new bool[5, 6];
        private List<TesterTest> testList = new List<TesterTest>();
        private TesterStatistics statistics;

        //Ctors
        //--------------------------------------------------------------------------------------------------------
        public Tester() : base("") { }
        public Tester(string id) : base(id)
        {
            for (int i = 0; i < 5; ++i)
                for (int j = 0; j < 6; ++j)
                    AvailiableWorkTime[i, j] = false;
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
            Statistics = other.Statistics;
            AvailiableWorkTime = other.AvailiableWorkTime;
            foreach (var item in other.TestList)
                TestList.Add(item);
        }

        //propertys
        //--------------------------------------------------------------------------------------------------------
        public int Seniority { get => seniority; set => seniority = value; }
        public int MaxDistance { get => maxDistance; set => maxDistance = value; }
        public int MaxTestsPerWeek { get => maxTestsPerWeek; set => maxTestsPerWeek = value; }
        public CarTypeEnum TypeCarToTest { get => typeCarToTest; set => typeCarToTest = value; }
        public bool[,] AvailiableWorkTime { get => availableWorkTime; set => availableWorkTime = value; }
        public List<TesterTest> TestList { get => testList; set => testList = value; }
        public TesterStatistics Statistics { get => statistics; set => statistics = value; }

        //--------------------------------------------------------------------------------------------------------

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
