﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DL
{
    internal class DataSource
    {
        protected DataSource() { }
        private static DataSource instanceOfDS = new DataSource();
        public static DataSource GetDSObject { get => instanceOfDS; }
        /// <summary>
        /// class to represent single configuratuon parameter
        /// </summary>
        internal class ConfigurationParameter
        {
            public bool Readable;
            public bool Writable;
            public object Value;
        }
        internal static List<Test> tests;
        internal static List<Tester> testers;
        internal static List<Trainee> trainees;
        internal static Dictionary<String, ConfigurationParameter> Configuration = new Dictionary<string, ConfigurationParameter>();
        internal static Dictionary<String, bool[,]> Schedules = new Dictionary<string, bool[,]>();

        /// <summary>
        /// private default ctor
        /// </summary>
        static DataSource()
        {
            tests = new List<Test>();
            testers = new List<Tester>();
            trainees = new List<Trainee>();
            Configuration.Add("גיל בוחן מינימלי", new ConfigurationParameter() { Readable = true, Writable = true, Value = 40 });
            Configuration.Add("מינימום ימים בין מבחנים", new ConfigurationParameter() { Readable = true, Writable = false, Value = 14 });
            Configuration.Add("גיל נבחן מינימלי", new ConfigurationParameter() { Readable = true, Writable = false, Value = 18 });
            Configuration.Add("מספר שיעורים מינימלי", new ConfigurationParameter() { Readable = true, Writable = false, Value = 20 });
            Configuration.Add("גיל בוחן מקסימל", new ConfigurationParameter() { Readable = true, Writable = false, Value = 67 });
            Configuration.Add("מספר מבחן", new ConfigurationParameter() { Readable = true, Writable = true, Value = 10000000 });
            Configuration.Add("גיל נבחן מקסימלי", new ConfigurationParameter() { Readable = true, Writable = true, Value = 80 });
            Configuration.Add("סיסמת מנהל המערכת", new ConfigurationParameter() { Readable = true, Writable = true, Value = 1111 });
            Configuration.Add("סיסמת ניהול משרדי", new ConfigurationParameter() { Readable = true, Writable = true, Value = 1111 });
            //*********************************************************************
            Tester tester = new Tester("111111111");
            tester.FirstName = "אברהם";
            tester.LastName = "אבינו";
            tester.Address = new Address("חברון", "קריית ארבע", 16);
            tester.DateOfBirth = new DateTime(1970, 01, 04);
            tester.MaxTestsPerWeek = 2;
            tester.TypeCarToTest = CarTypeEnum.MotorCycle;
            tester.MaxDistance = 50;
            bool[,] tmp = new bool[5, 6];
            for (int i = 0; i < 5; ++i)
                for (int j = 5; j < 6; ++j)
                    tmp[i, j] = true;

            Tester tester1 = new Tester("222222222");
            tester1.FirstName = "יצחק";
            tester1.LastName = "אילוז";
            tester1.DateOfBirth = new DateTime(1975, 12, 05);
            tester1.Address = new Address("אלעד", "מאיר", 16);
            tester1.MaxTestsPerWeek = 2;
            tester1.TypeCarToTest = CarTypeEnum.MotorCycle;
            tester1.MaxDistance = 50;
            bool[,] tmp1 = new bool[5, 6];
            for (int i = 0; i < 5; ++i)
                for (int j = 4; j < 6; ++j)
                    tmp1[i, j] = true;

            Tester tester2 = new Tester("333333333");
            tester2.FirstName = "Yaakov";
            tester2.LastName = "Bardugo";
            tester2.Address = new Address("פתח תקווה", "יהודה הלוי", 6);
            tester2.DateOfBirth = new DateTime(1967, 02, 04);
            tester2.MaxTestsPerWeek = 2;
            tester2.TypeCarToTest = CarTypeEnum.MotorCycle;
            tester2.MaxDistance = 40;
            bool[,] tmp2 = new bool[5, 6];
            for (int i = 0; i < 5; ++i)
                for (int j = 3; j < 6; ++j)
                    tmp2[i, j] = true;

            testers.Add(tester);
            testers.Add(tester1);
            testers.Add(tester2);
            Schedules.Add(tester.Id, tmp);
            Schedules.Add(tester1.Id, tmp1);
            Schedules.Add(tester2.Id, tmp2);

            Trainee trainee = new Trainee("444444444");
            trainee.FirstName = "משה";
            trainee.LastName = "כהן";
            trainee.DateOfBirth = new DateTime(1990, 11, 01);
            trainee.CurrCarType = CarTypeEnum.MotorCycle;
            trainee.NumOfFinishedLessons = 100;
            trainees.Add(trainee);

            Test test = new Test("11111111");
            test.TesterId = "333333333";
            test.TraineeId = "444444444";
            test.DateOfTest = new DateTime(2019,1,1);
            test.HourOfTest = 12;
            test.CarType = CarTypeEnum.MotorCycle;
            test.IsTestAborted = false;
            test.IsTesterUpdateStatus = false;
            tests.Add(test);
            //************************************************************************
        }



    }
}

