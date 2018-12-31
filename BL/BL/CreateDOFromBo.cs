using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CreateDOFromBO
    {
        public static DO.Tester CreateDOTester(BO.Tester other)
        {
            DO.Tester tester = new DO.Tester(other.Id);
            tester.LastName = other.LastName;
            tester.FirstName = other.FirstName;
            tester.SchoolName = other.SchoolName;
            tester.TeacherName = other.TeacherName;
            tester.PhoneNumber = other.PhoneNumber;
            tester.Gender = (DO.GenderEnum)other.Gender;
            tester.Address = new DO.Address(other.Address.City, other.Address.Street, other.Address.BuildingNumber);
            tester.DateOfBirth = other.DateOfBirth;
            tester.Seniority = other.Seniority;
            tester.MaxDistance = other.MaxDistance;
            tester.MaxTestsPerWeek = other.MaxTestsPerWeek;
            tester.TypeCarToTest = (DO.CarTypeEnum)other.TypeCarToTest;
            tester.AvailableWorkTime = other.AvailableWorkTime;
            foreach (var item in other.TestList)
                tester.TestList.Add(CreateDOTest(item));
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
            trainee.LastTest = new DateTime(other.LastTest.Ticks);
            trainee.CurrCarType = (DO.CarTypeEnum)other.CurrCarType;
            trainee.CurrGearType = (DO.GearboxTypeEnum)other.CurrGearType;
            trainee.NumOfFinishedLessons = other.NumOfFinishedLessons;
            trainee.NumOfTests = other.NumOfTests;
            trainee.IsAlreadyDidTest = other.IsAlreadyDidTest;
            trainee.ExistingLicenses = new List<DO.CarTypeEnum>();
            foreach (var item in other.ExistingLicenses)
                trainee.ExistingLicenses.Add((DO.CarTypeEnum)item);
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
    }
}