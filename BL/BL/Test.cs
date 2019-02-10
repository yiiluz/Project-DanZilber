using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class test used to implement driving test
    /// </summary>
    public class Test : TestResult
    {
        private readonly string testId = "";
        private ExternalTrainee exTrainee;
        private ExternalTester exTester;
        private DateTime dateOfTest = new DateTime();
        private int hourOfTest;
        private CarTypeEnum carType;
        private Address startTestAddress = new Address();
        private bool isTesterUpdateStatus;
        private bool isTestAborted = false;
        private TestStatus status;

        public Test() { }

        public Test(string id)
        {
            this.testId = id;
        }

        /// <summary>
        /// Copy constructor. used for create copy of original test with differents dates.
        /// </summary>
        /// <param name="Test obj to copy"></param>
        public Test(Test other) //needed for copying a few tests from original test with different dates.
        {
            testId = other.TestId;
            ExTrainee = other.ExTrainee;
            ExTester = other.ExTester;
            DateOfTest = other.DateOfTest;
            HourOfTest = other.HourOfTest;
            CarType = other.CarType;
            StartTestAddress = new Address(other.StartTestAddress.City, other.StartTestAddress.Street, other.StartTestAddress.BuildingNumber);
            DistanceKeeping = other.DistanceKeeping;
            ReverseParking = other.ReverseParking;
            MirrorsCheck = other.MirrorsCheck;
            Signals = other.Signals;
            CorrectSpeed = other.CorrectSpeed;
            IsPassed = other.IsPassed;
            TesterNotes = other.TesterNotes;
            IsTesterUpdateStatus = other.isTesterUpdateStatus;
            IsTestAborted = other.IsTestAborted;
        }

        public ExternalTrainee ExTrainee { get => exTrainee; set => exTrainee = value; }
        public ExternalTester ExTester { get => exTester; set => exTester = value; }
        public DateTime DateOfTest { get => dateOfTest; set => dateOfTest = value; }
        public int HourOfTest { get => hourOfTest; set => hourOfTest = value; }
        public CarTypeEnum CarType { get => carType; set => carType = value; }
        public Address StartTestAddress { get => startTestAddress; set => startTestAddress = value; }
        public bool IsTesterUpdateStatus { get => isTesterUpdateStatus; set => isTesterUpdateStatus = value; }
        public string TestId { get => testId; }
        public string City { get => StartTestAddress.City; set => startTestAddress.City = value; }
        public string Street { get => StartTestAddress.Street; set => startTestAddress.Street = value; }
        public int BuildingNumber { get => StartTestAddress.BuildingNumber; set => startTestAddress.BuildingNumber = value; }
        public bool IsTestAborted { get => isTestAborted; set => isTestAborted = value; }
        public TestStatus Status { get => isTestAborted ? TestStatus.מבוטל : (dateOfTest > DateTime.Now ? TestStatus.מבחן_עתידי : (!isTesterUpdateStatus ? TestStatus.מחכה_לעידכון : (IsPassed ? TestStatus.עבר : TestStatus.לא_עבר))); }

        public override string ToString()
        {
            string tmp = "מספר מבחן: " + TestId + "\n\n"
                + "פרטי בוחן:\n" + ExTester + "\n"
                + "פרטי נבחן:\n" + ExTrainee + "\n"
                + "תאריך המבחן: " + DateOfTest.ToShortDateString() + "\n"
                + "שעת התחלת המבחן: " + HourOfTest + ":00\n"
                + "כתובת התחלת המבחן: " + StartTestAddress + "\n"
                + (IsTesterUpdateStatus ? "תוצאות המבחן:\n" + base.ToString() : "")
                + "סטטוס המבחן: " + (IsTestAborted ? "בוטל." : (!IsTesterUpdateStatus ? "ממתין לעידכון תוצאות." : (IsPassed ? "התלמיד הצליח במבחן." : "התלמיד נכשל במבחן."))) + "\n";
            return tmp;
        }
    }
}
