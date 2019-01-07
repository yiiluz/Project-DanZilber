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
        protected static DLObject instance = null;
        protected DLObject() { }
        public static DLObject GetDLObject()
        {
            if (instance == null)
            {
                instance = new DLObject();
                DateSource a = DateSource.GetDSObject();
            }
            return instance;
        }

        void IDAL.AddTest(Test t)
        {
            if (DateSource.tests.Find(x => x.TestId == t.TestId) != null)
            {
                throw new DuplicateWaitObjectException("ERROR! The test or the test serial already exists in the system ");
            }
            else if (DateSource.testers.Find(x => x.Id == t.TesterId) == null)
            {
                throw new KeyNotFoundException("ERROR! The tester does not exist in the system ");
            }
            else if (DateSource.trainees.Find(x => x.Id == t.TraineeId) == null)
            {
                throw new KeyNotFoundException("ERROR! The trainee does not exist in the system ");
            }
            else
            {
                DateSource.tests.Add(t);
            }
        }

        void IDAL.AddTester(Tester t)
        {
            if (DateSource.testers.Find(x => x.Id == t.Id) != null)
            {
                throw new DuplicateWaitObjectException("This tester is already registered in the system");
            }
            else
            {
                DateSource.testers.Add(t);
                DateSource.Schedules.Add(t.Id, t.AvailableWorkTime);
            }
        }

        void IDAL.AddTrainee(Trainee t)
        {
            if (DateSource.trainees.Find(x => x.Id == t.Id) != null)
            {
                throw new DuplicateWaitObjectException("This trainee is already registered in the system");
            }
            else
            {
                DateSource.trainees.Add(t);
            }
        }

        List<Tester> IDAL.GetTestersList()
        {
            List<Tester> testers2 = new List<Tester>(DateSource.testers);
            return testers2;

        }

        List<Test> IDAL.GetTestsList()
        {
            List<Test> tests2 = new List<Test>(DateSource.tests);
            return tests2;
        }

        List<Trainee> IDAL.GetTraineeList()
        {
            List<Trainee> trainees2 = new List<Trainee>(DateSource.trainees);
            return trainees2;
        }

        void IDAL.RemoveTester(string id)
        {
            if (DateSource.testers.Exists(x => x.Id == id))
            {
                DateSource.testers.Remove(DateSource.testers.Find(x => x.Id == id));
            }
            else
            {
                throw new KeyNotFoundException("This tester does not exist in the system");
            }
        }

        void IDAL.RemoveTrainee(string id)
        {
            if (DateSource.trainees.Find(x => x.Id == id) != null)
            {
                DateSource.trainees.Remove(DateSource.trainees.Find(x => x.Id == id));
            }
            else
            {
                throw new KeyNotFoundException("This trainee does not exist in the system");
            }
        }

        void IDAL.UpdateTest(Test t)
        {
            int index = DateSource.tests.FindIndex(x => x.TestId == t.TestId);
            if (index > -1)
            {
                DateSource.tests[index] = t;
            }
            else
            {
                throw new KeyNotFoundException("This test does not exist in the system");
            }
        }

        void IDAL.UpdateTesterDetails(Tester T)
        {
            int index = DateSource.testers.FindIndex(x => x.Id == T.Id);
            if (index > -1)
            {
                DateSource.testers[index] = T;
            }
            else
            {
                throw new KeyNotFoundException("This tester does not exist in the system");
            }
        }

        void IDAL.UpdateTraineeDetails(Trainee T)
        {
            int index = DateSource.trainees.FindIndex(x => x.Id == T.Id);
            if (index > -1)
            {
                DateSource.trainees[index] = T;
            }
            else
            {
                throw new KeyNotFoundException("this trainee does not exist in the system");
            }
        }
        void IDAL.RemoveTest(string id)
        {
            if (DateSource.tests.Exists(x => x.TestId == id))
            {
                DateSource.tests.Remove(DateSource.tests.Find(x => x.TestId == id));
            }
            else
            {
                throw new KeyNotFoundException("This test does not exist in the system");
            }
        }
        Dictionary<string, Object> IDAL.GetConfig()
        {
            Dictionary<string, Object> keyValues = new Dictionary<string, Object>();
            foreach (var item in DateSource.Configuration)
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
            foreach (var item in DateSource.Configuration)
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
            foreach (var item in DateSource.Configuration)
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
    }
}
