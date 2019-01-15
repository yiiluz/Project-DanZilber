using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Tester : Person
    {
        private int seniority;
        private int maxDistance;
        private int maxTestsPerWeek;
        private CarTypeEnum typeCarToTest;
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
        }
        public int Seniority { get => seniority; set => seniority = value; }
        public int MaxDistance { get => maxDistance; set => maxDistance = value; }
        public int MaxTestsPerWeek { get => maxTestsPerWeek; set => maxTestsPerWeek = value; }
        public CarTypeEnum TypeCarToTest { get => typeCarToTest; set => typeCarToTest = value; }


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
