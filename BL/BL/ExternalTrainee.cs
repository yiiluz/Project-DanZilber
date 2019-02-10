using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ExternalTrainee
    {
        private readonly string id;
        private CarTypeEnum currCarType;
        private string firstName;
        private string lastName;
        private string schoolName;
        private string teacherName;
        private string phoneNumber;
        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="id"></param>
        public ExternalTrainee(string id)
        {
            this.id = id;
        }

        public ExternalTrainee(Trainee other)
        {
            id = other.Id;
            LastName = other.LastName;
            FirstName = other.FirstName;
            SchoolName = other.SchoolName;
            TeacherName = other.TeacherName;
            PhoneNumber = other.PhoneNumber;
            CurrCarType = other.CurrCarType;
        }

        public CarTypeEnum CurrCarType { get => currCarType; set => currCarType = value; }
        public string SchoolName { get => schoolName; set => schoolName = value; }
        public string TeacherName { get => teacherName; set => teacherName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }

        public string Id => id;


        /// <summary>
        /// override of ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "תעודת זהות של התלמיד: " + Id + "\n" +
                "שם: " + FirstName + " " + LastName + "\n" +
                "מספר פלאפון: " + PhoneNumber + "\n" +
                "לומד כרגע נהיגה על רכב מסוג:  " + CurrCarType + "\n" +
                "שם בית ספר לנהיגה: " + SchoolName + "\n" +
                "שם מורה נהיגה: " + TeacherName + "\n";
        }
    }
}
