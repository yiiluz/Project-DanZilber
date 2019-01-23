using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public class BLImplementation : IBL
    {
        /// <summary>
        /// static variable of DL
        /// </summary>
        private static DO.IDAL instance = null;
        private AllConfiguretion allConfiguretion;
        /// <summary>
        /// default ctor. initialize the instance of DL
        /// </summary>
        public BLImplementation()
        {
            try
            {
                instance = DO.Factory.GetDLObj("xml");
                allConfiguretion = AllConfiguretion.ConfigurationFactory();
            }
            catch (NotImplementedException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Add tester
        /// </summary>
        /// <param name="tester"></param>
        public void AddTester(Tester t)
        {
            int minAge, maxAge;
            try
            {
                minAge = (int)allConfiguretion.GetConfiguretion("Tester minimum age");
                maxAge = (int)allConfiguretion.GetConfiguretion("Tester maximum age");
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
            else
            {
                try
                {
                    instance.AddTester(Converters.CreateDOTester(t));
                    instance.AddTesterSchedule(t.Id, t.AvailiableWorkTime);
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
            }
        }
        /// <summary>
        /// Remove tester
        /// </summary>
        /// <param name="tester id"></param>
        public List<TesterTest> RemoveTester(string id)
        {
            bool exist;
            List<TesterTest> abortedTests = new List<TesterTest>();
            exist = instance.GetTestersList().Exists(x => x.Id == id);
            if (!exist)
            {
                throw new KeyNotFoundException("Can't remove this tester becauze he is not on the system.");
            }
            else
            {
                try
                {
                    instance.RemoveTester(id);
                    foreach (var item in GetTestsList())
                    {
                        if (item.ExTester.Id == id && item.DateOfTest > DateTime.Now)
                        {
                            AbortTest(item.TestId);
                            abortedTests.Add(new TesterTest(item));
                        }

                    }
                    instance.RemoveTesterSchedule(id);
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
            }
            return abortedTests;
        }
        /// <summary>
        /// Update tester
        /// </summary>
        /// <param name="tester"></param>
        public void UpdateTesterDetails(Tester t)
        {
            bool exist;
            try
            {
                exist = GetTestersList().Exists(x => x.Id == t.Id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("Internal Error. Can't Update tester Deteails with id " + t.Id + ex.Message);
            }
            if (!exist)
                throw new KeyNotFoundException("Can't update this tester becauze he is not on the system.");
            else
            {
                int minAge, maxAge;
                try
                {
                    minAge = (int)allConfiguretion.GetConfiguretion("Tester minimum age");
                    maxAge = (int)allConfiguretion.GetConfiguretion("Tester maximum age");
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
                    instance.UpdateTesterDetails(Converters.CreateDOTester(t));
                    instance.UpdateTesterSchedule(t.Id, t.AvailiableWorkTime);
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
            }

        }

        /// <summary>
        /// Add trainee
        /// </summary>
        /// <param name="trainee"></param>
        public void AddTrainee(Trainee t)
        {
            int minAge;
            try
            {
                minAge = (int)allConfiguretion.GetConfiguretion("Trainee minimum age");
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
                    instance.AddTrainee(Converters.CreateDoTrainee(t));
                }
                catch (DuplicateWaitObjectException e)
                {
                    throw e;
                }
        }
        /// <summary>
        /// Remove trainee
        /// </summary>
        /// <param name="trainee id"></param>
        public void RemoveTrainee(string id)
        {

            Trainee trainee;
            try
            {
                trainee = GetTraineeByID(id);
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException("Can't remove this trainee because he is not on the system.");
            }
            foreach (var item in GetTestsList())
                if (item.ExTrainee.Id == trainee.Id)
                    AbortTest(item.TestId);
            try
            {
                instance.RemoveTrainee(id);
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }

        }
        /// <summary>
        /// Update trainee
        /// </summary>
        /// <param name="t"></param>
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
                    minAge = (int)allConfiguretion.GetConfiguretion("Trainee minimum age");
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
                    instance.UpdateTraineeDetails(Converters.CreateDoTrainee(t));
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Add test. returns Test Serial number
        /// </summary>
        /// <param name="test"></param>
        public string AddTest(Test t)
        {
            t.IsTesterUpdateStatus = false;
            string testId = "";
            string errors = "ERROR!\n";
            //check if trainee & tester already on system.
            bool traineeExist = GetTraineeList().Exists(x => x.Id == t.ExTrainee.Id);///////////////////////////////////////
            bool testerExist = false;
            try
            {
                testerExist = GetTestersList().Exists(x => x.Id == t.ExTester.Id);
            }
            catch (KeyNotFoundException ex)
            {
                errors += ex.Message;
            }
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
                //find last test object
                Test lastTest;
                if (trainee.LastTest != DateTime.MinValue)
                {
                    lastTest = GetTestsList().Find(x => x.IsTestAborted == false && x.DateOfTest == trainee.LastTest
                                    && x.ExTrainee.Id == trainee.Id && x.CarType == tester.TypeCarToTest);
                    if (lastTest != null && !lastTest.IsTesterUpdateStatus)
                        errors += "Can't add to this trainee new test until the results of the test on " + trainee.LastTest.ToShortDateString()
                            + " will be availiable.\n";
                }
                else
                {
                    int minDaysBetweenTests = -1;
                    try
                    {
                        minDaysBetweenTests = (int)allConfiguretion.GetConfiguretion("Minimum days between tests");
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
                            if (t.CarType == item.CarType && t.IsTestAborted == false
                                && Math.Abs((t.DateOfTest - item.DateOfTest).Days) < minDaysBetweenTests)
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
                    minLesson = (int)allConfiguretion.GetConfiguretion("Minimum lessons");
                }
                catch (KeyNotFoundException e)
                {
                    errors += (e.Message + "\n");
                }
                if (minLesson != -1 && trainee.NumOfFinishedLessons < minLesson) //if trainee didnt did enough lessons
                {
                    errors += "Trainee did not passed enough lessons for test.\n";
                }
                if (trainee.ExistingLicenses.Exists(x => (x == t.CarType) || (x == CarTypeEnum.PrivateCar && t.CarType == CarTypeEnum.PrivateCarAuto))) //if trainee already have license on the test type car
                    errors += "Trainee already have that kind of license.\n";
                if (errors == "ERROR!\n") //if there was no errors
                {
                    //
                    int serial = -1;
                    try
                    {
                        serial = (int)allConfiguretion.GetConfiguretion("Serial Number Test"); //get the serial number of the test
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
                            allConfiguretion.UpdateSerialNumber();
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
                                instance.AddTest(Converters.CreateDOTest(t, Convert.ToString(serial))); //add the test
                                testId = serial.ToString();
                            }
                            catch (DuplicateWaitObjectException e)
                            {
                                errors += (e.Message + "\n");
                            }
                            catch (KeyNotFoundException e)
                            {
                                errors += (e.Message + "\n");
                            }
                            //update trainee deteils
                            if (t.DateOfTest < DateTime.Now) //if the test was
                            {
                                if (!trainee.IsAlreadyDidTest)
                                {
                                    trainee.IsAlreadyDidTest = true;
                                    try
                                    {
                                        UpdateTraineeDetails(trainee);
                                    }
                                    catch (KeyNotFoundException e)
                                    {
                                        throw new MemberAccessException(e.Message + "\nCan't update trainee.");
                                    }
                                }
                            }
                            //finish update trainee
                        }
                    }
                }
                if (errors != "ERROR!\n")
                {
                    throw new ArgumentOutOfRangeException(errors);
                }
            }
            return testId;
        }
        /// <summary>
        /// Remove test
        /// </summary>
        /// <param name="test id"></param>
        public void AbortTest(string id)
        {
            Test test;
            try
            {
                test = GetTestByID(id);
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            try
            {
                test.IsTestAborted = true;
                instance.UpdateTestDetails(Converters.CreateDOTest(test, test.TestId));
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Update test deteils
        /// </summary>
        /// <param name="test"></param>
        public void UpdateTestResult(string serial, TestResult t) /////////
        {

            Test test;
            //get test obj
            try
            {
                test = GetTestByID(serial);
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            Trainee trainee;
            //get trainee obj
            try
            {
                trainee = GetTraineeByID(test.ExTrainee.Id);
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            string errorList = "ERROR!\n";
            if (DateTime.Now == test.DateOfTest && DateTime.Now.Hour < test.HourOfTest || DateTime.Now < test.DateOfTest)
                errorList += "You can not update test information before the intended date. \n";
            if (test.IsTesterUpdateStatus)
                errorList += "Test results have already been entered. You can not change the test details. \n";
            if (test.IsTestAborted)
                errorList += "The test has been canceled. details can not be updated for this test \n";
            if (errorList == "ERROR!\n")
            {

                UpdateTestDeteils(test, t);
                try
                {
                    instance.UpdateTestDetails(Converters.CreateDOTest(test, serial));
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
            }
            else
                throw new AccessViolationException(errorList);
        }
        /// <summary>
        /// Get List (BO.Tester) of all testers
        /// </summary>
        /// <returns></returns>

        public List<Tester> GetTestersList()
        {
            List<Tester> lst = new List<Tester>();
            foreach (var item in instance.GetTestersList())
            {
                Tester temp = Converters.CreateBOTester(item);
                try
                {
                    temp.AvailiableWorkTime = instance.GetTesterSchedule(temp.Id);
                }
                catch (KeyNotFoundException ex)
                {
                    throw new KeyNotFoundException("Internal Error. Can't import Testers list. " + ex.Message);
                }
                lst.Add(temp);
            }
            //var lst = from item in instance.GetTestersList() select CreateDOFromBO.GetBOTester(item);
            foreach (var x in lst)
            {
                var lstTests = from item in instance.GetTestsList()
                               where item.TesterId == x.Id
                               orderby item.DateOfTest
                               select new TesterTest(Converters.CreateBOTest(item));
                x.TestList = lstTests.ToList();
            }
            return lst;
        }
        /// <summary>
        /// Get List (BO.Trainee) of all trainees.
        /// </summary>
        /// <returns></returns>
        public List<Trainee> GetTraineeList()
        {
            List<Trainee> lst = new List<Trainee>();
            foreach (var item in instance.GetTraineeList())
            {

                Trainee temp = Converters.CreateBOTrainee(item);
                TraineeStatistics statistics = new TraineeStatistics();
                foreach (var test in GetTestsPartialListByPredicate(x => x.ExTrainee.Id == temp.Id))
                {
                    statistics.NumOfTests++;
                    if (test.DateOfTest > temp.LastTest)
                        temp.LastTest = test.DateOfTest;
                    if (test.IsTesterUpdateStatus)
                        if (test.IsPassed)
                        {
                            temp.ExistingLicenses.Add((CarTypeEnum)test.CarType);
                            statistics.SuccessTests++;
                        }
                        else
                            statistics.FailedTests++;
                    temp.TestList.Add(new TraineeTest(test));
                }
                temp.Statistics = statistics;
                lst.Add(temp);
            }
            return lst;
        }
        /// <summary>
        /// Get List (BO.Test) of all tests.
        /// </summary>
        /// <returns></returns>
        public List<Test> GetTestsList()
        {
            List<Test> lst = new List<Test>();
            foreach (var item in instance.GetTestsList())
                lst.Add(Converters.CreateBOTest(item));
            //var lst = from item in instance.GetTestsList() orderby item.DateOfTest select new Test(item);
            foreach (var x in lst)
            {
                if (instance.GetTestersList().Exists(t => t.Id == x.ExTester.Id)) //if tester hasnt deleted
                    x.ExTester = new ExternalTester(GetTesterByID(x.ExTester.Id));
                if (instance.GetTraineeList().Exists(t => t.Id == x.ExTrainee.Id))
                    x.ExTrainee = new ExternalTrainee(GetTraineeByID(x.ExTrainee.Id));
            }
            return lst;
        }

        /// <summary>
        /// Get BO.Trainee by trainee id. can throw KeyNotFoundException
        /// </summary>
        /// <param name="trainee id"></param>
        /// <returns></returns>
        public Trainee GetTraineeByID(string id)
        {
            if (!(instance.GetTraineeList().Exists(x => x.Id == id)))
                throw new KeyNotFoundException("Trainee id not exist on system.\n");
            else
            {
                Trainee trainee = Converters.CreateBOTrainee(instance.GetTraineeList().Find(x => x.Id == id));
                TraineeStatistics statistics = new TraineeStatistics();
                foreach (var item in instance.GetTestsList())
                {
                    if (item.TraineeId == trainee.Id)
                    {
                        statistics.NumOfTests++;
                        if (item.DateOfTest > trainee.LastTest)
                            trainee.LastTest = item.DateOfTest;
                        if (item.IsTesterUpdateStatus)
                            if (item.IsPassed)
                            {
                                trainee.ExistingLicenses.Add((CarTypeEnum)item.CarType);
                                statistics.SuccessTests++;
                            }
                            else
                                statistics.FailedTests++;
                        trainee.TestList.Add(new TraineeTest(Converters.CreateBOTest(item)));
                    }
                    trainee.Statistics = statistics;
                }
                return trainee;
            }
        }
        /// <summary>
        /// Get BO.Tester by tester id. can throw KeyNotFoundException
        /// </summary>
        /// <param name="tester id"></param>
        /// <returns></returns>
        public Tester GetTesterByID(string id)
        {
            if (!(instance.GetTestersList().Exists(x => x.Id == id)))
                throw new KeyNotFoundException("Tester id not exist on system.\n");
            else
            {
                Tester tester = Converters.CreateBOTester(instance.GetTestersList().Find(x => x.Id == id));
                foreach (var item in instance.GetTestsList())
                {
                    if (item.TesterId == tester.Id)
                    {
                        tester.TestList.Add(new TesterTest(Converters.CreateBOTest(item)));
                    }
                }
                try
                {
                    tester.AvailiableWorkTime = instance.GetTesterSchedule(tester.Id);
                }
                catch (KeyNotFoundException ex)
                {
                    throw new KeyNotFoundException("Tester schedule can not be imported. " + ex.Message);
                }
                return tester;
            }
        }
        /// <summary>
        /// get BO.Test obj by serial number of the test. can throw KeyNotFoundException
        /// </summary>
        /// <param name="test serial number"></param>
        /// <returns></returns>
        public Test GetTestByID(string id)
        {
            if (!(instance.GetTestsList().Exists(x => x.TestId == id)))
                throw new KeyNotFoundException("Test's serial number not exist on system.\n");
            else
            {
                Test test = Converters.CreateBOTest(instance.GetTestsList().Find(x => x.TestId == id));
                if (instance.GetTestersList().Exists(x => x.Id == test.ExTester.Id))
                    test.ExTester = new ExternalTester(GetTesterByID(test.ExTester.Id));
                if (instance.GetTraineeList().Exists(x => x.Id == test.ExTrainee.Id))
                    test.ExTrainee = new ExternalTrainee(GetTraineeByID(test.ExTrainee.Id));
                return test;
            }
        }

        public List<Test> GetOptionalTestsByHour(Test dataSourse, Trainee trainee)
        {
            List<Test> optionalTests = new List<Test>();
            List<Tester> optionalTesters = GetTestersPartialListByPredicate(x => x.TypeCarToTest == dataSourse.CarType &&
                                                IsTestersWorkAtSpesificHour(x, dataSourse.HourOfTest));
            //if there is no tester availiable for this hour
            if (optionalTesters.Count == 0)
                return optionalTests;
            int counter = 0, loopNumber = 0;
            while (counter < 7)
            {
                DateTime temp = dataSourse.DateOfTest.AddDays(-(loopNumber + 1)), temp2 = dataSourse.DateOfTest.AddDays(loopNumber);
                foreach (var item in optionalTesters)
                {
                    if (temp > DateTime.Now.AddDays(1) && temp.DayOfWeek != DayOfWeek.Friday && temp.DayOfWeek != DayOfWeek.Saturday)
                        if (!optionalTests.Exists(x => x.DateOfTest == temp) && IsTesterAvailiableOnDateAndHour(item, temp, dataSourse.HourOfTest))
                        {
                            Test test = new Test(dataSourse);
                            test.DateOfTest = temp;
                            test.ExTester = new ExternalTester(item);
                            test.ExTrainee = new ExternalTrainee(trainee);
                            optionalTests.Add(test);
                            counter++;
                        }
                    if (temp2.DayOfWeek != DayOfWeek.Friday && temp2.DayOfWeek != DayOfWeek.Saturday)
                        if (!optionalTests.Exists(x => x.DateOfTest == temp2) && IsTesterAvailiableOnDateAndHour(item, temp2, dataSourse.HourOfTest))
                        {
                            Test test = new Test(dataSourse);
                            test.DateOfTest = temp2;
                            test.ExTester = new ExternalTester(item);
                            test.ExTrainee = new ExternalTrainee(trainee);
                            optionalTests.Add(test);
                            counter++;
                        }
                }
                loopNumber++;
            }
            optionalTests.Sort((x, y) => x.DateOfTest.CompareTo(y.DateOfTest));
            return optionalTests;
        }
        public List<Test> GetOptionalTestsByDate(Test dataSourse, Trainee trainee)
        {
            List<Test> optionalTests = new List<Test>();
            List<Tester> testersList = GetTestersPartialListByPredicate(x => x.TypeCarToTest == dataSourse.CarType &&
                                            IsTesterAvailiableOnDateAndHour(x, dataSourse.DateOfTest) &&
                                            GetAvailiableHoursOfTesterForSpesificDate(x, dataSourse.DateOfTest).Count != 0);
            bool[] tmp = new bool[6]; //tmp array for delete dublicates.
            for (int i = 0; i < 6; ++i)
                tmp[i] = false;

            foreach (var item in testersList)
            {
                List<int> currHour = GetAvailiableHoursOfTesterForSpesificDate(item, dataSourse.DateOfTest);
                foreach (var item1 in currHour)
                {
                    if (tmp[item1 - 9] == false)
                    {
                        tmp[item1 - 9] = true;
                        Test test = new Test(dataSourse);
                        test.ExTester = new ExternalTester(item);
                        test.ExTrainee = new ExternalTrainee(trainee);
                        test.HourOfTest = item1;
                        optionalTests.Add(test);
                    }
                }
                if (!tmp.Any(x => !x))
                    break;
            }
            optionalTests.Sort((x, y) => x.HourOfTest.CompareTo(y.HourOfTest));
            return optionalTests;
        }

        public List<Test> GetTestsPartialListByPredicate(Func<BO.Test, bool> func)
        {
            return (from item in GetTestsList() where func(item) orderby item.DateOfTest, item.HourOfTest select item).ToList();
        }
        public List<Tester> GetTestersPartialListByPredicate(Func<BO.Tester, bool> func)
        {
            return (from item in GetTestersList() where func(item) orderby item.LastName, item.FirstName select item).ToList();
        }
        public List<Trainee> GetTraineesPartialListByPredicate(Func<BO.Trainee, bool> func)
        {
            return (from item in GetTraineeList() where func(item) orderby item.LastName, item.FirstName select item).ToList();
        }



        public string GetStringOfTraineeLicenses(string id)
        {
            string existing = "";
            Trainee trainee;
            try
            {
                trainee = GetTraineeByID(id);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            foreach (var item in trainee.TestList)
            {
                if (item.IsPassed == true && item.IsTesterUpdateStatus == true && !item.IsTestAborted)
                    existing += item.CarType + "\n";
            }
            return existing;
        }
        public int GetTraineeNumTestedTest(string id)
        {
            Trainee trainee;
            try
            {
                trainee = GetTraineeByID(id);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            int num = 0;
            foreach (var item in trainee.TestList)
            {
                if (item.DateOfTest < DateTime.Now)
                    num++;
            }
            return num;
        }

        public IEnumerable<IGrouping<string, Tester>> GetTestersGroupedByCity()
        {
            return from item in GetTestersList()
                   orderby item.LastName, item.FirstName
                   group item by item.City
                        into g
                   orderby g.Key
                   select g;
        }
        public IEnumerable<IGrouping<int, Tester>> GetTestersGropedBySeniority()
        {
            return from item in GetTestersList()
                   orderby item.LastName, item.FirstName
                   group item by item.Seniority
                        into g
                   orderby g.Key
                   select g;
        }
        public IEnumerable<IGrouping<CarTypeEnum, Tester>> GetTestersGrupedBySpecialization()
        {
            return from item in GetTestersList()
                   orderby item.LastName, item.FirstName
                   group item by item.TypeCarToTest
                   into g
                   orderby g.Key
                   select g;
        }
        public IEnumerable<IGrouping<int, Tester>> GetTestersGropedByMaxDistance()
        {
            return from item in GetTestersList()
                   orderby item.LastName, item.FirstName
                   group item by item.MaxDistance
                        into g
                   orderby g.Key
                   select g;
        }

        public IEnumerable<IGrouping<string, Trainee>> GetTraineesGroupsBySchool()
        {
            return from item in GetTraineeList()
                   orderby item.LastName, item.FirstName
                   group item by item.SchoolName
                   into g
                   orderby g.Key
                   select g;
        }
        public IEnumerable<IGrouping<string, Trainee>> GetTraineesGroupsByTeacher()
        {
            return from item in GetTraineeList()
                   orderby item.LastName, item.FirstName
                   group item by item.SchoolName
                   into g
                   orderby g.Key
                   select g;
        }
        public IEnumerable<IGrouping<int, Trainee>> GetTraineesGroupedByNumOfTests()
        {
            return from item in GetTraineeList()
                   orderby item.LastName, item.FirstName
                   group item by item.NumOfTests
                   into g
                   orderby g.Key
                   select g;
        }
        public IEnumerable<IGrouping<string, Trainee>> GetTraineessGroupedByCity()
        {
            return from item in GetTraineeList()
                   orderby item.LastName, item.FirstName
                   group item by item.City
                   into g
                   orderby g.Key
                   select g;
        }


        private int GetTesterNumOfTestForDateWeek(Tester tester, DateTime a)
        {
            int num = 0;
            foreach (var item in tester.TestList)
                if (item.DateOfTest.AddDays(-(int)item.DateOfTest.DayOfWeek) == a.AddDays(-(int)a.DayOfWeek) && !item.IsTestAborted)
                    num++;
            return num;
        }
        private List<int> GetAvailiableHoursOfTesterForSpesificDate(Tester tester, DateTime date)
        {
            List<int> AvailiableHours = new List<int>();
            if (!IsTesterAvailiableOnDateAndHour(tester, date))
                return AvailiableHours;
            bool[] temp = new bool[6];
            //set matrix of bool that contain the available times of the date week
            for (int i = 0; i < 6; ++i)
                temp[i] = tester.AvailiableWorkTime[(int)date.DayOfWeek, i];
            foreach (var x in tester.TestList)
            {
                if (x.DateOfTest == date && !x.IsTestAborted)
                    temp[x.HourOfTest - 9] = false;
            }
            for (int i = 0; i < 6; ++i)
            {
                if (temp[i])
                    AvailiableHours.Add(i + 9);
            }
            AvailiableHours.Sort();
            return AvailiableHours;//if day is full
        }
        private bool IsTesterAvailiableOnDateAndHour(Tester tester, DateTime date, int hour = -1)
        {
            if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday || GetTesterNumOfTestForDateWeek(tester, date) + 1 > tester.MaxTestsPerWeek)
                return false;
            bool isWorkUnlistOneHourOnThisDate = false;
            bool[] tmp = new bool[6];
            for (int i = 0; i < 6; ++i)
            {
                tmp[i] = tester.AvailiableWorkTime[(int)date.DayOfWeek, i];
                if (tmp[i])
                    isWorkUnlistOneHourOnThisDate = true;
            }
            if (isWorkUnlistOneHourOnThisDate)
            {
                foreach (var item in tester.TestList)
                {
                    if (item.DateOfTest == date && !item.IsTestAborted)
                    {
                        tmp[item.HourOfTest - 9] = false;
                    }
                }
                if (hour == -1) //if try to select by date
                {
                    for (int i = 0; i < 6; ++i) //if after filtering there is availiable hour
                    {
                        if (tmp[i])
                            return true;
                    }
                }
                else
                    return tmp[hour - 9];
            }
            return false;//if tester cant work at this day and hour
        }
        private bool IsTestersWorkAtSpesificHour(Tester tester, int hour)
        {
            for (int i = 0; i < 5; ++i)
            {
                if (tester.AvailiableWorkTime[i, hour - 9])
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateTestDeteils(Test test, TestResult other)
        {
            test.DistanceKeeping = other.DistanceKeeping;
            test.ReverseParking = other.ReverseParking;
            test.MirrorsCheck = other.MirrorsCheck;
            test.Signals = other.Signals;
            test.CorrectSpeed = other.CorrectSpeed;
            test.IsPassed = other.IsPassed;
            test.TesterNotes = other.TesterNotes;
            test.IsTesterUpdateStatus = true;
        }
    }
}
