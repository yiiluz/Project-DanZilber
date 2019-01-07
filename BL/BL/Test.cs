using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Test
    {
        private string testId = "";
        private ExternalTrainee exTrainee;
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
        private bool isTesterUpdateStatus;
        public Test(DO.Test other)
        {
            TestId = other.TestId;
            exTrainee = new ExternalTrainee(other.TraineeId);/////////////////////////////////////
            exTester = new ExternalTester(other.TesterId);////////////////////////////////////////
            dateOfTest = other.DateOfTest;
            hourOfTest = other.HourOfTest;
            carType = (CarTypeEnum)other.CarType;
            startTestAddress = new Address(other.StartTestAddress.City, other.StartTestAddress.Street, other.StartTestAddress.BuildingNumber);
            distanceKeeping = other.DistanceKeeping;
            reverseParking = other.ReverseParking;
            mirrorsCheck = other.MirrorsCheck;
            signals = other.Signals;
            correctSpeed = other.CorrectSpeed;
            isPassed = other.IsPassed;
            testerNotes = other.TesterNotes;
            isTesterUpdateStatus = other.IsTesterUpdateStatus;
        }
        public Test(Test other)
        {
            exTrainee = other.ExTrainee;
            exTester = other.ExTester;
            dateOfTest = other.DateOfTest;
            hourOfTest = other.HourOfTest;
            carType = other.CarType;
            startTestAddress = new Address(other.StartTestAddress.City, other.StartTestAddress.Street, other.StartTestAddress.BuildingNumber);
            distanceKeeping = other.DistanceKeeping;
            reverseParking = other.ReverseParking;
            mirrorsCheck = other.MirrorsCheck;
            signals = other.Signals;
            correctSpeed = other.CorrectSpeed;
            isPassed = other.IsPassed;
            testerNotes = other.TesterNotes;
            isTesterUpdateStatus = other.isTesterUpdateStatus;
        }
        public Test() { }

        public ExternalTrainee ExTrainee { get => exTrainee; set => exTrainee = value; }
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
        public bool IsTesterUpdateStatus { get => isTesterUpdateStatus; set => isTesterUpdateStatus = value; }
        public string TestId { get => testId; set => testId = value; }
    }
}
