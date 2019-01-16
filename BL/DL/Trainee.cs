using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Trainee : Person
    {
        private CarTypeEnum currCarType;
        private int numOfFinishedLessons;
        private int numOfTests;
        private bool isAlreadyDidTest;
        private string schoolName;
        private string teacherName;

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
            CurrCarType = other.CurrCarType;
            NumOfFinishedLessons = other.NumOfFinishedLessons;
            NumOfTests = other.NumOfTests;
            IsAlreadyDidTest = other.IsAlreadyDidTest;
        }
        
        public CarTypeEnum CurrCarType { get => currCarType; set => currCarType = value; }
        public int NumOfFinishedLessons { get => numOfFinishedLessons; set => numOfFinishedLessons = value; }
        public int NumOfTests { get => numOfTests; set => numOfTests = value; }
        public bool IsAlreadyDidTest { get => isAlreadyDidTest; set => isAlreadyDidTest = value; }
        public string SchoolName { get => schoolName; set => schoolName = value; }
        public string TeacherName { get => teacherName; set => teacherName = value; }

        /// <summary>
        /// override of ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string tmp = "Trainee name: " + FirstName + " " + LastName + ".\nID: " + Id + ".\nGender: " + Gender + ".\nDate Of Birth: " + DateOfBirth.ToShortDateString() +
                ".\nPhone number: " + PhoneNumber + ".\nAddress: " + Address +
                 ".\nType of current Car: " + CurrCarType + ".\nSchool name: " + SchoolName +
                ".\nTeacher name: " + TeacherName + ".\nSum of pased lessons: " + numOfFinishedLessons + ".\n";
            return tmp;
        }
    }
}
