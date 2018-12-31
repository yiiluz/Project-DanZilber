using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DL
{
    public interface IDAL
    {
        void AddTester(Tester T);
        void RemoveTester(string id);
        void UpdateTesterDetails(Tester T);
        void AddTrainee(Trainee T);
        void RemoveTrainee(string id);
        void RemoveTest(string id);
        void UpdateTraineeDetails(Trainee T);
        void AddTest(Test t);
        void UpdateTest(Test t);
        List<Tester> GetTestersList();
        List<Trainee> GetTraineeList();
        List<Test> GetTestsList();
        Dictionary<String, Object> GetConfig();
        Object GetConfig(String s);
        void SetConfig(String parm, Object value);
    }

}
