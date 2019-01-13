using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class TesterTest : TestResult
    {
        private string testId;
        private DateTime dateOfTest = new DateTime();
        private int hourOfTest;
        private CarTypeEnum carType;
        private Address startTestAddress = new Address();
        private bool isTesterUpdateStatus;
        private bool isTestAborted;

        public TesterTest(Test test)
        {
            TestId = test.TestId;
            DateOfTest = test.DateOfTest;
            HourOfTest = test.HourOfTest;
            CarType = test.CarType;
            StartTestAddress = test.StartTestAddress;
            DistanceKeeping = test.DistanceKeeping;
            ReverseParking = test.ReverseParking;
            MirrorsCheck = test.MirrorsCheck;
            Signals = test.Signals;
            CorrectSpeed = test.CorrectSpeed;
            IsPassed = test.IsPassed;
            TesterNotes = test.TesterNotes;
            IsTesterUpdateStatus = test.IsTesterUpdateStatus;
            IsTestAborted = test.IsTestAborted;
        }
        public TesterTest(DO.Test test)
        {
            TestId = test.TestId;
            DateOfTest = test.DateOfTest;
            HourOfTest = test.HourOfTest;
            CarType = (CarTypeEnum)test.CarType;
            StartTestAddress = new Address(test.StartTestAddress.City, test.StartTestAddress.Street, test.StartTestAddress.BuildingNumber);
            DistanceKeeping = test.DistanceKeeping;
            ReverseParking = test.ReverseParking;
            MirrorsCheck = test.MirrorsCheck;
            Signals = test.Signals;
            CorrectSpeed = test.CorrectSpeed;
            IsPassed = test.IsPassed;
            TesterNotes = test.TesterNotes;
            IsTesterUpdateStatus = test.IsTesterUpdateStatus;
            IsTestAborted = test.IsTestAborted;
        }

        public string TestId { get => testId; set => testId = value; }
        public DateTime DateOfTest { get => dateOfTest; set => dateOfTest = value; }
        public int HourOfTest { get => hourOfTest; set => hourOfTest = value; }
        public CarTypeEnum CarType { get => carType; set => carType = value; }
        public Address StartTestAddress { get => startTestAddress; set => startTestAddress = value; }
        public bool IsTesterUpdateStatus { get => isTesterUpdateStatus; set => isTesterUpdateStatus = value; }
        public bool IsTestAborted { get => isTestAborted; set => isTestAborted = value; }

        public override string ToString()
        {
            string tmp = "";
            tmp += "Test Serial Number: " + TestId + "\n"
                + "Test Date: " + DateOfTest.ToShortDateString() + "\n"
                + "Test Hour: " + HourOfTest + ":00\n"
                + "Start Test Address: " + StartTestAddress + "\n";
            if (!IsTestAborted)
            {
                tmp += "Test Status: Active\n";
                if (DateOfTest < DateTime.Now)
                    if (IsTesterUpdateStatus)
                    {
                        tmp += base.ToString();
                    }
                    else
                    {
                        tmp += "You need to update this test results!!.\n";
                    }
                else
                {
                    tmp += "That is all for now. After the test, Update test result as quickly as you can.\n";
                }
            }
            else
            {
                tmp += "Test Status: Aborted";
            }
            return tmp;
        }
    }
}
