using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Trainee : Person
    {

        private DateTime lastTest = DateTime.MinValue;//////////////////////
        private CarTypeEnum currCarType;
        private int numOfFinishedLessons;
        private int numOfTests;
        private string schoolName;
        private string teacherName;
        private bool isAlreadyDidTest;
        private List<CarTypeEnum> existingLicenses = new List<CarTypeEnum>();
        private List<TraineeTest> testList = new List<TraineeTest>();
        private TraineeStatistics statistics;

        public Trainee(string id) : base(id) { }

        public Trainee(string id, Trainee other) : base(id)//needed for clone the trainee from binding
        {
            LastName = other.LastName;
            FirstName = other.FirstName;
            SchoolName = other.SchoolName;
            TeacherName = other.TeacherName;
            PhoneNumber = other.PhoneNumber;
            Gender = other.Gender;
            Address = new Address(other.Address);
            DateOfBirth = other.DateOfBirth;
            LastTest = new DateTime(other.LastTest.Ticks);
            CurrCarType = other.CurrCarType;
            NumOfFinishedLessons = other.NumOfFinishedLessons;
            NumOfTests = other.NumOfTests;
            IsAlreadyDidTest = other.IsAlreadyDidTest;
            ExistingLicenses = new List<CarTypeEnum>(other.ExistingLicenses);
            statistics = other.statistics;
        }

        public DateTime LastTest { get => lastTest; set => lastTest = value; }
        public CarTypeEnum CurrCarType { get => currCarType; set => currCarType = value; }
        public int NumOfFinishedLessons { get => numOfFinishedLessons; set => numOfFinishedLessons = value; }
        public int NumOfTests { get => numOfTests; set => numOfTests = value; }
        public bool IsAlreadyDidTest { get => isAlreadyDidTest; set => isAlreadyDidTest = value; }
        public List<CarTypeEnum> ExistingLicenses { get => existingLicenses; set => existingLicenses = value; }
        public List<TraineeTest> TestList { get => testList; set => testList = value; }
        public string SchoolName { get => schoolName; set => schoolName = value; }
        public string TeacherName { get => teacherName; set => teacherName = value; }
        public TraineeStatistics Statistics { get => statistics; set => statistics = value; }


        /// <summary>
        /// override of ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string existingLic = "";
            if (ExistingLicenses.Count != 0)
            {
                foreach (var item in ExistingLicenses)
                    existingLic += item + ", ";
                existingLic = existingLic.Remove(existingLic.Length - 2, 2);
            }
            else
            {
                existingLic = "Non";
            }
            string tmp = "Trainee name: " + FirstName + " " + LastName + ".\nID: " + Id + ".\nGender: " + Gender + ".\nDate Of Birth: " + DateOfBirth.ToShortDateString() +
                ".\nPhone number: " + PhoneNumber + ".\nAddress: " + Address + "Existing linsences: " + existingLic +
                ".\nType of current Car: " + CurrCarType + ".\nSchool name: " + SchoolName +
                ".\nTeacher name: " + TeacherName + ".\nSum of pased lessons: " + numOfFinishedLessons + ".\n";
            return tmp;
        }
    }
}
