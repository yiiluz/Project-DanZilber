using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ExternalTester
    {
        private readonly string id;
        private string lastName;
        private string firstName;
        private string phoneNumber;
        private CarTypeEnum typeCarToTest;

        public ExternalTester(string id)
        {
            this.id = id;
        }
        public ExternalTester(Tester t)
        {
            id = t.Id;
            LastName = t.LastName;
            FirstName = t.FirstName;
            TypeCarToTest = t.TypeCarToTest;
            PhoneNumber = t.PhoneNumber;
        }
        public string Id => id;

        public string LastName { get => lastName; set => lastName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public CarTypeEnum TypeCarToTest { get => typeCarToTest; set => typeCarToTest = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }

        /// <summary>
        /// override of ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string tmp = "Tester ID: " + Id + "\n" +
                "Tester name: " + FirstName + " " + LastName + "\n" +
                "Tester Phone number: " + PhoneNumber + "\n" +
                 "Tester Type Car To Test: " + TypeCarToTest + "\n";
            return tmp;
        }
    }
}
