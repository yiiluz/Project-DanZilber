using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using DO;
using System.Threading;

namespace DL
{
    class DAL_XML_imp : BaseDL
    {
        protected static DAL_XML_imp instance = new DAL_XML_imp();
        protected DAL_XML_imp()
        {
            try
            {
                Load(ref TestersRoot, testersRootPath);
            }
            catch
            {
                TestersRoot = new XElement("Testers");
            }
            try
            {
                Load(ref TestsRoot, TestsRootPath);
            }
            catch
            {
                TestsRoot = new XElement("Tests");
            }
            try
            {
                Load(ref TraineesRoot, TraineesRootPath);
            }
            catch
            {
                TraineesRoot = new XElement("Trainees");
            }
            try
            {
                Load(ref ConfigRoot, ConfigRootPath);
            }
            catch
            {
                ConfigRoot = new XElement("Configuration");
            }
            (new Thread(ConfigUpdatdThreadFunc)).Start();
        }
        public static DAL_XML_imp GetAL_XML_Imp { get => instance; }
        public static void ConfigUpdatedAddEvent(Action action)
        {
            instance.ConfigUpdated += action;
        }
        private static volatile bool isConfigUpdated = false;
        static void ConfigUpdatdThreadFunc()
        {
            while (true)
            {
                if (isConfigUpdated)
                {
                    isConfigUpdated = false;
                    instance?.ConfigUpdated();
                }
                Thread.Sleep(1000);
            }
        }
        private void Load(ref XElement t, string a)
        {
            try
            {
                t = XElement.Load(a);
            }
            catch
            {
                throw new DirectoryNotFoundException(a + " upload problem");
            }

        }

        private XElement TestersRoot;
        private XElement TraineesRoot;
        private XElement TestsRoot;
        private XElement ConfigRoot;
        private XElement SchedulesRoot;
        private const string testersRootPath = @"..\..\..\Testers.xml";
        private const string TraineesRootPath = @"..\..\..\Trainees.xml";
        private const string TestsRootPath = @"..\..\..\Tests.xml";
        private const string ConfigRootPath = @"..\..\..\Config.xml";
        private const string SchedulesRootPath = @"..\..\..\Schedules.xml";
        private XElement PersonCreatorToXML(Person t)
        {
            return new XElement("Person", new XElement("ID", t.Id),
            new XElement("Name", new XElement("FirstName", t.FirstName), new XElement("LastName", t.LastName)),
            new XElement("Gender", t.Gender),
            new XElement("Address", new XElement("City", t.Address.City), new XElement("Street", t.Address.Street),
            new XElement("BuildingNumber", t.Address.BuildingNumber)),
            new XElement("DateOfBirth", t.DateOfBirth), new XElement("PhoneNumber", t.PhoneNumber));
        }
        public override void AddTester(Tester T)
        {
            try
            {
                Load(ref TestersRoot, testersRootPath);
            }
            catch (DirectoryNotFoundException r)
            {
                throw r;
            }

            var it = (from item in TestersRoot.Elements()
                      where item.Element("Person").Element("ID").Value == T.Id
                      select item).FirstOrDefault();
            if (it != null)
            {
                throw new DuplicateWaitObjectException("A Tester with this ID already exists in this document: " + testersRootPath);
            }
            TestersRoot.Add(new XElement("Tester", PersonCreatorToXML(T), new XElement("Seniority", T.Seniority),
            new XElement("MaxDistance", T.MaxDistance), new XElement("MaxTestsPerWeek", T.MaxTestsPerWeek),
            new XElement("TypeCarToTest", T.TypeCarToTest)));
            TestersRoot.Save(testersRootPath);
        }
        public override void RemoveTester(string id)
        {
            try
            {
                Load(ref TestersRoot, testersRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            try
            {
                var it = (from item in TestersRoot.Elements()
                          where item.Element("Person").Element("ID").Value == id
                          select item).FirstOrDefault();
                it.Remove();
                TestersRoot.Save(testersRootPath);
            }
            catch
            {
                throw new KeyNotFoundException("There is no Tester with this ID in this document:" + testersRootPath);
            }
        }
        public override void UpdateTesterDetails(Tester T)
        {
            RemoveTester(T.Id);
            AddTester(T);
            TestersRoot.Save(testersRootPath);
        }
        public override void AddTrainee(Trainee T)
        {
            try
            {
                Load(ref TraineesRoot, TraineesRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in TraineesRoot.Elements()
                      where item.Element("Person").Element("ID").Value == T.Id
                      select item).FirstOrDefault();
            if (it != null)
            {
                throw new DuplicateWaitObjectException("A Trainee with this ID already exists in this document: " + TraineesRootPath);
            }
            TraineesRoot.Add(new XElement("Trainee", PersonCreatorToXML(T), new XElement("CurrCarType", T.CurrCarType),
                new XElement("NumOfFinishedLessons", T.NumOfFinishedLessons), new XElement("NumOfTests", T.NumOfTests),
                new XElement("IsAlreadyDidTest", T.IsAlreadyDidTest), new XElement("SchoolName", T.SchoolName),
                new XElement("TeacherName", T.TeacherName)));
            TraineesRoot.Save(TraineesRootPath);
        }
        public override void RemoveTrainee(string id)
        {
            try
            {
                Load(ref TraineesRoot, TraineesRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            try
            {
                var it = (from item in TraineesRoot.Elements()
                          where item.Element("Person").Element("ID").Value == id
                          select item).FirstOrDefault();
                it.Remove();
                TraineesRoot.Save(TraineesRootPath);
            }
            catch
            {
                throw new KeyNotFoundException("There is no Trainee with this ID in this document: " + TraineesRootPath);
            }
        }
        public override void UpdateTraineeDetails(Trainee T)
        {
            try
            {
                RemoveTrainee(T.Id);
                AddTrainee(T);
                TraineesRoot.Save(TraineesRootPath);
            }
            catch (KeyNotFoundException e) { throw e; }
            catch (DirectoryNotFoundException d) { throw d; }
        }
        public override void AddTest(Test t)
        {
            try
            {
                Load(ref TestsRoot, TestsRootPath);
                Load(ref TraineesRoot, TraineesRootPath);
                Load(ref TestersRoot, testersRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in TestsRoot.Elements()
                      where item.Element("TestId").Value == t.TestId
                      select item).FirstOrDefault();
            if (it != null)
            {
                throw new DuplicateWaitObjectException("Test with this ID already exists in this document: "
                 + testersRootPath);
            }
            it = (from item in TraineesRoot.Elements()
                  where item.Element("Person").Element("ID").Value == t.TraineeId
                  select item).FirstOrDefault();
            if (it == null)
            {
                throw new KeyNotFoundException("ERROR! The trainee does not exist in this document: "
                            + TraineesRootPath);
            }
            it = (from item in TestersRoot.Elements()
                  where item.Element("Person").Element("ID").Value == t.TesterId
                  select item).FirstOrDefault();
            if (it == null)
            {
                throw new KeyNotFoundException("ERROR! The tester does not exist in this document: "
                            + testersRootPath);
            }
            TestsRoot.Add(new XElement("Test", new XElement("TestId", t.TestId), new XElement("TesterId", t.TesterId),
                new XElement("TraineeId", t.TraineeId), new XElement("DateOfTest", t.DateOfTest),
                new XElement("HourOfTest", t.HourOfTest), new XElement("StartTestAddress",
                new XElement("City", t.StartTestAddress.City), new XElement("Street", t.StartTestAddress.Street),
                new XElement("BuildingNumber", t.StartTestAddress.BuildingNumber)), new XElement("CarType", t.CarType),
                new XElement("DistanceKeeping", t.DistanceKeeping), new XElement("ReverseParking", t.ReverseParking),
                new XElement("MirrorsCheck", t.MirrorsCheck), new XElement("Signals", t.Signals),
                new XElement("CorrectSpeed", t.CorrectSpeed), new XElement("IsPassed", t.IsPassed),
                new XElement("TesterNotes", t.TesterNotes), new XElement("IsTesterUpdateStatus", t.IsTesterUpdateStatus),
                new XElement("IsTestAborted", t.IsTestAborted)));
            TestersRoot.Save(testersRootPath);
            TraineesRoot.Save(TraineesRootPath);
            TestsRoot.Save(TestsRootPath);
        }
        public override void RemoveTest(string id)
        {
            try
            {
                Load(ref TestsRoot, TestsRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            try
            {
                var it = (from item in TestsRoot.Elements()
                          where item.Element("TestId").Value == id
                          select item).FirstOrDefault();
                it.Remove();
                TestsRoot.Save(TestsRootPath);
            }
            catch
            {
                throw new KeyNotFoundException("There is no Test with this testId in this document: " + TestsRootPath);
            }
        }
        public override void UpdateTestDetails(Test t)
        {
            try
            {
                RemoveTest(t.TestId);
                AddTest(t);
                TestsRoot.Save(TestsRootPath);
            }
            catch (KeyNotFoundException e) { throw e; }
            catch (DirectoryNotFoundException d) { throw d; }
        }
        public override List<Tester> GetTestersList()
        {
            try
            {
                Load(ref TestersRoot, testersRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in TestersRoot.Elements()
                      select new Tester(item.Element("Person").Element("ID").Value)
                      {
                          LastName = item.Element("Person").Element("Name").Element("LastName").Value,
                          FirstName = item.Element("Person").Element("Name").Element("FirstName").Value,
                          PhoneNumber = item.Element("Person").Element("PhoneNumber").Value,
                          Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), item.Element("Person").Element("Gender").Value),
                          Address = new Address(item.Element("Person").Element("Address").Element("City").Value,
                          item.Element("Person").Element("Address").Element("Street").Value,
                          int.Parse(item.Element("Person").Element("Address").Element("BuildingNumber").Value)),
                          DateOfBirth = DateTime.Parse(item.Element("Person").Element("DateOfBirth").Value),
                          Seniority = int.Parse(item.Element("Seniority").Value),
                          MaxDistance = int.Parse(item.Element("MaxDistance").Value),
                          MaxTestsPerWeek = int.Parse(item.Element("MaxTestsPerWeek").Value),
                          TypeCarToTest = (CarTypeEnum)Enum.Parse(typeof(CarTypeEnum), item.Element("TypeCarToTest").Value),
                      }).ToList();

            return it;
        }
        public override List<Trainee> GetTraineeList()
        {
            try
            {
                Load(ref TraineesRoot, TraineesRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in TraineesRoot.Elements()
                      select new Trainee(item.Element("Person").Element("ID").Value)
                      {
                          LastName = item.Element("Person").Element("Name").Element("LastName").Value,
                          FirstName = item.Element("Person").Element("Name").Element("FirstName").Value,
                          PhoneNumber = item.Element("Person").Element("PhoneNumber").Value,
                          Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), (item.Element("Person").Element("Gender")).Value),
                          Address = new Address(item.Element("Person").Element("Address").Element("City").Value,
                          item.Element("Person").Element("Address").Element("Street").Value,
                          int.Parse(item.Element("Person").Element("Address").Element("BuildingNumber").Value)),
                          DateOfBirth = DateTime.Parse(item.Element("Person").Element("DateOfBirth").Value),
                          CurrCarType = (CarTypeEnum)Enum.Parse(typeof(CarTypeEnum), item.Element("CurrCarType").Value),
                          NumOfFinishedLessons = int.Parse(item.Element("NumOfFinishedLessons").Value),
                          NumOfTests = int.Parse(item.Element("NumOfTests").Value),
                          IsAlreadyDidTest = bool.Parse(item.Element("IsAlreadyDidTest").Value),
                          SchoolName = item.Element("SchoolName").Value,
                          TeacherName = item.Element("TeacherName").Value,
                      }).ToList();
            return it;
        }
        public override List<Test> GetTestsList()
        {
            try
            {
                Load(ref TestsRoot, TestsRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in TestsRoot.Elements()
                      select new Test(item.Element("TestId").Value)
                      {
                          TesterId = item.Element("TesterId").Value,
                          TraineeId = item.Element("TraineeId").Value,
                          DateOfTest = DateTime.Parse(item.Element("DateOfTest").Value),
                          HourOfTest = int.Parse(item.Element("HourOfTest").Value),
                          StartTestAddress = new Address(item.Element("StartTestAddress").Element("City").Value,
                          item.Element("StartTestAddress").Element("Street").Value,
                          int.Parse(item.Element("StartTestAddress").Element("BuildingNumber").Value)),
                          CarType = (CarTypeEnum)Enum.Parse(typeof(CarTypeEnum), item.Element("CarType").Value),
                          DistanceKeeping = Convert.ToBoolean(item.Element("DistanceKeeping").Value),
                          ReverseParking = Convert.ToBoolean(item.Element("ReverseParking").Value),
                          MirrorsCheck = Convert.ToBoolean(item.Element("MirrorsCheck").Value),
                          Signals = Convert.ToBoolean(item.Element("Signals").Value),
                          CorrectSpeed = Convert.ToBoolean(item.Element("CorrectSpeed").Value),
                          IsPassed = Convert.ToBoolean(item.Element("IsPassed").Value),
                          TesterNotes = item.Element("TesterNotes").Value,
                          IsTesterUpdateStatus = Convert.ToBoolean(item.Element("IsTesterUpdateStatus").Value),
                          IsTestAborted = Convert.ToBoolean(item.Element("IsTestAborted").Value)
                      }).ToList();
            try
            {

                TestsRoot.Save(TestsRootPath);
            }
            catch { }
            return it;
        }
        public override Dictionary<String, Object> GetConfig()
        {
            try
            {
                Load(ref ConfigRoot, ConfigRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            foreach (var item in ConfigRoot.Elements())
            {
                if (Convert.ToBoolean(item.Element("Value").Element("Readable").Value))
                {
                    keyValues.Add(item.Element("Key").Value, int.Parse(item.Element("Value").Element("value").Value));
                }
            }
            ConfigRoot.Save(ConfigRootPath);
            return keyValues;
        }
        public override Object GetConfig(String s)
        {
            try
            {
                Load(ref ConfigRoot, ConfigRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            foreach (var X in ConfigRoot.Elements())
            {
                if (X.Element("Key").Value == s)
                {
                    if (Convert.ToBoolean(X.Element("Value").Element("Readable").Value))
                        return int.Parse(X.Element("Value").Element("value").Value);
                    throw new AccessViolationException("ERROR! There is no permission to read this configutation property in this document: " + ConfigRootPath);
                }
            }
            throw new KeyNotFoundException("ERROR! There is no configuration feature with this name in this document: " + ConfigRootPath);
        }
        public override void SetConfig(string parm, Object value)
        {
            try
            {
                Load(ref ConfigRoot, ConfigRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            foreach (var item in ConfigRoot.Elements())
            {
                if (item.Element("Key").Value == parm)
                {
                    if (Convert.ToBoolean(item.Element("Value").Element("Writable").Value))
                    {
                        item.Element("Value").Element("value").Value = value.ToString();
                        ConfigRoot.Save(ConfigRootPath);
                        isConfigUpdated = true;
                        return;
                    }
                    throw new AccessViolationException("ERROR! There is no permission to write on this configutation property in this document: " + ConfigRootPath);
                }
            }
            throw new KeyNotFoundException("ERROR! There is no configuration feature with this name in this document: " + ConfigRootPath);
        }
        public override void AddTesterSchedule(string id, bool[,] sched)
        {
            try
            {
                Load(ref SchedulesRoot, SchedulesRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in SchedulesRoot.Elements()
                      where item.Element("ID").Value == id
                      select item).FirstOrDefault();
            if (it != null) { throw new DuplicateWaitObjectException("This testerSchedule already exists in this document: " + SchedulesRootPath); }
            SchedulesRoot.Add(new XElement("Schedule", new XElement("ID", id), new XElement("WorkDays",
                new XElement("Day", new XElement("Hour", sched[0, 0]), new XElement("Hour", sched[0, 1]), new XElement("Hour", sched[0, 2]),
                new XElement("Hour", sched[0, 3]), new XElement("Hour", sched[0, 4]), new XElement("Hour", sched[0, 5])),
                new XElement("Day", new XElement("Hour", sched[1, 0]), new XElement("Hour", sched[1, 1]), new XElement("Hour", sched[1, 2]),
                new XElement("Hour", sched[1, 3]), new XElement("Hour", sched[1, 4]), new XElement("Hour", sched[1, 5])),
                new XElement("Day", new XElement("Hour", sched[2, 0]), new XElement("Hour", sched[2, 1]), new XElement("Hour", sched[2, 2]),
                new XElement("Hour", sched[2, 3]), new XElement("Hour", sched[2, 4]), new XElement("Hour", sched[2, 5])),
                new XElement("Day", new XElement("Hour", sched[3, 0]), new XElement("Hour", sched[3, 1]), new XElement("Hour", sched[3, 2]),
                new XElement("Hour", sched[3, 3]), new XElement("Hour", sched[3, 4]), new XElement("Hour", sched[3, 5])),
                new XElement("Day", new XElement("Hour", sched[4, 0]), new XElement("Hour", sched[4, 1]), new XElement("Hour", sched[4, 2]),
                new XElement("Hour", sched[4, 3]), new XElement("Hour", sched[4, 4]), new XElement("Hour", sched[4, 5])))));
            SchedulesRoot.Save(SchedulesRootPath);
        }
        public override void UpdateTesterSchedule(string id, bool[,] sched)
        {
            RemoveTesterSchedule(id);
            AddTesterSchedule(id, sched);
        }
        public override bool[,] GetTesterSchedule(string id)
        {
            bool[,] temp = new bool[5, 6];
            int j = 0, i = 0;
            try
            {
                Load(ref SchedulesRoot, SchedulesRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in SchedulesRoot.Elements()
                      where item.Element("ID").Value == id
                      select item).FirstOrDefault();
            if (it == null) { throw new KeyNotFoundException("There is not testerSchedule in this document: " + SchedulesRootPath + ". For this ID" + id); }
            foreach (var x in it.Element("WorkDays").Elements())
            {
                foreach (var v in x.Elements())
                {
                    temp[j, i] = Convert.ToBoolean(v.Value);
                    i++;
                }
                j++;
                i = 0;
            }
            return temp;
        }
        public override void RemoveTesterSchedule(string id)
        {
            try
            {
                Load(ref SchedulesRoot, SchedulesRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            try
            {
                var it = (from item in SchedulesRoot.Elements()
                          where item.Element("ID").Value == id
                          select item).FirstOrDefault();
                it.Remove();
                SchedulesRoot.Save(SchedulesRootPath);
            }
            catch
            {
                throw new KeyNotFoundException("There is not testerSchedule in this document: " + SchedulesRootPath);
            }

        }
    }
}
