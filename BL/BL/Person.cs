using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Person
    {
        readonly string id;
        private string lastName;
        private string firstName;
        private string phoneNumber;
        private GenderEnum gender;
        private Address address = new Address();
        private DateTime dateOfBirth = new DateTime();

        public Person(string id)
        {
            this.id = id;
        }

        public string LastName { get => lastName; set => lastName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public GenderEnum Gender { get => gender; set => gender = value; }
        public Address Address { get => address; set => address = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string City { get => address.City; set => address.City = value; }
        public string Street { get => address.Street; set => address.Street = value; }
        public int BuildingNumber { get => address.BuildingNumber; set => address.BuildingNumber = value; }
        public string Id { get => id; }
    }
}
