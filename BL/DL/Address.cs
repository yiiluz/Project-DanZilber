using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct Address
    {
        private string city;
        private string street;
        private int buildingNumber;


        /// <summary>
        /// Argument ctor of struct Address
        /// </summary>
        /// <param name="city"></param>
        /// <param name="street"></param>
        /// <param name="building number"></param>
        public Address(string c, string s, int n)
        {
            city = c;
            street = s;
            buildingNumber = n;

        }
        public Address(Address other)
        {
            city = other.City;
            street = other.Street;
            buildingNumber = other.BuildingNumber;
        }
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }
        public string Street
        {
            get
            {
                return street;
            }
            set
            {
                street = value;
            }
        }
        public int BuildingNumber
        {
            get
            {
                return buildingNumber;
            }
            set
            {
                buildingNumber = value;
            }
        }
        public override string ToString()
        {
            string tmp = "City: " + City + ".\nStreet: " + Street + ". Building number: " + BuildingNumber + ".\n";
            return tmp;
        }
    }
}
