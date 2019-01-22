using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class TraineeStatistics
    {
        private int numOfTests;
        private int successTests;
        private int failedTests;

        public int NumOfTests { get => numOfTests; set => numOfTests = value; }
        public int SuccessTests { get => successTests; set => successTests = value; }
        public int FailedTests { get => failedTests; set => failedTests = value; }
        public double SuccessProportion { get => numOfTests != 0 ? (double)successTests * 100 / numOfTests : 0; }
    }
}
