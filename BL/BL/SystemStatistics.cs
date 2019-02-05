using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class SystemStatistics
    {
        static int numOfTestWaitForUpdate = 0;
        static int numOfAbortedTests = 0;
        static int numOfSuccessedTests = 0;
        static int numOfFailedTest = 0;
        // static double averageNumOfTestToSuccess = 0;
        static int sumTesterDistanceToTest = 0;
        static int sumNumOfTestsPerWeek = 0;
        static int numOfTraineesMotorCycle = 0;
        static int numOfTraineesPrivateCar = 0;
        static int numOfTraineesAutoPrivateCar = 0;
        static int numOfTraineesTruckUnlimited = 0;
        static int numOfTraineesTruck12Ton = 0;
        static int numOfTraineesBus = 0;
        static int numOfTestersMotorCycle = 0;
        static int numOfTestersPrivateCar = 0;
        static int numOfTestersAutoPrivateCar = 0;
        static int numOfTestersTruckUnlimited = 0;
        static int numOfTestersTruck12Ton = 0;
        static int numOfTestersBus = 0;

        static int numOfTestsMotorCycle = 0;
        static int numOfTestsPrivateCar = 0;
        static int numOfTestsAutoPrivateCar = 0;
        static int numOfTestsTruckUnlimited = 0;
        static int numOfTestsTruck12Ton = 0;
        static int numOfTestsBus = 0;

        public static int NumOfTrainees
        {
            get => numOfTraineesMotorCycle + numOfTraineesPrivateCar + numOfTraineesAutoPrivateCar
                    + numOfTraineesTruckUnlimited + numOfTraineesTruck12Ton + numOfTraineesBus;
        }
        public static int NumOfTesters
        {
            get => numOfTestersMotorCycle + numOfTestersPrivateCar + numOfTestersAutoPrivateCar
                    + numOfTestersTruckUnlimited + numOfTestersTruck12Ton + numOfTestersBus;
        }
        public static int NumOfTests
        {
            get => numOfTestsMotorCycle + numOfTestsPrivateCar + numOfTestsAutoPrivateCar
                    + numOfTestsTruckUnlimited + numOfTestsTruck12Ton + numOfTestsBus;
        }

        public static int NumOfSuccessedTests { get => numOfSuccessedTests; set => numOfSuccessedTests = value; }
        public static int NumOfFailedTest { get => numOfFailedTest; set => numOfFailedTest = value; }

        public static double AverageNumOfTestToSuccess { get => NumOfTests != 0 ? NumOfSuccessedTests / NumOfTests : 0; }
        public static double AverageTesterDistanceToTest { get => NumOfTesters != 0 ? sumTesterDistanceToTest / NumOfTesters : 0; }
        public static double AverageNumOfTestsPerWeek { get => NumOfTesters != 0 ? sumNumOfTestsPerWeek / NumOfTesters : 0; }
        public static int NumOfTraineesMotorCycle { get => numOfTraineesMotorCycle; set => numOfTraineesMotorCycle = value; }
        public static int NumOfTraineesPrivateCar { get => numOfTraineesPrivateCar; set => numOfTraineesPrivateCar = value; }
        public static int NumOfTraineesAutoPrivateCar { get => numOfTraineesAutoPrivateCar; set => numOfTraineesAutoPrivateCar = value; }
        public static int NumOfTraineesTruckUnlimited { get => numOfTraineesTruckUnlimited; set => numOfTraineesTruckUnlimited = value; }
        public static int NumOfTraineesTruck12Ton { get => numOfTraineesTruck12Ton; set => numOfTraineesTruck12Ton = value; }
        public static int NumOfTraineesBus { get => numOfTraineesBus; set => numOfTraineesBus = value; }
        public static int NumOfTestersMotorCycle { get => numOfTestersMotorCycle; set => numOfTestersMotorCycle = value; }
        public static int NumOfTestersPrivateCar { get => numOfTestersPrivateCar; set => numOfTestersPrivateCar = value; }
        public static int NumOfTestersAutoPrivateCar { get => numOfTestersAutoPrivateCar; set => numOfTestersAutoPrivateCar = value; }
        public static int NumOfTestersTruckUnlimited { get => numOfTestersTruckUnlimited; set => numOfTestersTruckUnlimited = value; }
        public static int NumOfTestersTruck12Ton { get => numOfTestersTruck12Ton; set => numOfTestersTruck12Ton = value; }
        public static int NumOfTestersBus { get => numOfTestersBus; set => numOfTestersBus = value; }
        public static int NumOfTestsMotorCycle { get => numOfTestsMotorCycle; set => numOfTestsMotorCycle = value; }
        public static int NumOfTestsPrivateCar { get => numOfTestsPrivateCar; set => numOfTestsPrivateCar = value; }
        public static int NumOfTestsAutoPrivateCar { get => numOfTestsAutoPrivateCar; set => numOfTestsAutoPrivateCar = value; }
        public static int NumOfTestsTruckUnlimited { get => numOfTestsTruckUnlimited; set => numOfTestsTruckUnlimited = value; }
        public static int NumOfTestsTruck12Ton { get => numOfTestsTruck12Ton; set => numOfTestsTruck12Ton = value; }
        public static int NumOfTestsBus { get => numOfTestsBus; set => numOfTestsBus = value; }
        public static int NumOfTestWaitForUpdate { get => numOfTestWaitForUpdate; set => numOfTestWaitForUpdate = value; }
        public static int NumOfAbortedTests { get => numOfAbortedTests; set => numOfAbortedTests = value; }
        public static int SumTesterDistanceToTest { get => sumTesterDistanceToTest; set => sumTesterDistanceToTest = value; }
        public static int SumNumOfTestsPerWeek { get => sumNumOfTestsPerWeek; set => sumNumOfTestsPerWeek = value; }
    }
}
