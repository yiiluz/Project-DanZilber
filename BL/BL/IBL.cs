using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public interface IBL
    {
        void AddTester(Tester T);
        void RemoveTester(string id);
        void UpdateTesterDetails(Tester T);
        void AddTrainee(Trainee T);
        void RemoveTrainee(string id);
        void AbortTest(string id);
        //void RemoveTest(string testId);
        void UpdateTraineeDetails(Trainee T);
        string AddTest(Test t);
        List<Test> GetOptionalTestsByDate(Test dataSourse, Trainee trainee);
        List<Test> GetOptionalTestsByHour(Test dataSourse, Trainee trainee);
        void UpdateTest(string id, TestResult t);
        List<Tester> GetTestersList();
        List<Trainee> GetTraineeList();
        List<Test> GetTestsList();
        Trainee GetTraineeByID(string id);
        Tester GetTesterByID(string id);
        Test GetTestByID(string id);
        List<Tester> GetAvailableTestersForSpecificDay(DateTime time, int hour, CarTypeEnum carType);
        List<Test> GetTestsPartialListByPredicate(Func<BO.Test, bool> func);
        List<Tester> GetTestersPartialListByPredicate(Func<BO.Tester, bool> func);
        List<Trainee> GetTraineesPartialListByPredicate(Func<BO.Trainee, bool> func);
        //bool IsHaveLicense(Trainee T, CarTypeEnum car);
        IEnumerable<Test> TheTestsWillBeDoneToday_Month(DateTime t, bool Byday);
        IEnumerable<IGrouping<CarTypeEnum, Tester>> GetTestersBySpecialization(bool byOrder = false);
        IEnumerable<IGrouping<string, Trainee>> GetStudentGroupsBySchool(bool byOrder = false);
        IEnumerable<IGrouping<string, Trainee>> GetStudentGroupsByTeacher(bool byOrder = false);
        IEnumerable<IGrouping<int, Trainee>> GetStudentsGroupedaccordingByNumOfTests(bool byOrder = false);
        IEnumerable<IGrouping<int, Tester>> GetTestersGroupedAccordingToYearsOfExperience();
    }
}
