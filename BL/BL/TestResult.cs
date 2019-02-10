using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class TestResult used to implement test results.
    /// </summary>
    public class TestResult
    {
        private bool distanceKeeping;
        private bool reverseParking;
        private bool mirrorsCheck;
        private bool signals;
        private bool correctSpeed;
        private bool isPassed;
        private string testerNotes;

        public bool DistanceKeeping { get => distanceKeeping; set => distanceKeeping = value; }
        public bool ReverseParking { get => reverseParking; set => reverseParking = value; }
        public bool MirrorsCheck { get => mirrorsCheck; set => mirrorsCheck = value; }
        public bool Signals { get => signals; set => signals = value; }
        public bool CorrectSpeed { get => correctSpeed; set => correctSpeed = value; }
        public bool IsPassed { get => isPassed; set => isPassed = value; }
        public string TesterNotes { get => testerNotes; set => testerNotes = value; }

        public override string ToString()
        {
            return "תוצאת המבחן: " + "\t" + (isPassed ? "התלמיד הצליח." : "התלמיד נכשל.") + "\n"
                + "שמירת מרחק: " + "\t" + (DistanceKeeping ? "V" : "X") + "\n"
                + "חניה ברוורס: " + "\t" + (ReverseParking ? "V" : "X") + "\n"
                + "בדיקת מראות: " + "\t" + (MirrorsCheck ? "V" : "X") + "\n"
                + "איתות: " + "\t" + "\t" + (signals ? "V" : "X") + "\n"
                + "מהירות נכונה: " + "\t" + (CorrectSpeed ? "V" : "X") + "\n"
                + "הערות הבוחן: " + "\t" + TesterNotes + "\n";
        }
    }
}
