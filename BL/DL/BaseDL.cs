using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DL
{
    public abstract class BaseDL : DO.IDAL
    {
        protected Action ConfigUpdated = null;
        public abstract void AddTester(Tester T);
        public abstract void RemoveTester(string id);
        public abstract void UpdateTesterDetails(Tester T);
        public abstract void AddTrainee(Trainee T);
        public abstract void RemoveTrainee(string id);
        public abstract void RemoveTest(string id);
        public abstract void UpdateTraineeDetails(Trainee T);
        public abstract void AddTest(Test t);
        public abstract void UpdateTestDetails(Test t);
        public abstract List<Tester> GetTestersList();
        public abstract List<Trainee> GetTraineeList();
        public abstract List<Test> GetTestsList();
        public abstract Dictionary<String, Object> GetConfig();
        public abstract Object GetConfig(String s);
        public abstract void SetConfig(String parm, Object value);
        public abstract void AddTesterSchedule(string id, bool[,] sched);
        public abstract void UpdateTesterSchedule(string id, bool[,] sched);
        public abstract bool[,] GetTesterSchedule(string id);
        public abstract void RemoveTesterSchedule(string id);
        public abstract bool CheckTheIntegrityOfSystemDataInXml(ref string a);
    }

}
