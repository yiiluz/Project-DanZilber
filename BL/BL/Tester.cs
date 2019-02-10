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
        /// <summary>
        /// overide ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string tmp = "שם הבוחן: " + FirstName + " " + LastName + ".\nת.ז.: " + Id + ".\nמין: " + Gender + ".\nתאריך לידה: " + DateOfBirth.ToShortDateString() +
                ".\nמספר פלאפון: " + PhoneNumber + ".\nכתובת: " + Address + "\nמספר שנות וותק: " + Seniority + ".\nמתמחה על רכב מסוג: " + TypeCarToTest +
                ".\nמספר מבחנים מקסימלי בשבוע: " + MaxTestsPerWeek + ".\nמרחק מקסימלי מכתובת התחלה של מבחן: " + MaxDistance + ".\nשעות עבודה שבועיות: \n";
            bool isFirstHouer;
            for (int i = 0; i < 5; i++)
            {
                isFirstHouer = true; ;
                tmp += (Days)i + ": ";
                for (int j = 0; j < 6; j++)
                {
                    if (availableWorkTime[i, j])
                    {
                        if (!isFirstHouer)
                            tmp += ", ";
                        tmp += j + 9;
                        isFirstHouer = false;
                    }
                }
                tmp += "\n";
            }
            return tmp;
        }
    }
}
