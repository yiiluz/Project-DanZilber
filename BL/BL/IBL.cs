using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public interface IBL
    {
        void UpdateStatistics();
        void AddStatisticsChangedObserve(Action action);
        Dictionary<String, Object> GetConfig();
        void SetConfig(String parm, Object value);
        void AddEventIfConfigChanged(Action action);
        void AddTester(Tester T);
        List<TesterTest> RemoveTester(string id);
        void UpdateTesterDetails(Tester T);
        void AddTrainee(Trainee T);
        void RemoveTrainee(string id);
        void AbortTest(string id);
        //void RemoveTest(string testId);
        void UpdateTraineeDetails(Trainee T);
        string AddTest(Test t);
        List<Test> GetOptionalTestsByDate(Test dataSourse, Trainee trainee);
        List<Test> GetOptionalTestsByHour(Test dataSourse, Trainee trainee);
        void UpdateTestResult(string id, TestResult t);
        List<Tester> GetTestersList();
        List<Trainee> GetTraineeList();
        List<Test> GetTestsList();
        Trainee GetTraineeByID(string id);
        Tester GetTesterByID(string id);
        Test GetTestByID(string id);
        //List<Tester> GetAvailableTestersForSpecificDay(DateTime time, CarTypeEnum carType);
        List<Test> GetTestsPartialListByPredicate(Func<BO.Test, bool> func);
        List<Tester> GetTestersPartialListByPredicate(Func<BO.Tester, bool> func);
        List<Trainee> GetTraineesPartialListByPredicate(Func<BO.Trainee, bool> func);
        string GetStringOfTraineeLicenses(string id);
        int GetTraineeNumTestedTest(Trainee t);
        int GetTraineeNumOfTotalTests(Trainee t);

        int GetTesterNumOfTestForDateWeek(Tester tester, DateTime a);


        IEnumerable<IGrouping<string, Trainee>> GetTraineesGroupsBySchool();
        IEnumerable<IGrouping<string, Trainee>> GetTraineesGroupsByTeacher();
        IEnumerable<IGrouping<int, Trainee>> GetTraineesGroupedByNumOfTests();
        IEnumerable<IGrouping<string, Trainee>> GetTraineessGroupedByCity();
        IEnumerable<IGrouping<CarTypeEnum, Trainee>> GetTraineesGroupedByCarType();



        IEnumerable<IGrouping<string, Tester>> GetTestersGroupedByCity();
        IEnumerable<IGrouping<int, Tester>> GetTestersGropedBySeniority();
        IEnumerable<IGrouping<CarTypeEnum, Tester>> GetTestersGrupedBySpecialization();
        IEnumerable<IGrouping<int, Tester>> GetTestersGropedByMaxDistance();



        IEnumerable<IGrouping<CarTypeEnum, Test>> GetTestsGroupedByCarType();
        IEnumerable<IGrouping<string, Test>> GetTestsGroupedByCity();

        IEnumerable<IGrouping<TestStatus, Test>> GetTestsGroupedByStatus();
    }
}
