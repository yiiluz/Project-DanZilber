using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Converters
    {
        static IBL bl = BL.Factory.GetBLObj();
        public static DO.Tester CreateDOTester(BO.Tester other)
        {
            DO.Tester tester = new DO.Tester(other.Id);
            tester.LastName = other.LastName;
            tester.FirstName = other.FirstName;
            tester.PhoneNumber = other.PhoneNumber;
            tester.Gender = (DO.GenderEnum)other.Gender;
            tester.Address = new DO.Address(other.Address.City, other.Address.Street, other.Address.BuildingNumber);
            tester.DateOfBirth = other.DateOfBirth;
            tester.Seniority = other.Seniority;
            tester.MaxDistance = other.MaxDistance;
            tester.MaxTestsPerWeek = other.MaxTestsPerWeek;
            tester.TypeCarToTest = (DO.CarTypeEnum)other.TypeCarToTest;
            return tester;
        }
        public static DO.Trainee CreateDoTrainee(BO.Trainee other)
        {
            DO.Trainee trainee = new DO.Trainee(other.Id);
            trainee.LastName = other.LastName;
            trainee.FirstName = other.FirstName;
            trainee.SchoolName = other.SchoolName;
            trainee.TeacherName = other.TeacherName;
            trainee.PhoneNumber = other.PhoneNumber;
            trainee.Gender = (DO.GenderEnum)other.Gender;
            trainee.Address = new DO.Address(other.Address.City, other.Address.Street, other.Address.BuildingNumber);
            trainee.DateOfBirth = other.DateOfBirth;
            trainee.CurrCarType = (DO.CarTypeEnum)other.CurrCarType;
            trainee.NumOfFinishedLessons = other.NumOfFinishedLessons;
            trainee.NumOfTests = other.NumOfTests;
            trainee.IsAlreadyDidTest = other.IsAlreadyDidTest;
            return trainee;
        }
        public static DO.Test CreateDOTest(BO.Test other, string id)
        {
            var test = new DO.Test(id);
            test.TesterId = other.ExTester.Id;
            test.TraineeId = other.ExTrainee.Id;
            test.DateOfTest = other.DateOfTest;
            test.HourOfTest = other.HourOfTest;
            test.StartTestAddress = new DO.Address(other.StartTestAddress.City, other.StartTestAddress.Street, other.StartTestAddress.BuildingNumber);
            test.CarType = (DO.CarTypeEnum)other.CarType;
            test.DistanceKeeping = other.DistanceKeeping;
            test.ReverseParking = other.ReverseParking;
            test.MirrorsCheck = other.MirrorsCheck;
            test.Signals = other.Signals;
            test.CorrectSpeed = other.CorrectSpeed;
            test.IsPassed = other.IsPassed;
            test.TesterNotes = other.TesterNotes;
            test.IsTesterUpdateStatus = other.IsTesterUpdateStatus;
            return test;
        }
        public static Tester GetBOTester(DO.Tester other)
        {
            BO.Tester temp = new Tester(other.Id);
            temp.LastName = other.LastName;
            temp.FirstName = other.FirstName;
            temp.PhoneNumber = other.PhoneNumber;
            temp.Gender = (GenderEnum)other.Gender;
            temp.Address = new Address(other.Address.City, other.Address.Street, other.Address.BuildingNumber);
            temp.DateOfBirth = other.DateOfBirth;
            temp.Seniority = other.Seniority;
            temp.MaxDistance = other.MaxDistance;
            temp.MaxTestsPerWeek = other.MaxTestsPerWeek;
            temp.TypeCarToTest = (CarTypeEnum)other.TypeCarToTest;
            return temp;
        }
        public static BO.Trainee CreateBOTrainee(DO.Trainee other)
        {
            
            Trainee temp = new Trainee(other.Id);
            temp.LastName = other.LastName;
            temp.FirstName = other.FirstName;
            temp.SchoolName = other.SchoolName;
            temp.TeacherName = other.TeacherName;
            temp.PhoneNumber = other.PhoneNumber;
            temp.Gender = (GenderEnum)other.Gender;
            temp.Address = new Address(other.Address.City, other.Address.Street, other.Address.BuildingNumber);
            temp.DateOfBirth = other.DateOfBirth;
            temp.CurrCarType = (CarTypeEnum)other.CurrCarType;
            temp.NumOfFinishedLessons = other.NumOfFinishedLessons;
            temp.NumOfTests = other.NumOfTests;
            temp.IsAlreadyDidTest = other.IsAlreadyDidTest;
            temp.ExistingLicenses = new List<CarTypeEnum>();
            
            return temp;
        }
    }
}