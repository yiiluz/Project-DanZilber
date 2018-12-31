using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Trainee : Person
    {

        private DateTime lastTest = new DateTime();
        private CarTypeEnum currCarType;
        private GearboxTypeEnum currGearType;
        private int numOfFinishedLessons;
        private int numOfTests;
        private string schoolName;
        private string teacherName;
        private bool isAlreadyDidTest;
        private List<CarTypeEnum> existingLicenses = new List<CarTypeEnum>();
        private List<TraineeTest> testList = new List<TraineeTest>();

        public Trainee(DO.Trainee other) : base(other.Id)
        {
            LastName = other.LastName;
            FirstName = other.FirstName;
            SchoolName = other.SchoolName;
            TeacherName = other.TeacherName;
            PhoneNumber = other.PhoneNumber;
            Gender = (GenderEnum)other.Gender;
            Address = new Address(other.Address.City, other.Address.Street, other.Address.BuildingNumber);
            DateOfBirth = other.DateOfBirth;
            LastTest = new DateTime(other.LastTest.Ticks);
            CurrCarType = (CarTypeEnum)other.CurrCarType;
            CurrGearType = (GearboxTypeEnum)other.CurrGearType;
            NumOfFinishedLessons = other.NumOfFinishedLessons;
            NumOfTests = other.NumOfTests;
            IsAlreadyDidTest = other.IsAlreadyDidTest;
            ExistingLicenses = new List<CarTypeEnum>();
            foreach (var item in other.ExistingLicenses)
                ExistingLicenses.Add((CarTypeEnum)item);
        }
        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="id"></param>
        public Trainee(string id) : base(id) { }

        public Trainee(Trainee other) : base(other.Id)
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
            CurrGearType = other.CurrGearType;
            NumOfFinishedLessons = other.NumOfFinishedLessons;
            NumOfTests = other.NumOfTests;
            IsAlreadyDidTest = other.IsAlreadyDidTest;
            ExistingLicenses = new List<CarTypeEnum>(other.ExistingLicenses);
        }

        public DateTime LastTest { get => lastTest; set => lastTest = value; }
        public CarTypeEnum CurrCarType { get => currCarType; set => currCarType = value; }
        public GearboxTypeEnum CurrGearType { get => currGearType; set => currGearType = value; }
        public int NumOfFinishedLessons { get => numOfFinishedLessons; set => numOfFinishedLessons = value; }
        public int NumOfTests { get => numOfTests; set => numOfTests = value; }
        public bool IsAlreadyDidTest { get => isAlreadyDidTest; set => isAlreadyDidTest = value; }
        public List<CarTypeEnum> ExistingLicenses { get => existingLicenses; set => existingLicenses = value; }
        public List<TraineeTest> TestList { get => testList; set => testList = value; }
        public string SchoolName { get => schoolName; set => schoolName = value; }
        public string TeacherName { get => teacherName; set => teacherName = value; }


        /// <summary>
        /// override of ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string existingLic = "";
            foreach (var item in ExistingLicenses)
                existingLic += item + ", ";
            existingLic.Remove(existingLic.Length - 2, existingLic.Length);
            string tmp = "Trainee name: " + FirstName + " " + LastName + ".\nID: " + Id + ".\nGender: " + Gender + ".\nDate Of Birth: " + DateOfBirth.ToShortDateString() +
                ".\nPhone number: " + PhoneNumber + ".\nAddress: " + Address + ".\nExisting linsences: " + existingLic +
                ".\nType of current Gearbox: " + CurrGearType + ".\nType of current Car: " + CurrCarType + ".\nSchool name: " + SchoolName +
                ".\nTeacher name: " + TeacherName + ".\nSum of pased lessons: " + numOfFinishedLessons + ".\n";
            return tmp;
        }
    }
}
