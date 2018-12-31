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
        /// <summary>
        /// static variable of DL
        /// </summary>
        private static IDAL instance = null;
        /// <summary>
        /// default ctor. initialize the instance of DL
        /// </summary>
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
                    foreach (var item in GetTestsList())
                    {
                        if (item.ExTester.Id == id)
                            RemoveTest(item.TestId);
                    }
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
            else
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
                try
                {
                    instance.UpdateTesterDetails(CreateDOFromBO.CreateDOTester(t));
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
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
            else
            {
                foreach (var item in GetTestsList())
                    RemoveTest(item.TestId);
                try
                {
                    instance.RemoveTrainee(id);
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
            }
        }
        public void UpdateTraineeDetails(Trainee t)
        {
            bool exist = GetTraineeList().Exists(x => x.Id == t.Id);
            if (!exist)
                throw new KeyNotFoundException("Can't update this trainee because he is not on the system.");
            else
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
                try
                {
                    instance.UpdateTraineeDetails(CreateDOFromBO.CreateDoTrainee(t));
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
            }
        }


        public void AddTest(Test t)
        {
            //check if trainee & tester already on system.
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
            //if trainee and tester on the system
            else
            {
                //get the trainee and tester objects
                Trainee trainee = GetTraineeByID(t.ExTrainee.Id);
                Tester tester = GetTesterByID(t.ExTester.Id);
                if (!tester.IsAvailiableOnDate(t.DateOfTest, t.HourOfTest))
                    errors += "Tester is not availiable for this specific date and hour.\n";
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
                    if (minDaysBetweenTests != -1) //if success on bring in the configuration of "Minimum days between tests"
                    {
                        bool flag = false;
                        foreach (var item in trainee.TestList) //check if the new test too close to other
                        {
                            if (t.CarType == item.CarType && Math.Abs((t.DateOfTest - item.DateOfTest).Days) < 7)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                            errors += "The date of test is closed than alowed to other trainee test on the same car type.\n";
                    }
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
                if (minLesson != -1 && trainee.NumOfFinishedLessons < minLesson) //if trainee didnt did enough lessons
                {
                    errors += "Trainee did not passed enough lessons for test.\n";
                }
                if (tester.MaxTestsPerWeek + 1 > tester.GetNumOfTestForSpecificWeek(t.DateOfTest)) //if the tester already did maximun tests in the new test week
                    errors += "Tester allready have maximum tests for this week.\n";
                if (trainee.ExistingLicenses.Exists(x => x == t.CarType)) //if trainee already have license on the test type car
                    errors += "Trainee already have that kind of license.\n";
                if (trainee.CurrCarType != tester.TypeCarToTest) //if the tester car type to test different from current trainee car type
                    errors += "Type car of tester to test different than the needed test.\n";
                if (errors == "ERROR!\n") //if there was no errors
                {
                    int serial = -1;
                    try
                    {
                        serial = (int)instance.GetConfig("Serial Number Test"); //get the serial number of the test
                    }
                    catch (AccessViolationException e)
                    {
                        errors += (e.Message + "\n");
                    }
                    catch (KeyNotFoundException e)
                    {
                        errors += (e.Message + "\n");
                    }
                    if (serial > 0)
                    {
                        bool flag = true;
                        try
                        {
                            instance.SetConfig("Serial Number Test", ++serial); //update the test serial number
                        }
                        catch (AccessViolationException e)
                        {
                            errors += (e.Message + "\n");
                            flag = false;
                        }
                        catch (KeyNotFoundException e)
                        {
                            errors += (e.Message + "\n");
                            flag = false;
                        }
                        if (flag) //if everything was good
                        {
                            try
                            {
                                instance.AddTest(CreateDOFromBO.CreateDOTest(t, Convert.ToString(serial))); //add the test
                            }
                            catch (DuplicateWaitObjectException e)
                            {
                                errors += (e.Message + "\n");
                                return;
                            }
                            catch (KeyNotFoundException e)
                            {
                                errors += (e.Message + "\n");
                                return;
                            }
                            //update trainee deteils
                            if (trainee.IsAlreadyDidTest && t.DateOfTest > trainee.LastTest) //update last test date
                                trainee.LastTest = t.DateOfTest;
                            if (t.IsPassed) //update license
                                trainee.ExistingLicenses.Add(t.CarType);
                            trainee.NumOfTests++; //update num of tests
                            if (t.DateOfTest < DateTime.Now) //if the test was
                            {
                                trainee.IsAlreadyDidTest = true;
                                trainee.LastTest = t.DateOfTest;
                            }
                            instance.UpdateTraineeDetails(CreateDOFromBO.CreateDoTrainee(trainee));
                            //finish update trainee
                        }
                    }
                }
                if (errors != "ERROR!\n")
                    throw new ArgumentOutOfRangeException(errors);
            }
        }
        public void RemoveTest(string id)
        {
            try
            {
                var test = GetTestByID(id);
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            try
            {
                instance.RemoveTest(id);
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
        }
        public void UpdateTest(Test t) /////////
        {
            try
            {
                var test = GetTestByID(t.TestId);
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            try
            {
                instance.UpdateTest(CreateDOFromBO.CreateDOTest(t, t.TestId));
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Get List (BO.Tester) of all testers
        /// </summary>
        /// <returns></returns>
        public List<Tester> GetTestersList()
        {
            var lst = from item in instance.GetTestersList() select new Tester(item);
            foreach (var x in lst)
            {
                var lstTests = from item in GetTestsList() where item.ExTester.Id == x.Id orderby item.DateOfTest select new TesterTest(item);
                x.TestList = (List<TesterTest>)lstTests;
            }
            return (List<Tester>)lst;
        }
        /// <summary>
        /// Get List (BO.Trainee) of all trainees.
        /// </summary>
        /// <returns></returns>
        public List<Trainee> GetTraineeList()
        {
            var lst = from item in instance.GetTraineeList() select new Trainee(item);
            foreach (var x in lst)
            {
                var lstTests = from item in GetTestsList() where item.ExTrainee.Id == x.Id orderby item.DateOfTest select new TraineeTest(item);
                x.TestList = (List<TraineeTest>)lstTests;
            }
            return (List<Trainee>)lst;
        }
        /// <summary>
        /// Get List (BO.Test) of all tests.
        /// </summary>
        /// <returns></returns>
        public List<Test> GetTestsList()
        {
            var lst = from item in instance.GetTestsList() orderby item.DateOfTest select new Test(item);
            foreach (var x in lst)
            {
                x.ExTester = new ExternalTester(GetTesterByID(x.ExTester.Id));
                x.ExTrainee = new ExternalTrainee(GetTraineeByID(x.ExTrainee.Id));
            }
            return (List<Test>)lst;
        }
        /// <summary>
        /// Get BO.Trainee by trainee id. can throw KeyNotFoundException
        /// </summary>
        /// <param name="trainee id"></param>
        /// <returns></returns>
        public Trainee GetTraineeByID(string id)
        {
            if (!(GetTraineeList().Exists(x => x.Id == id)))
                throw new KeyNotFoundException("Trainee id not exist on system.\n");
            else
            {
                return new Trainee(GetTraineeList().Find(x => x.Id == id));
            }
        }
        /// <summary>
        /// Get BO.Tester by tester id. can throw KeyNotFoundException
        /// </summary>
        /// <param name="tester id"></param>
        /// <returns></returns>
        public Tester GetTesterByID(string id)
        {
            if (!(GetTestersList().Exists(x => x.Id == id)))
                throw new KeyNotFoundException("Tester id not exist on system.\n");
            else
            {
                return new Tester(GetTestersList().Find(x => x.Id == id));
            }
        }
        /// <summary>
        /// get BO.Test obj by serial number of the test. can throw KeyNotFoundException
        /// </summary>
        /// <param name="test serial number"></param>
        /// <returns></returns>
        public Test GetTestByID(string id)
        {
            if (!(GetTestsList().Exists(x => x.TestId == id)))
                throw new KeyNotFoundException("Test's serial number not exist on system.\n");
            else
            {
                return new Test(GetTestsList().Find(x => x.TestId == id));
            }
        }
















        Dictionary<string, Object> keyValuesBL = instance.GetConfig();
        public IEnumerable<Tester> GetAvailableTeacher(DateTime time, int hour, int range = 0)
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
