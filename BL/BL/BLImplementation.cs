using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DL;
namespace BL
{
    internal class BLImplementation : IBL
    {
        private static IDAL instance = null;
        public BLImplementation()
        {
            try
            {
                instance = DL.Factory.GetDLObj("lists");
            }
            catch (NotImplementedException e)
            {
                throw e;
            }
        }
        public void AddTester(Tester t)
        {
            int minAge, maxAge;
            try
            {
                minAge = (int)instance.GetConfig("Tester minimum age");
                maxAge = (int)instance.GetConfig("Tester maximun age");
            }
            catch (AccessViolationException e)
            {
                throw e;
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            int testerAge = (DateTime.Now.Year - t.DateOfBirth.Year);
            if (testerAge < minAge || testerAge > maxAge)
            {
                throw new ArgumentOutOfRangeException("Tester age is not on range of " + minAge + "-" + maxAge);
            }
            else try
                {
                    instance.AddTester(CreateDOFromBO.CreateDOTester(t));
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
        }
        public void RemoveTester(string id)
        {
            bool exist = GetTestersList().Exists(x => x.Id == id);
            if (!exist)
            {
                throw new KeyNotFoundException("Can't remove this tester becauze he is not on the system.");
            }
            else try
                {
                    instance.RemoveTester(id);
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
        }
        public void UpdateTesterDetails(Tester t)
        {
            bool exist = GetTestersList().Exists(x => x.Id == t.Id);
            if (!exist)
                throw new KeyNotFoundException("Can't update this tester becauze he is not on the system.");
            else try
                {
                    instance.UpdateTesterDetails(CreateDOFromBO.CreateDOTester(t));
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
        }
        public void AddTrainee(Trainee t)
        {
            int minAge;
            try
            {
                minAge = (int)instance.GetConfig("Trainee minimum age");
            }
            catch (AccessViolationException e)
            {
                throw e;
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            int traineeAge = (DateTime.Now.Year - t.DateOfBirth.Year);
            if (traineeAge < minAge)
            {
                throw new ArgumentOutOfRangeException("Trainee age is not on above " + minAge + ".");
            }
            else try
                {
                    instance.AddTrainee(CreateDOFromBO.CreateDoTrainee(t));
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
        }
        public void RemoveTrainee(string id)
        {
            bool exist = GetTraineeList().Exists(x => x.Id == id);
            if (!exist)
            {
                throw new KeyNotFoundException("Can't remove this trainee becauze he is not on the system.");
            }
            else try
                {
                    instance.RemoveTrainee(id);
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
        }
        public void UpdateTraineeDetails(Trainee t)
        {
            bool exist = GetTraineeList().Exists(x => x.Id == t.Id);
            if (!exist)
                throw new KeyNotFoundException("Can't update this trainee because he is not on the system.");
            else try
                {
                    instance.UpdateTraineeDetails(CreateDOFromBO.CreateDoTrainee(t));
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
        }
        public void AddTest(Test t)
        {
            bool traineeExist = GetTraineeList().Exists(x => x.Id == t.ExTrainee.Id);
            bool testerExist = GetTestersList().Exists(x => x.Id == t.ExTester.Id);
            string errors = "ERROR!\n";
            if (!testerExist || !traineeExist)
            {
                if (!traineeExist)
                    errors += "The trainee linked to test is not exist on the system.\n";
                if (!testerExist)
                    errors += "The tester linked to test is not exist on the system.\n";
                throw new KeyNotFoundException(errors);
            }
            else
            {
                Trainee trainee = GetTraineeList().Find(x => x.Id == t.ExTrainee.Id);
                Tester tester = GetTestersList().Find(x => x.Id == t.ExTester.Id);
                if (trainee.IsAlreadyDidTest)
                {
                    int minDaysBetweenTests = -1;
                    try
                    {
                        minDaysBetweenTests = (int)instance.GetConfig("Minimum days between tests");
                    }
                    catch (AccessViolationException e)
                    {
                        errors += (e.Message + "\n");
                    }
                    catch (KeyNotFoundException e)
                    {
                        errors += (e.Message + "\n");
                    }
                    if (minDaysBetweenTests != -1 && (DateTime.Now - trainee.LastTest).Days < minDaysBetweenTests)
                        errors += "The trainee last days was closer then allowed.\n";
                }
                int minLesson = -1;
                try
                {
                    minLesson = (int)instance.GetConfig("Minimum lessons");
                }
                catch (AccessViolationException e)
                {
                    errors += (e.Message + "\n");
                }
                catch (KeyNotFoundException e)
                {
                    errors += (e.Message + "\n");
                }
                if (minLesson != -1 && trainee.NumOfFinishedLessons < minLesson)
                {
                    errors += "Trainee did not passed enough lessons for test.\n";
                }
                if (tester.MaxTestsPerWeek + 1 > tester.GetNumOfTestThisWeek())
                    errors += "Tester allready have maximum tests for this week.\n";

            }
        }
        public void RemoveTest(string id)
        {
            bool exist = GetTestsList().Exists(x => x.TestId == id);
            if (!exist)
            {
                throw new KeyNotFoundException("Can't remove this test because he is not on the system.");
            }
            else try
                {
                    instance.RemoveTest(id);
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
        }
        public void UpdateTest(Test t)
        {
            bool exist = GetTestsList().Exists(x => x.TestId == t.TestId);
            if (!exist)
                throw new KeyNotFoundException("Can't update this test because he is not on the system.");
            else try
                {
                    instance.UpdateTest(CreateDOFromBO.CreateDOTest(t));
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
        }
        public List<Tester> GetTestersList()
        {
            var lst = from item in instance.GetTestersList() select new Tester(item);
            foreach (var x in lst)
            {
                var lstTests = from item in GetTestsList() where item.ExTester.Id == x.Id select new TesterTest(item);
                x.TestList = (List<TesterTest>)lstTests;
            }
            return (List<Tester>)lst;
        }
        public List<Trainee> GetTraineeList()
        {
            var lst = from item in instance.GetTraineeList() select new Trainee(item);
            foreach (var x in lst)
            {
                var lstTests = from item in GetTestsList() where item.ExTrainee.Id == x.Id select new TraineeTest(item);
                x.TestList = (List<TraineeTest>)lstTests;
            }
            return (List<Trainee>)lst;
        }
        public List<Test> GetTestsList()
        {
            var lst = from item in instance.GetTestsList() select new Test(item);
            foreach (var x in lst)
            {
                x.ExTester = new ExternalTester(GetTesterByID(x.ExTester.Id));
                x.ExTrainee = new ExternalTrainee(GetTraineeByID(x.ExTrainee.Id));
            }
            return (List<Test>)lst;
        }
        public Trainee GetTraineeByID(string id)
        {
            if (!(GetTraineeList().Exists(x => x.Id == id)))
                throw new KeyNotFoundException("Trainee id not exist on system.\n");
            else
            {
                return new Trainee(GetTraineeList().Find(x => x.Id == id));
            }
        }
        public Tester GetTesterByID(string id)
        {
            if (!(GetTestersList().Exists(x => x.Id == id)))
                throw new KeyNotFoundException("Tester id not exist on system.\n");
            else
            {
                return new Tester(GetTestersList().Find(x => x.Id == id));
            }
        }
        public Test GetTestByID(string id)
        {
            if (!(GetTestsList().Exists(x => x.TestId == id)))
                throw new KeyNotFoundException("Test id not exist on system.\n");
            else
            {
                return new Test(GetTestsList().Find(x => x.TestId == id));
            }
        }
















        Dictionary<string, Object> keyValuesBL = instance.GetConfig();
        public IEnumerable<Tester> AvailableTeacher(DateTime time, int hour)
        {
            var AvailableTesters = from item in instance.GetTestersList()
                                   orderby item.LastName, item.FirstName
                                   where item.IsTesterAvailiableOnDate(time, hour) == true
                                   select new Tester(item);
            return AvailableTesters;
        }
        public IEnumerable<Test> GetTestsPartialListByPredicate(Func<DO.Test, bool> func)
        {
            var StandOnTheCondition = from item in instance.GetTestsList()
                                      orderby item.TestId
                                      where func(item) == true
                                      select new Test(item);
            return StandOnTheCondition;
        }
        public int NumberOfTests(Trainee t)
        {
            return t.NumOfTests;
        }
        public bool IsHaveLicense(Trainee T, CarTypeEnum car)
        {

            return T.ExistingLicenses.Exists(x => x == car);
        }
        public IEnumerable<Test> TheTestsWillBeDoneToday_Month(DateTime t, bool Byday)
        {
            if (Byday == true)
            {
                var toDay = from item in instance.GetTestsList()
                            orderby item.TestId
                            where item.DateOfTest.DayOfYear == t.DayOfYear
                            select new Test(item);
                return toDay;
            }
            var ThisMonth = from item in GetTestsList()
                            orderby item.TestId
                            where item.DateOfTest.Month == t.Month
                            select item;
            return ThisMonth;
        }
        public IEnumerable<IGrouping<CarTypeEnum, Tester>> GetTestersBySpecialization(bool byOrder = false)
        {
            if (byOrder == true)
            {
                var TestersGroupsWithOrder = from item in GetTestersList()
                                             orderby item.LastName, item.FirstName
                                             group item by item.TypeCarToTest
                                             into g
                                             orderby g.Key
                                             select g;
                return TestersGroupsWithOrder;
            }
            var TestersGroupsWithoutOrder = from item in GetTestersList()
                                            group item by item.TypeCarToTest;
            return TestersGroupsWithoutOrder;
        }
        public IEnumerable<IGrouping<string, Trainee>> GetStudentGroupsBySchool(bool byOrder = false)
        {
            if (byOrder == true)
            {
                var StudentGroupsByAttributeWithOrder = from item in GetTraineeList()
                                                        orderby item.LastName, item.FirstName
                                                        group item by item.SchoolName
                                                        into g
                                                        orderby g.Key
                                                        select g;
                return StudentGroupsByAttributeWithOrder;
            }
            var StudentGroupsByAttributeWithOutOrder = from item in GetTraineeList()
                                                       group item by item.SchoolName;
            return StudentGroupsByAttributeWithOutOrder;
        }
        public IEnumerable<IGrouping<string, Trainee>> GetStudentGroupsByTeacher(bool byOrder = false)
        {
            if (byOrder == true)
            {
                var StudentGroupsByTeacherWithOrder = from item in GetTraineeList()
                                                      orderby item.LastName, item.FirstName
                                                      group item by item.SchoolName
                                                      into g
                                                      orderby g.Key
                                                      select g;
                return StudentGroupsByTeacherWithOrder;
            }
            var StudentGroupsByTeacherWithOutOrder = from item in GetTraineeList()
                                                     group item by item.SchoolName;
            return StudentGroupsByTeacherWithOutOrder;
        }
        public IEnumerable<IGrouping<int, Trainee>> GetStudentsGroupedaccordingByNumOfTests(bool byOrder = false)
        {
            if (byOrder == true)
            {
                var StudentsGroupedaccordingByNumOfTestsWithOrder = from item in GetTraineeList()
                                                                    orderby item.LastName, item.FirstName
                                                                    group item by item.NumOfTests
                                                                    into g
                                                                    orderby g.Key
                                                                    select g;
                return StudentsGroupedaccordingByNumOfTestsWithOrder;
            }
            var StudentsGroupedaccordingByNumOfTestsWithOutrder = from item in GetTraineeList()
                                                                  group item by item.NumOfTests;
            return StudentsGroupedaccordingByNumOfTestsWithOutrder;
        }
        public IEnumerable<IGrouping<int, Tester>> GetTestersGroupedAccordingToYearsOfExperience()
        {
            var TestersGroupedAccordingToYearsOfExperience = from item in GetTestersList()
                                                             orderby item.LastName, item.FirstName
                                                             group item by item.Seniority
                                                             into g
                                                             orderby g.Key
                                                             select g;
            return (IEnumerable<IGrouping<int, Tester>>)TestersGroupedAccordingToYearsOfExperience;
        }
    }
}
