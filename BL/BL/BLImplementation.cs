using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DL;
namespace BL
{
    public class BLImplementation : IBL
    {
        /// <summary>
        /// static variable of DL
        /// </summary>
        private static IDAL instance = null;
        private AllConfiguretion allConfiguretion;
        /// <summary>
        /// default ctor. initialize the instance of DL
        /// </summary>
        public BLImplementation()
        {
            try
            {
                instance = DL.Factory.GetDLObj("lists");
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
        public void RemoveTester(string id)
        {
            bool exist;
            try
            {
                exist = GetTestersList().Exists(x => x.Id == id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("Internal Error. Can't Remove tester with id " + id + ex.Message);
            }
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
                        if (item.ExTester.Id == id)
                            AbortTest(item.TestId);
                    }
                    instance.RemoveTesterSchedule(id);
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
            }
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
                throw new KeyNotFoundException("Can't remove this trainee becauze he is not on the system.");
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
                //if (tester.MaxTestsPerWeek + 1 > tester.GetNumOfTestForSpecificWeek(t.DateOfTest)) //if the tester already did maximun tests in the new test week
                //    errors += "Tester allready have maximum tests for this week.\n";
                if (trainee.ExistingLicenses.Exists(x => x == t.CarType)) //if trainee already have license on the test type car
                    errors += "Trainee already have that kind of license.\n";
                //if (trainee.CurrCarType != tester.TypeCarToTest) //if the tester car type to test different from current trainee car type
                //    errors += "Type car of tester to test different than the needed test.\n";

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
        public void UpdateTest(string serial, TestResult t) /////////
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
            test.UpdateTestDeteils(t);
            try
            {
                instance.UpdateTestDetails(Converters.CreateDOTest(test, serial));
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
            List<Tester> lst = new List<Tester>();
            foreach (var item in instance.GetTestersList())
            {
                Tester temp = Converters.GetBOTester(item);
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
                var lstTests = from item in instance.GetTestsList() where item.TesterId == x.Id orderby item.DateOfTest select new TesterTest(item);
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
                foreach (var test in GetTestsPartialListByPredicate(x => x.ExTrainee.Id == temp.Id))
                {
                    if (test.DateOfTest > temp.LastTest)
                        temp.LastTest = test.DateOfTest;
                    if (test.IsPassed)
                        temp.ExistingLicenses.Add((CarTypeEnum)test.CarType);
                    temp.TestList.Add(new TraineeTest(test));
                }
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
                lst.Add(new Test(item));
            //var lst = from item in instance.GetTestsList() orderby item.DateOfTest select new Test(item);
            foreach (var x in lst)
            {
                x.ExTester = new ExternalTester(GetTesterByID(x.ExTester.Id));
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
                foreach (var item in instance.GetTestsList())
                {
                    if (item.TraineeId == trainee.Id)
                    {
                        trainee.TestList.Add(new TraineeTest(new Test(item)));
                        if (item.DateOfTest > trainee.LastTest)
                            trainee.LastTest = item.DateOfTest;
                        if (item.IsPassed)
                            trainee.ExistingLicenses.Add((CarTypeEnum)item.CarType);
                    }
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
                Tester tester = new Tester(instance.GetTestersList().Find(x => x.Id == id));
                foreach (var item in instance.GetTestsList())
                {
                    if (item.TesterId == tester.Id)
                    {
                        tester.TestList.Add(new TesterTest(new Test(item)));
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
                Test test = new Test(instance.GetTestsList().Find(x => x.TestId == id));
                test.ExTester = new ExternalTester(GetTesterByID(test.ExTester.Id));
                test.ExTrainee = new ExternalTrainee(GetTraineeByID(test.ExTrainee.Id));
                return test;
            }
        }

        public List<Test> GetOptionalTestsByHour(Test dataSourse, Trainee trainee)
        {
            List<Tester> optionalTesters = GetTestersPartialListByPredicate(x => x.TypeCarToTest == dataSourse.CarType);
            //if there is no tester availiable for this hour
            //-------------------------------------------------------------------
            bool isExistTesterForThisHour = false;
            foreach (var item in optionalTesters)
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (item.AvailiableWorkTime[i, dataSourse.HourOfTest - 9])
                    {
                        isExistTesterForThisHour = true;
                        break;
                    }
                }
            }
            if (!isExistTesterForThisHour)
                return new List<Test>();
            //---------------------------------------------------------------------
            List<Test> optionalTests = new List<Test>();
            int counter = 0, loopNumber = 0;
            //Get previous dates with same hour
            while (counter < 3 && dataSourse.DateOfTest.AddDays(-(loopNumber + 1)) > DateTime.Now.AddDays(1))
            {
                DateTime temp = dataSourse.DateOfTest.AddDays(-(loopNumber + 1));
                if (temp.DayOfWeek != DayOfWeek.Friday && temp.DayOfWeek != DayOfWeek.Saturday)
                    foreach (var item in optionalTesters)
                    {
                        if (item.IsAvailiableOnDateAndHour(temp, dataSourse.HourOfTest) && !optionalTests.Exists(x => x.DateOfTest == temp))
                        {
                            Test test = new Test(dataSourse);
                            test.DateOfTest = temp;
                            test.ExTester = new ExternalTester(item);
                            test.ExTrainee = new ExternalTrainee(trainee);
                            optionalTests.Add(test);
                            counter++;
                        }
                    }
                loopNumber++;
            }
            loopNumber = 0;
            //get older dates with same hour
            while (counter < 7)
            {
                DateTime temp = dataSourse.DateOfTest.AddDays(loopNumber);
                if (temp.DayOfWeek != DayOfWeek.Friday && temp.DayOfWeek != DayOfWeek.Saturday)
                    foreach (var item in optionalTesters)
                    {
                        if (item.IsAvailiableOnDateAndHour(temp, dataSourse.HourOfTest) && !optionalTests.Exists(x => x.DateOfTest == temp))
                        {
                            Test test = new Test(dataSourse);
                            test.DateOfTest = temp;
                            test.ExTester = new ExternalTester(item);
                            test.ExTrainee = new ExternalTrainee(trainee);
                            optionalTests.Add(test);
                            counter++;
                        }
                    }
                loopNumber++;
            }
            return optionalTests;
        }
        public List<Test> GetOptionalTestsByDate(Test dataSourse, Trainee trainee)
        {
            bool[] tmp = new bool[6]; //tmp array for delete dublicates.
            for (int i = 0; i < 6; ++i)
                tmp[i] = false;
            var testersList = GetAvailableTestersForSpecificDay(dataSourse.DateOfTest, dataSourse.HourOfTest,
                trainee.CurrCarType); //get testers list that have unlist one hour availiable on this date
            List<Test> optionalTests = new List<Test>();
            foreach (var item in testersList)
            {
                List<int> currHour = item.GetClosetHour(dataSourse.DateOfTest, dataSourse.HourOfTest);
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
                bool isAllHouersFullOnArray = true;
                for (int i = 0; i < 6; ++i)
                {
                    if (tmp[i])
                    {
                        isAllHouersFullOnArray = false;
                        break;
                    }
                }
                if (isAllHouersFullOnArray)
                    break;
            }
            optionalTests.Sort((x, y) => x.HourOfTest.CompareTo(y.HourOfTest));
            return optionalTests;
        }

        /// <summary>
        /// Get list of testers that availiable on specific date, sorted distance from original hour
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hour"></param>
        /// <param name="carType"></param>
        /// <returns></returns>
        public List<Tester> GetAvailableTestersForSpecificDay(DateTime time, int hour, CarTypeEnum carType)
        {
            var availableTesters = from item in GetTestersList()
                                   where (item.TypeCarToTest == carType && item.IsAvailiableOnDate(time))
                                   //where distance is OK
                                   //orderby Math.Abs(item.GetClosetHour(time, hour))
                                   select item;
            return availableTesters.ToList();
        }
        public List<Test> GetTestsPartialListByPredicate(Func<BO.Test, bool> func)
        {
            return (from item in GetTestsList() where func(item) select new Test(item)).ToList();
        }

        public List<Tester> GetTestersPartialListByPredicate(Func<BO.Tester, bool> func)
        {
            return (from item in GetTestersList() where func(item) select new Tester(item)).ToList();
        }
        public List<Trainee> GetTraineesPartialListByPredicate(Func<BO.Trainee, bool> func)
        {
            return (from item in GetTraineeList() where func(item) select new Trainee(item)).ToList();
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
