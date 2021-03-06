﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class TesterTest used to implement test object from the tester view only.
    /// </summary>
    public class TesterTest : TestResult
    {
        private string testId;
        private DateTime dateOfTest = new DateTime();
        private int hourOfTest;
        private CarTypeEnum carType;
        private Address startTestAddress = new Address();
        private bool isTesterUpdateStatus;
        private bool isTestAborted;

        public TesterTest(Test test)//used to create TesterTest from TestObject
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

        public string TestId { get => testId; set => testId = value; }
        public DateTime DateOfTest { get => dateOfTest; set => dateOfTest = value; }
        public int HourOfTest { get => hourOfTest; set => hourOfTest = value; }
        public CarTypeEnum CarType { get => carType; set => carType = value; }
        public Address StartTestAddress { get => startTestAddress; set => startTestAddress = value; }
        public bool IsTesterUpdateStatus { get => isTesterUpdateStatus; set => isTesterUpdateStatus = value; }
        public bool IsTestAborted { get => isTestAborted; set => isTestAborted = value; }
        public string City { get => StartTestAddress.City; set => startTestAddress.City = value; }
        public string Street { get => StartTestAddress.Street; set => startTestAddress.Street = value; }
        public int BuildingNumber { get => StartTestAddress.BuildingNumber; set => startTestAddress.BuildingNumber = value; }

        public override string ToString()
        {
            string tmp = "";
            tmp += "מספר מבחן: " + TestId + "\n"
                + "תאריך המבחן: " + DateOfTest.ToShortDateString() + "\n"
                + "שעת התחלה: " + HourOfTest + ":00\n"
                + "כתובת התחלת המבחן: " + StartTestAddress + "\n"
                + "סטטוס המבחן: " + (IsTestAborted ? "בוטל." :
                (DateOfTest > DateTime.Now ? "נכון לעכשיו, זה הכל. בסיום המבחן, עדכן תוצאות במהירות האפשרית." :
                (IsTesterUpdateStatus ? "עידכת תוצאות עבור מבחן זה." : "המבחן מחכה לעידכון תוצאות!!"))) + "\n";
            if (IsTesterUpdateStatus)
                tmp += "תוצאות המבחן כפי שעידכנת:\n" + base.ToString();
            return tmp;
        }
    }
}
