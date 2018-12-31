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
        internal static Dictionary<String, bool[,]> Schedules;
        
        /// <summary>
        /// private default ctor
        /// </summary>
        static DateSource()
        {
            tests = new List<Test>();
            testers = new List<Tester>();
            trainees = new List<Trainee>();
            Configuration.Add("Tester minimum age", new ConfigurationParameter() { Readable = true, Writable = false, Value = 40 });
            Configuration.Add("Minimun days between tests", new ConfigurationParameter() { Readable = true, Writable = false, Value = 14 });
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
            if (data == null)
            {
                data = new DateSource();
            }
            return data;
        }
    }

}
