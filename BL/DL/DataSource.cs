using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DL
{
    internal class DateSource
    {
        /// <summary>
        /// class to represent single configuratuon parameter
        /// </summary>
        internal class ConfigurationParameter
        {
            public bool Readable;
            public bool Writable;
            public object Value;
        }
        private static DateSource data = null;
        internal static List<Test> tests;
        internal static List<Tester> testers;
        internal static List<Trainee> trainees;
        internal static Dictionary<String, ConfigurationParameter> Configuration = new Dictionary<string, ConfigurationParameter>();
        internal static Dictionary<String, bool[,]> Schedules = new Dictionary<string, bool[,]>();
        
        /// <summary>
        /// private default ctor
        /// </summary>
        static DateSource()
        {
            tests = new List<Test>();
            testers = new List<Tester>();
            trainees = new List<Trainee>();
            Configuration.Add("Tester minimum age", new ConfigurationParameter() { Readable = true, Writable = false, Value = 40 });
            Configuration.Add("Minimum days between tests", new ConfigurationParameter() { Readable = true, Writable = false, Value = 14 });
            Configuration.Add("Trainee minimum age", new ConfigurationParameter() { Readable = true, Writable = false, Value = 18 });
            Configuration.Add("Minimum lessons", new ConfigurationParameter() { Readable = true, Writable = false, Value = 20 });
            Configuration.Add("Tester maximum age", new ConfigurationParameter() { Readable = true, Writable = false, Value = 67 });
            Configuration.Add("Serial Number Test", new ConfigurationParameter() { Readable = true, Writable = true, Value = 10000000 });
        }

        /// <summary>
        /// Get object by singelton model
        /// </summary>
        /// <returns></returns>
        public static DateSource GetDSObject()
        {
            //*********************************************************************
            Tester tester = new Tester("111111111");
            tester.FirstName = "avraham";
            tester.DateOfBirth = new DateTime(1970, 01, 01);
            tester.MaxTestsPerWeek = 2;
            tester.TypeCarToTest = CarTypeEnum.MotorCycle;
            tester.MaxDistance = 50;
            for (int i = 0; i < 5; ++i)
                for (int j = 5; j < 6; ++j)
                    tester.AvailableWorkTime[i, j] = true;
            Tester tester1 = new Tester("222222222");
            tester1.FirstName = "yitzhak";
            tester1.DateOfBirth = new DateTime(1970, 01, 01);
            tester1.MaxTestsPerWeek = 2;
            tester1.TypeCarToTest = CarTypeEnum.MotorCycle;
            tester1.MaxDistance = 50;
            for (int i = 0; i < 5; ++i)
                for (int j = 4; j < 6; ++j)
                    tester1.AvailableWorkTime[i, j] = true;
            Tester tester2 = new Tester("333333333");
            tester2.FirstName = "yaakov";
            tester2.DateOfBirth = new DateTime(1970, 01, 01);
            tester2.MaxTestsPerWeek = 2;
            tester2.TypeCarToTest = CarTypeEnum.MotorCycle;
            tester2.MaxDistance = 50;
            for (int i = 0; i < 5; ++i)
                for (int j = 3; j < 6; ++j)
                    tester2.AvailableWorkTime[i, j] = true;
            testers.Add(tester);
            testers.Add(tester1);
            testers.Add(tester2);

            Trainee trainee = new Trainee("444444444");
            trainee.FirstName = "moshe";
            trainee.DateOfBirth = new DateTime(1990, 01, 01);
            trainee.CurrCarType = CarTypeEnum.MotorCycle;
            trainee.NumOfFinishedLessons = 100;
            trainees.Add(trainee);
            //************************************************************************
            if (data == null)
            {
                data = new DateSource();
            }
            return data;
        }
    }

}
