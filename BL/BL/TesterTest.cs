using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class TesterTest : TestResult
    {
        private string testId;
        private ExternalTrainee exTrainee;
        private DateTime dateOfTest = new DateTime();
        private int hourOfTest;
        private CarTypeEnum carType;
        private Address startTestAddress = new Address();

        public TesterTest(Test test)
        {
            TestId = test.TestId;
            ExTrainee = test.ExTrainee;
            DateOfTest = test.DateOfTest;
            HourOfTest = test.HourOfTest;
            CarType = test.CarType;
            StartTestAddress = test.StartTestAddress;
            DistanceKeeping = test.DistanceKeeping;
            ReverseParking = test.ReverseParking;
            MirrorsCheck = test.MirrorsCheck;
            Signals = test.Signals;
            CorrectSpeed = test.CorrectSpeed;
            IsPassed = test.IsPassed;
            TesterNotes = test.TesterNotes;
        }
        public TesterTest(DO.Test test)
        {
            TestId = test.TestId;
            ExTrainee = new ExternalTrainee(test.TraineeId);
            DateOfTest = test.DateOfTest;
            HourOfTest = test.HourOfTest;
            CarType = (CarTypeEnum)test.CarType;
            StartTestAddress = new Address(test.StartTestAddress.City, test.StartTestAddress.Street, test.StartTestAddress.BuildingNumber);
            DistanceKeeping = test.DistanceKeeping;
            ReverseParking = test.ReverseParking;
            MirrorsCheck = test.MirrorsCheck;
            Signals = test.Signals;
            CorrectSpeed = test.CorrectSpeed;
            IsPassed = test.IsPassed;
            TesterNotes = test.TesterNotes;
        }

        public string TestId { get => testId; set => testId = value; }
        public ExternalTrainee ExTrainee { get => exTrainee; set => exTrainee = value; }
        public DateTime DateOfTest { get => dateOfTest; set => dateOfTest = value; }
        public int HourOfTest { get => hourOfTest; set => hourOfTest = value; }
        public CarTypeEnum CarType { get => carType; set => carType = value; }
        public Address StartTestAddress { get => startTestAddress; set => startTestAddress = value; }
    }
}
