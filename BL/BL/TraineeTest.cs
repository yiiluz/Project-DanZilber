using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class TraineeTest
    {
        private string testId;
        private ExternalTester exTester;
        private DateTime dateOfTest = new DateTime();
        private int hourOfTest;
        private CarTypeEnum carType;
        private Address startTestAddress = new Address();
        private bool distanceKeeping;
        private bool reverseParking;
        private bool mirrorsCheck;
        private bool signals;
        private bool correctSpeed;
        private bool isPassed;
        private string testerNotes;

        public TraineeTest(Test test)
        {
            testId = test.TestId;
            exTester = test.ExTester;
            dateOfTest = test.DateOfTest;
            hourOfTest = test.HourOfTest;
            carType = test.CarType;
            startTestAddress = test.StartTestAddress;
            distanceKeeping = test.DistanceKeeping;
            reverseParking = test.ReverseParking;
            mirrorsCheck = test.MirrorsCheck;
            signals = test.Signals;
            correctSpeed = test.CorrectSpeed;
            isPassed = test.IsPassed;
            testerNotes = test.TesterNotes;
        }

        public string TestId { get => testId; set => testId = value; }
        public ExternalTester ExTester { get => exTester; set => exTester = value; }
        public DateTime DateOfTest { get => dateOfTest; set => dateOfTest = value; }
        public int HourOfTest { get => hourOfTest; set => hourOfTest = value; }
        public CarTypeEnum CarType { get => carType; set => carType = value; }
        public Address StartTestAddress { get => startTestAddress; set => startTestAddress = value; }
        public bool DistanceKeeping { get => distanceKeeping; set => distanceKeeping = value; }
        public bool ReverseParking { get => reverseParking; set => reverseParking = value; }
        public bool MirrorsCheck { get => mirrorsCheck; set => mirrorsCheck = value; }
        public bool Signals { get => signals; set => signals = value; }
        public bool CorrectSpeed { get => correctSpeed; set => correctSpeed = value; }
        public bool IsPassed { get => isPassed; set => isPassed = value; }
        public string TesterNotes { get => testerNotes; set => testerNotes = value; }

        //public TraineeTest(Trainee tr, Tester tr, Test ts)
        //{
        //    exTester = new ExternalTester()
        //}
    }
}
