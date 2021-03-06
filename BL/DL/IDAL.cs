﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DO
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
        void UpdateTestDetails(Test t);
        List<Tester> GetTestersList();
        List<Trainee> GetTraineeList();
        List<Test> GetTestsList();
        Dictionary<string, Object> GetConfig();
        Object GetConfig(string s);
        void SetConfig(string parm, Object value);
        void AddTesterSchedule(string id, bool[,] sched);
        void UpdateTesterSchedule(string id, bool[,] sched);
        bool[,] GetTesterSchedule(string id);
        void RemoveTesterSchedule(string id);
        bool CheckTheIntegrityOfSystemDataInXml(ref string a);
    }

}
