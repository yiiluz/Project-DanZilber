﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class TraineeTest used to implement test object from the trainee view only.
    /// </summary>
    public class TraineeTest : TestResult
    {
        private string testId;
        private DateTime dateOfTest = new DateTime();
        private int hourOfTest;
        private CarTypeEnum carType;
        private Address startTestAddress = new Address();
        private bool isTesterUpdateStatus;
        private bool isTestAborted;

        public TraineeTest(Test test) //used to create TraineeTest from Test
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
                + "שעת התחלת המבחן: " + HourOfTest + ":00\n"
                + "כתובת התחלת המבחן: " + StartTestAddress + "\n";
            if (!IsTestAborted)
            {
                if (DateOfTest > DateTime.Now)
                {
                    tmp += "בהצלחה!!\n";
                    return tmp;
                }
                if (IsTesterUpdateStatus)
                {
                    tmp += base.ToString();
                }
                else
                {
                    tmp += "המתן בסבלנות. הבוחן עדיין לא עידכן תוצאות. אם עבר יותר מיומיים ממועד הבחינה ולא התקבלו תוצאות, צור קשר עם המשרד.";
                }
            }
            else
            {
                tmp += "שים לב: המבחן בוטל!";
            }
            return tmp;
        }
    }
}
