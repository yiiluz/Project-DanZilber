using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DL
{
    internal class DLObject : IDAL
    {
        protected static DLObject instance = new DLObject();
        protected DLObject() { }
        public static DLObject GetDLObject { get => instance; }

        void IDAL.AddTest(Test t)
        {
            if (DataSource.tests.Find(x => x.TestId == t.TestId) != null)
            {
                throw new DuplicateWaitObjectException("ERROR! The test or the test serial already exists in the system ");
            }
            else if (DataSource.testers.Find(x => x.Id == t.TesterId) == null)
            {
                throw new KeyNotFoundException("ERROR! The tester does not exist in the system ");
            }
            else if (DataSource.trainees.Find(x => x.Id == t.TraineeId) == null)
            {
                throw new KeyNotFoundException("ERROR! The trainee does not exist in the system ");
            }
            else
            {
                DataSource.tests.Add(t);
            }
        }

        void IDAL.AddTester(Tester t)
        {
            if (DataSource.testers.Find(x => x.Id == t.Id) != null)
            {
                throw new DuplicateWaitObjectException("This tester is already registered in the system");
            }
            else
            {
                DataSource.testers.Add(t);
            }
        }

        void IDAL.AddTrainee(Trainee t)
        {
            if (DataSource.trainees.Find(x => x.Id == t.Id) != null)
            {
                throw new DuplicateWaitObjectException("This trainee is already registered in the system");
            }
            else
            {
                DataSource.trainees.Add(t);
            }
        }

        List<Tester> IDAL.GetTestersList()
        {
            List<Tester> testers2 = new List<Tester>(DataSource.testers);
            return testers2;

        }

        List<Test> IDAL.GetTestsList()
        {
            List<Test> tests2 = new List<Test>(DataSource.tests);
            return tests2;
        }

        List<Trainee> IDAL.GetTraineeList()
        {
            List<Trainee> trainees2 = new List<Trainee>(DataSource.trainees);
            return trainees2;
        }

        void IDAL.RemoveTester(string id)
        {
            if (DataSource.testers.Exists(x => x.Id == id))
            {
                DataSource.testers.Remove(DataSource.testers.Find(x => x.Id == id));
            }
            else
            {
                throw new KeyNotFoundException("This tester does not exist in the system");
            }
        }

        void IDAL.RemoveTrainee(string id)
        {
            if (DataSource.trainees.Find(x => x.Id == id) != null)
            {
                DataSource.trainees.Remove(DataSource.trainees.Find(x => x.Id == id));
            }
            else
            {
                throw new KeyNotFoundException("This trainee does not exist in the system");
            }
        }

        void IDAL.UpdateTest(Test t)
        {
            int index = DataSource.tests.FindIndex(x => x.TestId == t.TestId);
            if (index > -1)
            {
                DataSource.tests[index] = t;
            }
            else
            {
                throw new KeyNotFoundException("This test does not exist in the system");
            }
        }

        void IDAL.UpdateTesterDetails(Tester T)
        {
            int index = DataSource.testers.FindIndex(x => x.Id == T.Id);
            if (index > -1)
            {
                DataSource.testers[index] = T;
            }
            else
            {
                throw new KeyNotFoundException("This tester does not exist in the system");
            }
        }

        void IDAL.UpdateTraineeDetails(Trainee T)
        {
            int index = DataSource.trainees.FindIndex(x => x.Id == T.Id);
            if (index > -1)
            {
                DataSource.trainees[index] = T;
            }
            else
            {
                throw new KeyNotFoundException("this trainee does not exist in the system");
            }
        }
        void IDAL.RemoveTest(string id)
        {
            if (DataSource.tests.Exists(x => x.TestId == id))
            {
                DataSource.tests.Remove(DataSource.tests.Find(x => x.TestId == id));
            }
            else
            {
                throw new KeyNotFoundException("This test does not exist in the system");
            }
        }
        Dictionary<string, Object> IDAL.GetConfig()
        {
            Dictionary<string, Object> keyValues = new Dictionary<string, Object>();
            foreach (var item in DataSource.Configuration)
            {
                if (item.Value.Readable == true)
                {
                    keyValues.Add(item.Key, item.Value.Value);
                }
            }
            return keyValues;
        }
        void IDAL.SetConfig(string parm, Object value)
        {
            foreach (var item in DataSource.Configuration)
            {
                if (item.Key == parm)
                {
                    if (item.Value.Writable == true)
                    {
                        item.Value.Value = value;
                        return;
                    }
                    throw new AccessViolationException("ERROR! There is no permission to change this configuration property");
                }
            }
        }
        Object IDAL.GetConfig(string s)
        {
            foreach (var item in DataSource.Configuration)
            {
                if (item.Key == s)
                {
                    if (item.Value.Readable == true)
                    {
                        return item.Value.Value;
                    }
                    else
                    {
                        throw new AccessViolationException("ERROR! There is no permission to read this configutation property");
                    }
                }
            }
            throw new KeyNotFoundException("ERROR! There is no configuration feature with this name");
        }
        public bool[,] GetTesterSchedule(string id)
        {
            bool[,] tmp;
            try
            {
                tmp = DataSource.Schedules[id];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Schedule not exist for id " + id + ".");
            }
            return tmp;
        }
        public void UpdateTesterSchedule(string id, bool[,] sched)
        {
            try
            {
                DataSource.Schedules[id] = sched;
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Can't Update Schedule. Schedule not exist for id " + id + ".");
            }
        }
        public void AddTesterSchedule(string id, bool[,] sched)
        {
            try
            {
                DataSource.Schedules.Add(id, sched);
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("Internal error. Can't add Schedule for id " + id + ".");
            }
        }

        public void RemoveTesterSchedule(string id)
        {
            try
            {
                DataSource.Schedules.Remove(id);
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("Internal Error. Can't delete Schedule for id " + id + ".");
            }
        }
    }
}

