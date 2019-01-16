using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Test : TestResult
    {
        private readonly string testId = "";
        private ExternalTrainee exTrainee;
        private ExternalTester exTester;
        private DateTime dateOfTest = new DateTime();
        private int hourOfTest;
        private CarTypeEnum carType;
        private Address startTestAddress = new Address();
        private bool isTesterUpdateStatus;
        private bool isTestAborted = false;

        public Test() { }

        public Test(string id)
        {
            this.testId = id;
        }

        /// <summary>
        /// Copy constructor. used for create copy of original test with differents dates.
        /// </summary>
        /// <param name="Test obj to copy"></param>
        public Test(Test other)
        {
            testId = other.TestId;
            ExTrainee = other.ExTrainee;
            ExTester = other.ExTester;
            DateOfTest = other.DateOfTest;
            HourOfTest = other.HourOfTest;
            CarType = other.CarType;
            StartTestAddress = new Address(other.StartTestAddress.City, other.StartTestAddress.Street, other.StartTestAddress.BuildingNumber);
            DistanceKeeping = other.DistanceKeeping;
            ReverseParking = other.ReverseParking;
            MirrorsCheck = other.MirrorsCheck;
            Signals = other.Signals;
            CorrectSpeed = other.CorrectSpeed;
            IsPassed = other.IsPassed;
            TesterNotes = other.TesterNotes;
            IsTesterUpdateStatus = other.isTesterUpdateStatus;
            IsTestAborted = other.IsTestAborted;
        }

        public ExternalTrainee ExTrainee { get => exTrainee; set => exTrainee = value; }
        public ExternalTester ExTester { get => exTester; set => exTester = value; }
        public DateTime DateOfTest { get => dateOfTest; set => dateOfTest = value; }
        public int HourOfTest { get => hourOfTest; set => hourOfTest = value; }
        public CarTypeEnum CarType { get => carType; set => carType = value; }
        public Address StartTestAddress { get => startTestAddress; set => startTestAddress = value; }
        public bool IsTesterUpdateStatus { get => isTesterUpdateStatus; set => isTesterUpdateStatus = value; }
        public string TestId { get => testId; }
        public string City { get => StartTestAddress.City; set => startTestAddress.City = value; }
        public string Street { get => StartTestAddress.Street; set => startTestAddress.Street = value; }
        public int BuildingNumber { get => StartTestAddress.BuildingNumber; set => startTestAddress.BuildingNumber = value; }
        public bool IsTestAborted { get => isTestAborted; set => isTestAborted = value; }

        public override string ToString()
        {
            string tmp = "Test ID: " + TestId + "\n\n"
                + "Tester Details:\n" + ExTester + "\n"
                + "Trainee Details:\n" + ExTrainee + "\n"
                + "Date of Test: " + DateOfTest.ToShortDateString() + "\n"
                + "Hour of Test: " + HourOfTest + ":00\n"
                + "Test's start address: " + StartTestAddress + "\n"
                + (IsTesterUpdateStatus ? "Test Results:\n" + base.ToString() : "")
                + "Test Status: " + (IsTestAborted ? "Aborted" : "Active") + "\n";
            return tmp;
        }
    }
}
