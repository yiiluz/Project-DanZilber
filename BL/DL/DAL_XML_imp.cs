using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using DO;
namespace DL
{
    class DAL_XML_imp : IDAL
    {
        protected static DAL_XML_imp dAL_XML_ = new DAL_XML_imp();
        protected DAL_XML_imp() { }
        public static DAL_XML_imp GetAL_XML_Imp { get => dAL_XML_; }
        private void Load(XElement t, string a)
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
        private const string testersRootPath = @"Testers.xml";
        private const string TraineesRootPath = @"Trainees.xml";
        private const string TestsRootPath = @"Tests.xml";
        private const string ConfigRootPath = @"Config.xml";
        private XElement PersonCreatorToXML(Person t)
        {
            return new XElement("*****", new XElement("ID", t.Id),
            new XElement("Name", new XElement("FirstName", t.FirstName), new XElement("LastName", t.LastName)),
            new XElement("Gender", t.Gender),
            new XElement("Address", new XElement("City", t.Address.City), new XElement("Street", t.Address.Street),
            new XElement("BuildingNumber", t.Address.BuildingNumber)),
            new XElement("DateOfBirth", t.DateOfBirth), new XElement("PhoneNumber", t.PhoneNumber));
        }
        public void AddTester(Tester T)
        {
            try
            {
                Load(TestersRoot, testersRootPath);
            }
            catch (DirectoryNotFoundException r)
            {
                throw r;
            }
            var it = (from item in TestersRoot.Elements()
                      where item.Element("ID").Value.ToString() == T.Id
                      select item).FirstOrDefault();
            if (it != null)
            {
                throw new DuplicateWaitObjectException("A Tester with this ID already exists in this" +
   " document: " + testersRootPath);
            }
            TestersRoot.Add(new XElement("Tester", PersonCreatorToXML(T), new XElement("Seniority", T.Seniority),
            new XElement("MaxDistance", T.MaxDistance), new XElement("MaxTestsPerWeek", T.MaxTestsPerWeek),
            new XElement("TypeCarToTest", T.TypeCarToTest)));
            TestersRoot.Save(testersRootPath);
        }
        public void RemoveTester(string id)
        {
            try
            {
                Load(TestersRoot, testersRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            try
            {
                var it = (from item in TestersRoot.Elements()
                          where item.Element("ID").Value.ToString() == id
                          select item).FirstOrDefault();
                it.Remove();
                TestersRoot.Save(testersRootPath);
            }
            catch
            {
                throw new KeyNotFoundException("There is no Tester with this ID in this document:" + testersRootPath);
            }
        }
        public void UpdateTesterDetails(Tester T)
        {
            RemoveTester(T.Id);
            AddTester(T);
            TestersRoot.Save(testersRootPath);
        }
        public void AddTrainee(Trainee T)
        {
            try
            {
                Load(TraineesRoot, TraineesRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in TraineesRoot.Elements()
                      where item.Element("ID").Value.ToString() == T.Id
                      select item).FirstOrDefault();
            if (it != null)
            {
                throw new DuplicateWaitObjectException("A Trainee with this ID already exists in this document: " + TraineesRootPath);
            }
            TraineesRoot.Add("Trainee", PersonCreatorToXML(T), new XElement("CurrCarType", T.CurrCarType),
                new XElement("NumOfFinishedLessons", T.NumOfFinishedLessons), new XElement("NumOfTests", T.NumOfTests),
                new XElement("IsAlreadyDidTest", T.IsAlreadyDidTest), new XElement("SchoolName", T.SchoolName),
                new XElement("TeacherName", T.TeacherName));
            TraineesRoot.Save(TraineesRootPath);
        }
        public void RemoveTrainee(string id)
        {
            try
            {
                Load(TraineesRoot, TraineesRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            try
            {
                var it = (from item in TraineesRoot.Elements()
                          where item.Element("ID").Value.ToString() == id
                          select item).FirstOrDefault();
                it.Remove();
                TraineesRoot.Save(TraineesRootPath);
            }
            catch
            {
                throw new KeyNotFoundException("There is no Trainee with this ID in this document: " + TraineesRootPath);
            }
        }
        public void UpdateTraineeDetails(Trainee T)
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
        public void AddTest(Test t)
        {
            try
            {
                Load(TestsRoot, TestsRootPath);
                Load(TraineesRoot, TraineesRootPath);
                Load(TestersRoot, testersRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in TestersRoot.Elements()
                      where item.Element("TestId").Value.ToString() == t.TestId
                      select item).FirstOrDefault();
            if (it != null)
            {
                throw new DuplicateWaitObjectException("Test with this ID already exists in this document: "
   + testersRootPath);
            }
            it = (from item in TraineesRoot.Elements()
                  where item.Element("ID").Value.ToString() == t.TraineeId
                  select item).FirstOrDefault();
            if (it == null)
            {
                throw new KeyNotFoundException("ERROR! The trainee does not exist in this document: "
   + TraineesRootPath);
            }
            it = (from item in TestersRoot.Elements()
                  where item.Element("ID").Value.ToString() == t.TesterId
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
        public void RemoveTest(string id)
        {
            try
            {
                Load(TestsRoot, TestsRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            try
            {
                var it = (from item in TestsRoot.Elements()
                          where item.Element("TestId").Value.ToString() == id
                          select item).FirstOrDefault();
                it.Remove();
                TestsRoot.Save(TestsRootPath);
            }
            catch
            {
                throw new KeyNotFoundException("There is no Test with this testId in this document: " + TestsRootPath);
            }
        }
        public void UpdateTestDetails(Test t)
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
        public List<Tester> GetTestersList()
        {
            try
            {
                Load(TestersRoot, testersRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in TestersRoot.Elements()
                      select new Tester(item.Element("ID").Value.ToString())
                      {
                          LastName = item.Element("Name").Element("LastName").Value,
                          FirstName = item.Element("Name").Element("FirstName").Value,
                          PhoneNumber = item.Element("PhoneNumber").Value,
                          Gender = (GenderEnum)int.Parse(item.Element("Gender").Value),
                          Address = new Address(item.Element("Address").Element("City").Value,
                          item.Element("Address").Element("Street").Value,
                          int.Parse(item.Element("Address").Element("BuildingNumber").Value)),
                          DateOfBirth = DateTime.Parse(item.Element("DateOfBirth").Value),
                          Seniority = int.Parse(item.Element("Seniority").Value),
                          MaxDistance = int.Parse(item.Element("MaxDistance").Value),
                          MaxTestsPerWeek = int.Parse(item.Element("MaxTestsPerWeek").Value),
                          TypeCarToTest = (CarTypeEnum)int.Parse(item.Element("TypeCarToTest").Value),
                      }).ToList();
            TestsRoot.Save(testersRootPath);
            return it;
        }
        public List<Trainee> GetTraineeList()
        {
            try
            {
                Load(TraineesRoot, TraineesRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in TestersRoot.Elements()
                      select new Trainee(item.Element("ID").Value.ToString())
                      {
                          LastName = item.Element("Name").Element("LastName").Value,
                          FirstName = item.Element("Name").Element("FirstName").Value,
                          PhoneNumber = item.Element("PhoneNumber").Value,
                          Gender = (GenderEnum)int.Parse(item.Element("Gender").Value),
                          Address = new Address(item.Element("Address").Element("City").Value,
                          item.Element("Address").Element("Street").Value,
                          int.Parse(item.Element("Address").Element("BuildingNumber").Value)),
                          DateOfBirth = DateTime.Parse(item.Element("DateOfBirth").Value),
                          CurrCarType = (CarTypeEnum)int.Parse(item.Element("CurrCarType").Value),
                          NumOfFinishedLessons = int.Parse(item.Element("NumOfFinishedLessons").Value),
                          NumOfTests = int.Parse(item.Element("NumOfTests").Value),
                          IsAlreadyDidTest = bool.Parse(item.Element("IsAlreadyDidTest").Value),
                          SchoolName = item.Element("SchoolName").Value,
                          TeacherName = item.Element("TeacherName").Value,
                      }).ToList();
            TraineesRoot.Save(TraineesRootPath);
            return it;
        }
        public List<Test> GetTestsList()
        {
            try
            {
                Load(TestsRoot, TestsRootPath);
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
                          CarType = (CarTypeEnum)int.Parse(item.Element("CarType").Value),
                          DistanceKeeping = bool.Parse(item.Element("DistanceKeeping").Value),
                          ReverseParking = bool.Parse(item.Element("ReverseParking").Value),
                          MirrorsCheck = bool.Parse(item.Element("MirrorsCheck").Value),
                          Signals = bool.Parse(item.Element("Signals").Value),
                          CorrectSpeed = bool.Parse(item.Element("CorrectSpeed").Value),
                          IsPassed = bool.Parse(item.Element("IsPassed").Value),
                          TesterNotes = item.Element("TesterNotes").Value,
                          IsTesterUpdateStatus = bool.Parse(item.Element("IsTesterUpdateStatus").Value),
                          IsTestAborted = bool.Parse(item.Element("IsTestAborted").Value)
                      }).ToList();
            TestsRoot.Save(TestsRootPath);
            return it;
        }
        public Dictionary<String, Object> GetConfig()
        {
            try
            {
                Load(ConfigRoot, ConfigRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            foreach (var item in ConfigRoot.Elements())
            {
                if (bool.Parse(item.Element("Value").Element("Readable").Value))
                {
                    keyValues.Add(item.Element("Key").Value, int.Parse(item.Element("Value").Element("value").Value));
                }
            }
            ConfigRoot.Save(ConfigRootPath);
            return keyValues;
        }
        public Object GetConfig(String s)
        {
            try
            {
                Load(ConfigRoot, ConfigRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            foreach (var X in ConfigRoot.Elements())
            {
                if (X.Element("Key").Value == s)
                {
                    if (bool.Parse(X.Element("Value").Element("Readable").Value))
                        return int.Parse(X.Element("Value").Element("value").Value);
                    throw new AccessViolationException("ERROR! There is no permission to read this configutation property in this document: " + ConfigRootPath);
                }
            }
            throw new KeyNotFoundException("ERROR! There is no configuration feature with this name in this document: " + ConfigRootPath);
        }
        public void SetConfig(String parm, Object value)
        {
            try
            {
                Load(ConfigRoot, ConfigRootPath);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            foreach (var item in ConfigRoot.Elements())
            {
                if (item.Element("Key").Value == parm)
                {
                    if (bool.Parse(item.Element("Value").Element("Readable").Value))
                        item.Element("Value").Element("Readable").Value = parm;
                    throw new AccessViolationException("ERROR! There is no permission to write on this configutation property in this document: " + ConfigRootPath);
                }
            }
            throw new KeyNotFoundException("ERROR! There is no configuration feature with this name in this document: " + ConfigRootPath);
        }

        public void AddTesterSchedule(string id, bool[,] sched)
        {
            throw new NotImplementedException();
        }
    }

}
