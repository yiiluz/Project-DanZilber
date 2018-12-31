using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Test
    {
        

        private string testId;
        private string testerId;
        private string traineeId;
        private DateTime dateOfTest = new DateTime();
        private int hourOfTest;
        private Address startTestAddress = new Address();
        private CarTypeEnum carType;
        private bool distanceKeeping;
        private bool reverseParking;
        private bool mirrorsCheck;
        private bool signals;
        private bool correctSpeed;
        private bool isPassed;
        private string testerNotes;
        private bool isTesterUpdateStatus;

        public Test(string id)
        {
            TestId = id;
        }
        public string TestId { get => testId; set => testId = value; }
        public string TesterId { get => testerId; set => testerId = value; }
        public string TraineeId { get => traineeId; set => traineeId = value; }
        public DateTime DateOfTest { get => dateOfTest; set => dateOfTest = value; }
        public int HourOfTest { get => hourOfTest; set => hourOfTest = value; }
        public Address StartTestAddress { get => startTestAddress; set => startTestAddress = value; }
        public CarTypeEnum CarType { get => carType; set => carType = value; }
        public bool DistanceKeeping { get => distanceKeeping; set => distanceKeeping = value; }
        public bool ReverseParking { get => reverseParking; set => reverseParking = value; }
        public bool MirrorsCheck { get => mirrorsCheck; set => mirrorsCheck = value; }
        public bool Signals { get => signals; set => signals = value; }
        public bool CorrectSpeed { get => correctSpeed; set => correctSpeed = value; }
        public bool IsPassed { get => isPassed; set => isPassed = value; }
        public string TesterNotes { get => testerNotes; set => testerNotes = value; }
        public bool IsTesterUpdateStatus { get => isTesterUpdateStatus; set => isTesterUpdateStatus = value; }

        public override string ToString()
        {
            string tmp = "Test ID: " + TestId + ".\nTester ID: " + TesterId + ".\nTrainee ID: " + TraineeId + ".\nDate of Test: " +
                DateOfTest.ToShortDateString() + ".\nTest-start address: " + StartTestAddress + ".\n" + (IsPassed ? "Trainee passed" : "Trainee didn't passed") + ".\n";
            return tmp;
        }
    }
}
