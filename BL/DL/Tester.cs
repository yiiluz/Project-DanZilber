using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Tester : Person
    {
        private double seniority;
        private double maxDistance;
        private int maxTestsPerWeek;
        private CarTypeEnum typeCarToTest;
        private bool[,] availableWorkTime = new bool[5, 6];
        private List<Test> testList = new List<Test>();
        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="id"></param>
        public Tester(string id) : base(id) { }
        public Tester(Tester other) : base(other.Id)
        {
            LastName = other.LastName;
            FirstName = other.FirstName;
            SchoolName = other.SchoolName;
            TeacherName = other.TeacherName;
            PhoneNumber = other.PhoneNumber;
            Gender = other.Gender;
            Address = new Address(other.Address);
            DateOfBirth = other.DateOfBirth;
            Seniority = other.Seniority;
            MaxDistance = other.MaxDistance;
            MaxTestsPerWeek = other.MaxTestsPerWeek;
            TypeCarToTest = other.TypeCarToTest;
            AvailableWorkTime = other.AvailableWorkTime;
            foreach (var item in TestList)
                TestList.Add(item);
        }
        public double Seniority { get => seniority; set => seniority = value; }
        public double MaxDistance { get => maxDistance; set => maxDistance = value; }
        public int MaxTestsPerWeek { get => maxTestsPerWeek; set => maxTestsPerWeek = value; }
        public CarTypeEnum TypeCarToTest { get => typeCarToTest; set => typeCarToTest = value; }
        public bool[,] AvailableWorkTime { get => availableWorkTime; set => availableWorkTime = value; }
        public List<Test> TestList { get => testList; set => testList = value; }

        public bool IsTesterAvailiableOnDate(DateTime t, int hour)
        {
            if (AvailableWorkTime[t.DayOfYear, hour] == false)
            {
                return false;
            }
            return testList.FindIndex(x => x.DateOfTest.DayOfWeek == t.DayOfWeek && x.HourOfTest == hour) == -1;
        }
        public int GetNumOfTestThisWeek()
        {
            int num = 0;
            foreach (var item in TestList)
                if (item.DateOfTest.AddDays(-(int)item.DateOfTest.DayOfWeek) == (DateTime.Now).AddDays(-(int)DateTime.Now.DayOfWeek))
                    num++;
            return num;
        }
        /// <summary>
        /// overide ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string tmp = "Tester name: " + FirstName + " " + LastName + ".\nID: " + Id + ".\nGender: " + Gender + ".\nDate Of Birth: " + DateOfBirth.ToShortDateString() +
                ".\nPhone number: " + PhoneNumber + ".\nAddress: " + Address + ".\nSeniority: " + Seniority + ".\nType of car: " + TypeCarToTest +
                ".\nMax tests per week: " + MaxTestsPerWeek + ".\nMax distance for test: " + MaxDistance + ".\n";
            return tmp;
        }
    }
}
