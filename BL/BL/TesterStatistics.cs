using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class TesterStatistics
    {
        private int numOfTests = 0;
        private int successTests = 0;
        private int failedTests = 0;
        private int futureTests = 0;

        public int NumOfTests { get => numOfTests; set => numOfTests = value; }
        public int SuccessTests { get => successTests; set => successTests = value; }
        public int FailedTests { get => failedTests; set => failedTests = value; }
        public double SuccessProportion { get => numOfTests != 0 ? (double)successTests * 100 / (successTests + failedTests) : 0; }
        public int FutureTests { get => futureTests; set => futureTests = value; }
    }
}