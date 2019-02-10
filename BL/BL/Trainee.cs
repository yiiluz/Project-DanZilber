using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class Trainee used to implement driving trainee
    /// </summary>
    public class Trainee : Person
    {
        private DateTime lastTest = DateTime.MinValue;//////////////////////
        private CarTypeEnum currCarType;
        private int numOfFinishedLessons;
        private string schoolName;
        private string teacherName;
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
            CurrCarType = other.CurrCarType;
            NumOfFinishedLessons = other.NumOfFinishedLessons;
            ExistingLicenses = new List<CarTypeEnum>(other.ExistingLicenses);
            statistics = other.statistics;
            testList = other.TestList;
        }

        public DateTime LastTest { get => lastTest; set => lastTest = value; }
        public CarTypeEnum CurrCarType { get => currCarType; set => currCarType = value; }
        public int NumOfFinishedLessons { get => numOfFinishedLessons; set => numOfFinishedLessons = value; }
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
            string tmp = "שם התלמיד: " + FirstName + " " + LastName + ".\nת.ז.: " + Id + ".\nמין: " + Gender + ".\nתאריך לידה: " + DateOfBirth.ToShortDateString() +
                ".\nמספר פלאפון: " + PhoneNumber + ".\nכתובת מגורים: " + Address + "רשיונות קיימים: " + existingLic +
                ".\nסוג רכב ללמידה נוכחי: " + CurrCarType + ".\nשם בית ספר לנהיגה: " + SchoolName +
                ".\nשם המורה לנהיגה: " + TeacherName + ".\nמספר שיעורים שעשה: " + numOfFinishedLessons + ".\n";
            return tmp;
        }
    }
}
