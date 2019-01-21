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
    class DAL_XML_imp //: IDAL
    {
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
        private const string testersRootPath = @"Testers.xml";
        private const string TraineesRootPath = @"Trainees.xml";
        private const string TestsRootPath = @"Tests.xml";
       private XElement PersonCreator(Person t)
        {
            return new XElement("*****", new XElement("ID", t.Id),
            new XElement("Name", new XElement("FirstName", t.FirstName),new XElement("LastName",t.LastName)),
            new XElement("Gender",t.Gender),
            new XElement("Address", new XElement("City",t.Address.City),new XElement("Street", t.Address.Street), new XElement("BuildingNumber", t.Address.BuildingNumber)),
            new XDocument("DateOfBirth",t.DateOfBirth),new XElement("PhoneNumber",t.PhoneNumber));
        }
        public void AddTester(Tester T)
        {
            try
            {
                Load(TestersRoot, testersRootPath);
            }
            catch(DirectoryNotFoundException r)           
            {
                throw  r;
            }
            var it = (from item in TestersRoot.Elements()
                      where item.Element("ID").Value.ToString() == T.Id
                      select item).FirstOrDefault();
            if(it != null) { throw new DuplicateWaitObjectException("A Tester with this ID already exists in this document: " + testersRootPath); }
            TestersRoot.Add(new XElement("Tester", PersonCreator(T), new XElement("Seniority", T.Seniority),
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
            catch(DirectoryNotFoundException e)
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
            try
            {
                Load(TestersRoot, testersRootPath);
            }
            catch(DirectoryNotFoundException e)
            {
                throw e;
            }
            try
            {
                RemoveTester(T.Id);
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            AddTester(T);
            TestersRoot.Save(testersRootPath);           
    }
        public void AddTrainee(Trainee T)
        {
            try
            {
                Load(TraineesRoot, TraineesRootPath);
            }
            catch(DirectoryNotFoundException e)
            {
                throw e;
            }
            var it = (from item in TraineesRoot.Elements()
                      where item.Element("ID").Value.ToString() == T.Id
                      select item).FirstOrDefault();
            if(it != null) { throw new DuplicateWaitObjectException("A Trainee with this ID already exists in this document: " + TraineesRootPath); }
            TraineesRoot.Add("Trainee", PersonCreator(T), new XElement("CurrCarType", T.CurrCarType),
                new XElement("NumOfFinishedLessons", T.NumOfFinishedLessons), new XElement("NumOfTests", T.NumOfTests),
                new XElement("IsAlreadyDidTest", T.IsAlreadyDidTest), new XElement("SchoolName", T.SchoolName), new XElement("TeacherName", T.TeacherName));
            TraineesRoot.Save(TraineesRootPath);
        }
       private void RemoveTrainee(string id)
        {
            try
            {
                Load(TraineesRoot, TraineesRootPath);
            }
            catch(DirectoryNotFoundException e)
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
                Load(TraineesRoot, TraineesRootPath);
            }
            catch(DirectoryNotFoundException e)
            {
                throw e;
            }
            try
            {
                RemoveTrainee(T.Id);
            }
            catch(KeyNotFoundException e)
            {
                throw e;
            }
            AddTrainee(T);
            TraineesRoot.Save(TraineesRootPath);
        }
        public void AddTest(Test t)
        {
            try
            {
                Load(TestsRoot, TestsRootPath);
            }
            catch(DirectoryNotFoundException e)
            {
                throw e;
            }
            TestsRoot.Add(new XElement(""));
        }
    }
}
