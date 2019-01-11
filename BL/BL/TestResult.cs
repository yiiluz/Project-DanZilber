using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
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
            return "Test Result: " + (isPassed ? "Passed" : "Not Passed") + "\n"
                + "Distance Keeping: " + (DistanceKeeping ? "V" : "X") + "\n"
                + "Reverse Parking: " + (ReverseParking ? "V" : "X") + "\n"
                + "Mirrors Check: " + (MirrorsCheck ? "V" : "X") + "\n"
                + "Signaling: " + (signals ? "V" : "X") + "\n"
                + "Correct Speed: " + (CorrectSpeed ? "V" : "X") + "\n"
                + "Reverse Parking: " + (ReverseParking ? "V" : "X") + "\n"
                + "Tester Notes: " + TesterNotes + "\n";
        }
    }
}
