using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DO;
namespace DL
{
    internal class DLObject : BaseDL
    {
        protected static DLObject instance = new DLObject();
        protected DLObject()
        {
            (new Thread(ConfigUpdatedThreadFunc)).Start();
        }
        public static DLObject GetDLObject { get => instance; }
 
        public static void ConfigUpdatedAddEvent(Action action)
        {
            instance.ConfigUpdated += action;
        }
        private static volatile bool isConfigUpdated = false;
        static void ConfigUpdatedThreadFunc()
        {
            while (true)
            {
                if (isConfigUpdated)
                {
                    isConfigUpdated = false;
                    instance?.ConfigUpdated();
                }
                Thread.Sleep(1000);
            }
        }

        public override void AddTest(Test t)
        {
            if (DataSource.tests.Find(x => x.TestId == t.TestId) != null)
            {
                throw new DuplicateWaitObjectException("שגיאה! המבחן אן מספר המבחן כבר קיים קיים במערכת.  ");
            }
            else if (DataSource.testers.Find(x => x.Id == t.TesterId) == null)
            {
                throw new KeyNotFoundException("שגיאה! הבוחן אינו קיים במערכת. ");
            }
            else if (DataSource.trainees.Find(x => x.Id == t.TraineeId) == null)
            {
                throw new KeyNotFoundException("שגיאה! הנבחן אינו קיים במערכת.");
            }
            else
            {
                DataSource.tests.Add(t);
            }
        }
        public override void AddTester(Tester t)
        {
            if (DataSource.testers.Find(x => x.Id == t.Id) != null)
            {
                throw new DuplicateWaitObjectException("שגיאה! הבוחן כבר קיים במערכת.");
            }
            else
            {
                DataSource.testers.Add(t);
            }
        }
        public override void AddTrainee(Trainee t)
        {
            if (DataSource.trainees.Find(x => x.Id == t.Id) != null)
            {
                throw new DuplicateWaitObjectException("שגיאה! הנבחן כבר קיים במערכת.");
            }
            else
            {
                DataSource.trainees.Add(t);
            }
        }
        public override void AddTesterSchedule(string id, bool[,] sched)
        {
            try
            {
                DataSource.Schedules.Add(id, sched);
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("שגיאה פנימית! לא ניתן להוסיף תעודת זהות  " + id + ".");
            }
        }

        public override List<Tester> GetTestersList()
        {
            return new List<Tester>(DataSource.testers);

        }
        public override List<Test> GetTestsList()
        {
            return new List<Test>(DataSource.tests);
        }
        public override List<Trainee> GetTraineeList()
        {
            return new List<Trainee>(DataSource.trainees);
        }

        public override void RemoveTester(string id)
        {
            if (DataSource.testers.Exists(x => x.Id == id))
            {
                DataSource.testers.Remove(DataSource.testers.Find(x => x.Id == id));
            }
            else
            {
                throw new KeyNotFoundException("הבוחן אינו קיים במערכת");
            }
        }
        public override void RemoveTrainee(string id)
        {
            if (DataSource.trainees.Find(x => x.Id == id) != null)
            {
                DataSource.trainees.Remove(DataSource.trainees.Find(x => x.Id == id));
            }
            else
            {
                throw new KeyNotFoundException("הנבחן אינו קיים במערכת");
            }
        }
        public override void RemoveTest(string id)
        {
            if (DataSource.tests.Exists(x => x.TestId == id))
            {
                DataSource.tests.Remove(DataSource.tests.Find(x => x.TestId == id));
            }
            else
            {
                throw new KeyNotFoundException("המבחן אינו קיים במערכת");
            }
        }
        public override void RemoveTesterSchedule(string id)
        {
            try
            {
                DataSource.Schedules.Remove(id);
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("שגיאה פנימית לא א יכול למחוק מערכת שעות עבור תעודת זהות: " + id + ".");
            }
        }

        public override void UpdateTestDetails(Test t)
        {
            int index = DataSource.tests.FindIndex(x => x.TestId == t.TestId);
            if (index > -1)
            {
                DataSource.tests[index] = t;
            }
            else
            {
                throw new KeyNotFoundException("המבחן לא קיים במערכת");
            }
        }
        public override void UpdateTesterDetails(Tester T)
        {
            int index = DataSource.testers.FindIndex(x => x.Id == T.Id);
            if (index > -1)
            {
                DataSource.testers[index] = T;
            }
            else
            {
                throw new KeyNotFoundException("הבוחן לא קיים במערכת.");
            }
        }
        public override void UpdateTraineeDetails(Trainee T)
        {
            int index = DataSource.trainees.FindIndex(x => x.Id == T.Id);
            if (index > -1)
            {
                DataSource.trainees[index] = T;
            }
            else
            {
                throw new KeyNotFoundException("הנבחן הזה לא קיים במערכת.");
            }
        }
        public override void UpdateTesterSchedule(string id, bool[,] sched)
        {
            try
            {
                DataSource.Schedules[id] = sched;
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException(" לא ניתן לעדכן מערכת שעות. לא קיים מערכת שעות עבור תעודת זהות זו:" + id + ".");
            }
        }

        public override Dictionary<string, Object> GetConfig()
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
        public override void SetConfig(string parm, Object value)
        {
            foreach (var item in DataSource.Configuration)
            {
                if (item.Key == parm)
                {
                    if (item.Value.Writable == true)
                    {
                        item.Value.Value = value;
                        isConfigUpdated = true;
                        return;
                    }
                    throw new AccessViolationException("שגיאה! אין הרשאה לשנות מאפיין קונפיגורציה זה");
                }
            }
        }
        public override Object GetConfig(string s)
        {

            if (DataSource.Configuration[s] != null)
                return DataSource.Configuration[s];
            else
            throw new KeyNotFoundException("שגיאה! אין מאפיין קונפיגורציה עם שם זה.");
        }
        public override bool[,] GetTesterSchedule(string id)
        {
            bool[,] tmp;
            try
            {
                tmp = DataSource.Schedules[id];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException(" לא קיימת מערכת שעות עבור תעודת זהות:" + id + ".");
            }
            return tmp;
        }
        public override bool CheckTheIntegrityOfSystemDataInXml(ref string a)
        {
            return true;
        }
    }
}

