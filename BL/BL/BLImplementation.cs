using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using BO;
using System.Threading;
using System.Net;
using System.Xml;

namespace BL
{

    public class BLImplementation : IBL
    {
        Action StatisticsChanged;
        volatile bool isStatisticsChanged = false;

        string typeOfDL = "xml";
        /// <summary>
        /// static variable of DL
        /// </summary>
        private static DO.IDAL instance = null;
        //private static BO.SystemStatistics systemStatistics;
        /// <summary>
        /// default ctor. initialize the instance of DL
        /// </summary>
        public BLImplementation()
        {
            try
            {
                instance = DO.Factory.GetDLObj(typeOfDL);
                DO.Factory.AddConfigUpdatedObserver(ConfigChanged);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            StatisticsChanged += UpdateStatistics;
            new Thread(() =>
            {
                while (true)
                {
                    if (isStatisticsChanged)
                    {
                        isStatisticsChanged = false;
                        StatisticsChanged();
                    }
                    Thread.Sleep(2000);
                }
            }).Start();
        }

        public void AddEventIfConfigChanged(Action action)
        {
            DO.Factory.AddConfigUpdatedObserver(action);
        }

        public static void ConfigChanged()
        {
            Configuretion.UpdateDictonary();
        }

        public Dictionary<String, Object> GetConfig()
        {
            return Configuretion.ConfiguretionsDictionary;
        }

        public void SetConfig(String parm, Object value)
        {
            try
            {
                instance.SetConfig(parm, value);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("שגיאה! לא ניתן לעדכן מאפיין קונפגורציה זה.\n" + ex.Message);
            }
            catch (AccessViolationException ex)
            {
                throw new AccessViolationException("שגיאה! לא ניתן לעדכן מאפיין קונפגורציה זה.\n" + ex.Message);
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
                minAge = (int)Configuretion.ConfiguretionsDictionary["גיל בוחן מינימלי"];
                maxAge = (int)Configuretion.ConfiguretionsDictionary["גיל בוחן מקסימלי"];
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            int testerAge = (DateTime.Now.Year - t.DateOfBirth.Year);
            if (testerAge < minAge || testerAge > maxAge)
            {
                throw new ArgumentOutOfRangeException(" שגיאה! גיל הבוחן לא בטווח המתאים:" + minAge + "-" + maxAge);
            }
            else
            {
                try
                {
                    instance.AddTesterSchedule(t.Id, t.AvailiableWorkTime);
                    instance.AddTester(Converters.CreateDOTester(t));
                    isStatisticsChanged = true;
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
                catch (DirectoryNotFoundException e)
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
            try
            {
                exist = instance.GetTestersList().Exists(x => x.Id == id);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            if (!exist)
            {
                throw new KeyNotFoundException("שגיאה! לא ניתן למחוק בוחן שלא קיים במערכת.");
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
                    isStatisticsChanged = true;
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
                catch (DirectoryNotFoundException e)
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
                throw new KeyNotFoundException("שגיאה פנימית! לא ניתן לעדכן בוחן עם תעודת זהות:" + t.Id + ex.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            if (!exist)
                throw new KeyNotFoundException("שגיאה! לא ניתן לעדכן בוחן זה משום שהוא לא קיים במערכת. ");
            else
            {
                int minAge, maxAge;
                try
                {
                    minAge = (int)Configuretion.ConfiguretionsDictionary["גיל בוחן מינימלי"];
                    maxAge = (int)Configuretion.ConfiguretionsDictionary["גיל בוחן מקסימלי"];
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
                int testerAge = (DateTime.Now.Year - t.DateOfBirth.Year);
                if (testerAge < minAge || testerAge > maxAge)
                {
                    throw new ArgumentOutOfRangeException("שגיאה! גיל הבוחן לא בטווח המתאים:" + minAge + "-" + maxAge);
                }
                try
                {
                    instance.UpdateTesterDetails(Converters.CreateDOTester(t));
                    instance.UpdateTesterSchedule(t.Id, t.AvailiableWorkTime);
                    isStatisticsChanged = true;
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
                catch (DirectoryNotFoundException e)
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
                minAge = (int)Configuretion.ConfiguretionsDictionary["גיל נבחן מינימלי"];
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            int traineeAge = (DateTime.Now.Year - t.DateOfBirth.Year);
            if (traineeAge < minAge)
            {
                throw new ArgumentOutOfRangeException("שגיאה! גיל התלמיד מתחת לגיל המינימום:" + minAge);
            }
            else try
                {
                    instance.AddTrainee(Converters.CreateDoTrainee(t));
                    isStatisticsChanged = true;
                }
                catch (DuplicateWaitObjectException e)
                {
                    throw e;
                }
                catch (DirectoryNotFoundException e)
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
                throw new KeyNotFoundException("שגיאה! לא ניתן למחוק תלמיד שאינו קיים במערכת:");
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            foreach (var item in GetTestsList())
                if (item.ExTrainee.Id == trainee.Id)
                    AbortTest(item.TestId);
            try
            {
                instance.RemoveTrainee(id);
                isStatisticsChanged = true;
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            catch (DirectoryNotFoundException e)
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
                throw new KeyNotFoundException("שגיאה! לא ניתן למחוק תלמיד שאינו קיים במערכת:");
            else
            {
                int minAge;
                try
                {
                    minAge = (int)Configuretion.ConfiguretionsDictionary["גיל נבחן מינימלי"];
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
                int traineeAge = (DateTime.Now.Year - t.DateOfBirth.Year);
                if (traineeAge < minAge)
                {
                    throw new ArgumentOutOfRangeException("שגיאה! גיל התלמיד מתחת לגיל המינימום:" + minAge);
                }
                try
                {
                    instance.UpdateTraineeDetails(Converters.CreateDoTrainee(t));
                    isStatisticsChanged = true;
                }
                catch (KeyNotFoundException e)
                {
                    throw e;
                }
                catch (DirectoryNotFoundException e)
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
            string errors = "";
            //check if trainee & tester already on system.
            bool traineeExist = GetTraineeList().Exists(x => x.Id == t.ExTrainee.Id);///////////////////////////////////////
            bool testerExist = false;
            try
            {
                testerExist = GetTestersList().Exists(x => x.Id == t.ExTester.Id);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            catch (KeyNotFoundException ex)
            {
                errors += ex.Message;
            }
            if (!testerExist || !traineeExist)
            {
                if (!traineeExist)
                    errors += "שגיאה! התלמיד רשום למבחן שלא קיים במערכת. \n";
                if (!testerExist)
                    errors += " שגיאה! הבוחן רשום למבחן שלא קיים במערכת. \n";
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
                if (trainee.LastTest > DateTime.Now)
                {
                    lastTest = GetTestsList().Find(x => x.IsTestAborted == false && x.DateOfTest == trainee.LastTest
                                    && x.ExTrainee.Id == trainee.Id && x.CarType == tester.TypeCarToTest);
                    if (lastTest != null && !lastTest.IsTesterUpdateStatus)
                    {
                        errors += "-שגיאה! לא ניתן להוסיף לתלמיד מבחן עד אשר התוצאה במבחן ב " + trainee.LastTest.ToShortDateString() + " תהיה זמינה.";
                        errors += "\n";
                    }
                }
                else
                {
                    int minDaysBetweenTests = -1;
                    try
                    {
                        minDaysBetweenTests = (int)Configuretion.ConfiguretionsDictionary["מינימום ימים בין מבחנים"];
                    }
                    catch (KeyNotFoundException e)
                    {
                        errors += (e.Message + "\n");
                    }
                    if (minDaysBetweenTests != -1) //if success on bring in the configuration of "Minimum days between tests"
                    {
                        bool flag = false;
                        foreach (var existingTraineeTest in trainee.TestList) //check if the new test too close to other
                        {
                            if (t.CarType == existingTraineeTest.CarType && existingTraineeTest.IsTestAborted == false
                                && Math.Abs((t.DateOfTest - existingTraineeTest.DateOfTest).Days) < minDaysBetweenTests)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                            errors += "שגיאה! לא ניתן להוסיף מבחן. התלמיד ניגש לבחינה לפני פחות מ- " + minDaysBetweenTests + " ימים.\n";
                    }
                }
                int minLesson = -1;
                try
                {
                    minLesson = (int)Configuretion.ConfiguretionsDictionary["מספר שיעורים מינימלי"];
                }
                catch (KeyNotFoundException e)
                {
                    errors += (e.Message + "\n");
                }
                if (minLesson != -1 && trainee.NumOfFinishedLessons < minLesson) //if trainee didnt did enough lessons
                {
                    errors += "שגיאה! לא ניתן להוסיף מבחן. התלמיד לא השלים מספר שיעורים כנדרש.\n";
                }
                if (trainee.ExistingLicenses.Exists(x => (x == t.CarType) || (x == CarTypeEnum.רכב_פרטי && t.CarType == CarTypeEnum.רכב_פרטי_אוטומט))) //if trainee already have license on the test type car
                    errors += "שגיאה! לא ניתן להוסיף מבחן. התלמיד  כבר בעל רישיון מדרגה זו.\n";
                if (errors == "") //if there was no errors
                {
                    //
                    int serial = -1;
                    try
                    {
                        serial = (int)Configuretion.ConfiguretionsDictionary["מספר מבחן"]; //get the serial number of the test
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
                            instance.SetConfig("מספר מבחן", ++serial); //update the test serial number
                            Configuretion.UpdateSerialNumber();
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
                                isStatisticsChanged = true;
                            }
                            catch (DirectoryNotFoundException e)
                            {
                                throw e;
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
                        }
                    }
                }
                if (errors != "")
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
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            try
            {
                test.IsTestAborted = true;
                instance.UpdateTestDetails(Converters.CreateDOTest(test, test.TestId));
                isStatisticsChanged = true;
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
            catch (DirectoryNotFoundException e)
            {
                throw e;
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
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            catch (KeyNotFoundException e)
            {
                throw e;
            }
            string errorList = "!שגיאות\n";
            if (DateTime.Now == test.DateOfTest && DateTime.Now.Hour < test.HourOfTest || DateTime.Now < test.DateOfTest)
                errorList += "שגיאה! לא ניתן לעדכן פרטי מבחן לפני המועד המיועד. \n";
            if (test.IsTesterUpdateStatus)
                errorList += "שגיאה! תוצאת המבחן כבר הוזנה למערכת. לא ניתן לשנות פרטי מבחן. \n";
            if (test.IsTestAborted)
                errorList += "שגיאה! המבחן בוטל. לא ניתן לעדכן פרטים עבור מבחן זה.\n";
            if (errorList == "!שגיאות\n")
            {

                UpdateTestDeteils(test, t);
                try
                {
                    instance.UpdateTestDetails(Converters.CreateDOTest(test, serial));
                    isStatisticsChanged = true;
                }
                catch (DirectoryNotFoundException e)
                {
                    throw e;
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
                catch (DirectoryNotFoundException e)
                {
                    throw e;
                }
                catch (KeyNotFoundException ex)
                {
                    throw new KeyNotFoundException(" שגיאה!" + ex.Message);
                }
                lst.Add(temp);
            }
            //var lst = from item in instance.GetTestersList() select CreateDOFromBO.GetBOTester(item);
            foreach (var x in lst)
            {
                try
                {
                    var lstTests = from item in instance.GetTestsList()
                                   where item.TesterId == x.Id
                                   orderby item.DateOfTest
                                   select new TesterTest(Converters.CreateBOTest(item));
                    x.TestList = lstTests.ToList();
                    TesterStatistics statistics = new TesterStatistics();
                    foreach (var test in lstTests)
                    {
                        if (test.IsTesterUpdateStatus)
                            if (test.IsPassed)
                            {
                                statistics.SuccessTests++;
                            }
                            else
                                statistics.FailedTests++;
                        if (test.DateOfTest > DateTime.Now)
                            statistics.FutureTests++;
                        statistics.NumOfTests++;
                    }

                }
                catch (DirectoryNotFoundException e)
                {
                    throw e;
                }
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
            try
            {
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

            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
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
            try
            {
                if (!(instance.GetTraineeList().Exists(x => x.Id == id)))
                    throw new KeyNotFoundException("שגיאה!  לא קיים במערכת תלמיד עם תעודת זהות זו.");
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
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Get BO.Tester by tester id. can throw KeyNotFoundException
        /// </summary>
        /// <param name="tester id"></param>
        /// <returns></returns>
        public Tester GetTesterByID(string id)
        {
            bool exists;
            try
            {
                exists = instance.GetTestersList().Exists(x => x.Id == id);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            if (!exists)
                throw new KeyNotFoundException("שגיאה!  לא קיים במערכת בוחן עם תעודת זהות זו.\n");
            else
            {
                Tester tester = Converters.CreateBOTester(instance.GetTestersList().Find(x => x.Id == id));
                tester.Statistics = new TesterStatistics();
                foreach (var test in instance.GetTestsList())
                {
                    if (test.TesterId == tester.Id)
                    {
                        tester.TestList.Add(new TesterTest(Converters.CreateBOTest(test)));
                        if (test.IsTesterUpdateStatus)
                            if (test.IsPassed)
                            {
                                tester.Statistics.SuccessTests++;
                            }
                            else
                                tester.Statistics.FailedTests++;
                        if (test.DateOfTest > DateTime.Now || (test.DateOfTest.Date == DateTime.Now.Date && test.HourOfTest > DateTime.Now.Hour))
                            tester.Statistics.FutureTests++;
                        tester.Statistics.NumOfTests++;
                    }
                }
                try
                {
                    tester.AvailiableWorkTime = instance.GetTesterSchedule(tester.Id);
                }
                catch (DirectoryNotFoundException e)
                {
                    throw e;
                }
                catch (KeyNotFoundException ex)
                {
                    throw new KeyNotFoundException(ex.Message + " שגיאה פנימית לא ניתן להטעין מערכת שעות של הבוחן.\n");
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
            Test test;
            try
            {
                if (!(instance.GetTestsList().Exists(x => x.TestId == id)))
                    throw new KeyNotFoundException("שגיאה! מספר מבחן לא קיים במערכת.\n");
                else
                {
                    test = Converters.CreateBOTest(instance.GetTestsList().Find(x => x.TestId == id));
                    if (instance.GetTestersList().Exists(x => x.Id == test.ExTester.Id))
                        test.ExTester = new ExternalTester(GetTesterByID(test.ExTester.Id));
                    if (instance.GetTraineeList().Exists(x => x.Id == test.ExTrainee.Id))
                        test.ExTrainee = new ExternalTrainee(GetTraineeByID(test.ExTrainee.Id));
                    return test;
                }
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }

        public List<Test> GetOptionalTestsByHour(Test dataSourse, Trainee trainee)
        {
            List<Test> optionalTests = new List<Test>();
            List<Tester> optionalTesters = GetTestersPartialListByPredicate(x => x.TypeCarToTest == dataSourse.CarType);
            if (optionalTesters.Count == 0)
                throw new KeyNotFoundException("לא קיים בוחן עבור סוג רכב זה.");
            optionalTesters = (from tester in optionalTesters where (IsTestersWorkAtSpesificHour(tester, dataSourse.HourOfTest)) select tester).ToList();
            if (optionalTesters.Count == 0)
                throw new KeyNotFoundException("לא קיים בוחן העובד בשעה הרצויה, אנא נסה בשעה אחרת.");
            bool IsAddressErrorOccur = false;
            try
            {
                optionalTesters = (from tester in optionalTesters
                                   let x = GetDistanceBetweenTwoAddresses(dataSourse.StartTestAddress, tester.Address, ref IsAddressErrorOccur)
                                   where x <= tester.MaxDistance && x >= 0
                                   select tester).ToList();
            }
            catch (InternalBufferOverflowException)
            {
                throw new InternalBufferOverflowException();
            }
            if (optionalTesters.Count == 0)
            {
                if (IsAddressErrorOccur)
                    throw new KeyNotFoundException("שגיאה! אחת מהכתובות במשך תהליך החיפוש אחר טסטר לא הייתה תקינה.");
                throw new KeyNotFoundException("שגיאה!  אין בוחנים עבור כתובת זו.");
            }
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


            List<Tester> optionalTesters = GetTestersPartialListByPredicate(x => x.TypeCarToTest == dataSourse.CarType);
            if (optionalTesters.Count == 0)
                throw new KeyNotFoundException("לא קיים בוחן עבור סוג רכב זה.");
            optionalTesters = (from tester in optionalTesters
                               where IsTesterAvailiableOnDateAndHour(tester, dataSourse.DateOfTest) && GetAvailiableHoursOfTesterForSpesificDate(tester, dataSourse.DateOfTest).Count != 0
                               select tester).ToList();
            if (optionalTesters.Count == 0)
                throw new KeyNotFoundException("לא קיים בוחן העובד בתאריך הרצוי, אנא נסה תאריך אחר.");
            bool IsAddressErrorOccur = false;
            try
            {
                optionalTesters = (from tester in optionalTesters
                                   let x = GetDistanceBetweenTwoAddresses(dataSourse.StartTestAddress, tester.Address, ref IsAddressErrorOccur)
                                   where x <= tester.MaxDistance && x >= 0
                                   select tester).ToList();
            }
            catch (InternalBufferOverflowException)
            {
                throw new InternalBufferOverflowException();
            }
            if (optionalTesters.Count == 0)
            {
                if (IsAddressErrorOccur)
                    throw new KeyNotFoundException("שגיאה! אחת מהכתובות במשך תהליך החיפוש אחר טסטר לא הייתה תקינה.");
                throw new KeyNotFoundException("שגיאה!  אין בוחנים עבור כתובת זו.");
            }

            bool[] tmp = new bool[6]; //tmp array for delete dublicates.
            for (int i = 0; i < 6; ++i)
                tmp[i] = false;

            foreach (var item in optionalTesters)
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
            try
            {
                var it = (from item in GetTestsList() where func(item) orderby item.DateOfTest, item.HourOfTest select item).ToList();
                return it;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
                return new List<Test>();
            }
        }
        public List<Tester> GetTestersPartialListByPredicate(Func<BO.Tester, bool> func)
        {
            try
            {
                var ot = (from item in GetTestersList() where func(item) orderby item.LastName, item.FirstName select item).ToList();
                return ot;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public List<Trainee> GetTraineesPartialListByPredicate(Func<BO.Trainee, bool> func)
        {
            try
            {
                var it = (from item in GetTraineeList() where func(item) orderby item.LastName, item.FirstName select item).ToList();
                return it;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }



        public string GetStringOfTraineeLicenses(string id)
        {
            string existing = "";
            Trainee trainee;
            try
            {
                trainee = GetTraineeByID(id);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
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
        public int GetTraineeNumTestedTest(Trainee t)
        {
            int num = 0;
            foreach (var item in t.TestList)
            {
                if (item.DateOfTest < DateTime.Now)
                    num++;
            }
            return num;
        }
        public IEnumerable<IGrouping<CarTypeEnum, Test>> GetTestsGroupedByCarType()
        {
            try
            {
                return from item in GetTestsList()
                       orderby item.TestId
                       group item by item.CarType
                            into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public IEnumerable<IGrouping<string, Test>> GetTestsGroupedByCity()
        {
            try
            {
                return from item in GetTestsList()
                       orderby item.TestId
                       group item by item.City
                       into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public IEnumerable<IGrouping<TestStatus, Test>> GetTestsGroupedByStatus()
        {
            return from item in GetTestsList()
                   orderby item.TestId
                   group item by item.Status
                       into g
                   orderby g.Key
                   select g;
        }

        public IEnumerable<IGrouping<string, Tester>> GetTestersGroupedByCity()
        {
            try
            {
                return from item in GetTestersList()
                       orderby item.LastName, item.FirstName
                       group item by item.City
                            into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public IEnumerable<IGrouping<int, Tester>> GetTestersGropedBySeniority()
        {
            try
            {
                return from item in GetTestersList()
                       orderby item.LastName, item.FirstName
                       group item by item.Seniority
                            into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public IEnumerable<IGrouping<CarTypeEnum, Trainee>> GetTraineesGroupedByCarType()
        {
            try
            {
                return from item in GetTraineeList()
                       orderby item.LastName, item.FirstName
                       group item by item.CurrCarType
                       into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public IEnumerable<IGrouping<CarTypeEnum, Tester>> GetTestersGrupedBySpecialization()
        {
            try
            {
                return from item in GetTestersList()
                       orderby item.LastName, item.FirstName
                       group item by item.TypeCarToTest
                       into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public IEnumerable<IGrouping<int, Tester>> GetTestersGropedByMaxDistance()
        {
            try
            {
                return from item in GetTestersList()
                       orderby item.LastName, item.FirstName
                       group item by item.MaxDistance
                            into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }

        public IEnumerable<IGrouping<string, Trainee>> GetTraineesGroupsBySchool()
        {
            try
            {
                return from item in GetTraineeList()
                       orderby item.LastName, item.FirstName
                       group item by item.SchoolName
                       into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public IEnumerable<IGrouping<string, Trainee>> GetTraineesGroupsByTeacher()
        {
            try
            {
                return from item in GetTraineeList()
                       orderby item.LastName, item.FirstName
                       group item by item.SchoolName
                       into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public IEnumerable<IGrouping<int, Trainee>> GetTraineesGroupedByNumOfTests()
        {
            try
            {
                return from item in GetTraineeList()
                       orderby item.LastName, item.FirstName
                       group item by GetTraineeNumTestedTest(item)
                       into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public IEnumerable<IGrouping<string, Trainee>> GetTraineessGroupedByCity()
        {
            try
            {
                return from item in GetTraineeList()
                       orderby item.LastName, item.FirstName
                       group item by item.City
                       into g
                       orderby g.Key
                       select g;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
        }
        public int GetTesterNumOfTestForDateWeek(Tester tester, DateTime a)
        {
            int num = 0;
            //DateTime weekStart = a.AddDays(-(int)a.DayOfWeek);
            //List<TesterTest> temp = tester.TestList;
            //var x = from item in temp where (item.DateOfTest.AddDays(-(int)item.DateOfTest.DayOfWeek) == weekStart) where !item.IsTestAborted select num++;

            foreach (var item in tester.TestList)
            {
                if (item.DateOfTest.AddDays(-(int)item.DateOfTest.DayOfWeek) == a.AddDays(-(int)a.DayOfWeek) && !item.IsTestAborted)
                {
                    num++;
                }
            }
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

        //public IEnumerable<IGrouping<Object, Test>> GetTestsGroupedByredicate(Func<BO.Test, bool> func)
        //{
        //    try
        //    {
        //        var it = from item in GetTestsList() orderby item.TestId group item by func(item);
        //        return it;

        //    }
        //    catch (DirectoryNotFoundException e)
        //    {
        //        throw e;
        //    }
        //}


        double GetDistanceBetweenTwoAddresses(Address a, Address b, ref bool addressError)
        {
            double CalculatedDistance = 0;
            bool isNetBuisy = false;
            int i = 0;
            do
            {
                //string origin = "pisga 45 st. jerusalem"; //or "תקווה פתח 100 העם אחד "etc.
                string origin = a.Street + " " + a.BuildingNumber + " " + a.City;
                //string destination = "gilgal 78 st. ramat-gan";//or "גן רמת 10 בוטינסקי'ז "etc.
                string destination = b.Street + " " + b.BuildingNumber + " " + b.City;
                string KEY = @"8D0srxBcNkZk8VBRA0LBDrJEHO9KNMni";
                string url = @"https://www.mapquestapi.com/directions/v2/route" +
                 @"?key=" + KEY +
                 @"&from=" + origin +
                 @"&to=" + destination +
                 @"&outFormat=xml" +
                 @"&ambiguities=ignore&routeType=fastest&doReverseGeocode=false" +
                 @"&enhancedNarrative=false&avoidTimedConditions=false";
                //request from MapQuest service the distance between the 2 addresses
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader sreader = new StreamReader(dataStream);
                string responsereader = sreader.ReadToEnd();
                response.Close();
                //the response is given in an XML format
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(responsereader);
                if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "0")
                //we have the expected answer
                {
                    //display the returned distance
                    XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                    CalculatedDistance = Convert.ToDouble(distance[0].ChildNodes[0].InnerText) * 1.609344;
                    //Console.WriteLine("Distance In KM: " + distInMiles * 1.609344);
                    //display the returned driving time
                    XmlNodeList formattedTime = xmldoc.GetElementsByTagName("formattedTime");
                    //CalculatedDistance = double.Parse(formattedTime[0].ChildNodes[0].InnerText);
                    //Console.WriteLine("Driving Time: " + CalculatedDistance);
                }
                else if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText ==
                "402")
                //we have an answer that an error occurred, one of the addresses is not found
                {
                    if (!addressError)
                        addressError = true;
                    return -1;
                }
                else //busy network or other error...
                {
                    Thread.Sleep(2000);
                }
            }
            while (isNetBuisy && i++ < 10);
            if (i >= 10)
                throw new InternalBufferOverflowException();
            return CalculatedDistance;
        }

        public void UpdateStatistics()
        {
            SystemStatistics.Format();
            foreach (var tester in GetTestersList())
            {
                SystemStatistics.SumTesterDistanceToTest += tester.MaxDistance;
                SystemStatistics.SumNumOfTestsPerWeek += tester.MaxTestsPerWeek;
                switch (tester.TypeCarToTest)
                {
                    case CarTypeEnum.אוטובוס:
                        SystemStatistics.NumOfTestersBus++;
                        break;
                    case CarTypeEnum.אופנוע:
                        SystemStatistics.NumOfTestersMotorCycle++;
                        break;
                    case CarTypeEnum.רכב_פרטי:
                        SystemStatistics.NumOfTestersPrivateCar++;
                        break;
                    case CarTypeEnum.רכב_פרטי_אוטומט:
                        SystemStatistics.NumOfTestersAutoPrivateCar++;
                        break;
                    case CarTypeEnum.משאית_12_טון:
                        SystemStatistics.NumOfTestersTruck12Ton++;
                        break;
                    case CarTypeEnum.משאית_ללא_הגבלה:
                        SystemStatistics.NumOfTestersTruckUnlimited++;
                        break;
                }
            }
            foreach (var trainee in GetTraineeList())
            {
                switch (trainee.CurrCarType)
                {
                    case CarTypeEnum.אוטובוס:
                        SystemStatistics.NumOfTraineesBus++;
                        break;
                    case CarTypeEnum.אופנוע:
                        SystemStatistics.NumOfTraineesMotorCycle++;
                        break;
                    case CarTypeEnum.רכב_פרטי:
                        SystemStatistics.NumOfTraineesPrivateCar++;
                        break;
                    case CarTypeEnum.רכב_פרטי_אוטומט:
                        SystemStatistics.NumOfTraineesAutoPrivateCar++;
                        break;
                    case CarTypeEnum.משאית_12_טון:
                        SystemStatistics.NumOfTraineesTruck12Ton++;
                        break;
                    case CarTypeEnum.משאית_ללא_הגבלה:
                        SystemStatistics.NumOfTraineesTruckUnlimited++;
                        break;
                }
            }
            foreach (var test in GetTestsList())
            {
                if (test.IsTestAborted)
                    SystemStatistics.NumOfAbortedTests++;
                else if (!test.IsTesterUpdateStatus)
                    SystemStatistics.NumOfTestWaitForUpdate++;
                else if (test.IsPassed)
                    SystemStatistics.NumOfSuccessedTests++;
                else
                    SystemStatistics.NumOfFailedTest++;
                switch (test.CarType)
                {
                    case CarTypeEnum.אוטובוס:
                        SystemStatistics.NumOfTestsBus++;
                        break;
                    case CarTypeEnum.אופנוע:
                        SystemStatistics.NumOfTestsMotorCycle++;
                        break;
                    case CarTypeEnum.רכב_פרטי:
                        SystemStatistics.NumOfTestsPrivateCar++;
                        break;
                    case CarTypeEnum.רכב_פרטי_אוטומט:
                        SystemStatistics.NumOfTestsAutoPrivateCar++;
                        break;
                    case CarTypeEnum.משאית_12_טון:
                        SystemStatistics.NumOfTestsTruck12Ton++;
                        break;
                    case CarTypeEnum.משאית_ללא_הגבלה:
                        SystemStatistics.NumOfTestsTruckUnlimited++;
                        break;
                }
            }
        }
        public void AddStatisticsChangedObserve(Action action)
        {
            StatisticsChanged += action;
        }
        public int GetTraineeNumOfTotalTests(Trainee t)
        {
            return t.TestList.Count;
        }
    }
}
