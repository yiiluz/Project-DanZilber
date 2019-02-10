using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class TesterStatistics used to show statistics and counters for specific trainee.
    /// </summary>
    public class TraineeStatistics
    {
        private int numOfTests;
        private int successTests;
        private int failedTests;

        public int NumOfTests { get => numOfTests; set => numOfTests = value; }
        public int SuccessTests { get => successTests; set => successTests = value; }
        public int FailedTests { get => failedTests; set => failedTests = value; }
        public double SuccessProportion { get => numOfTests != 0 && (successTests + failedTests) != 0 ? (double)successTests * 100 / (successTests + failedTests) : 0; }
    }
}
