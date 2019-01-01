using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BL;
namespace PLConsole
{
    class Program
    {
        public static void Main (string [] args)
        {
            BO.IBL bl = new BLImplementation();
            UIFunctions uiFunc = new UIFunctions();
            bool ok, flag;
            int choice, id;
            do
            {
                Console.WriteLine("Enter 1 to add Tester.\nEnter 2 to add Trainee.\nEnter 3 to add Test.\nEnter 4 to update Tester.");
                Console.WriteLine("Enter 5 to update Trainee.\nEnter 6 to update Test.\nEnter 7 to get Tester by id.\nEnter 8 to get Trainee by id.");
                Console.WriteLine("Enter 9 to get test by ID");
                ok = int.TryParse(Console.ReadLine(), out choice);
                if (ok)
                {
                    flag = false;
                    switch (choice)
                    {
                        case 1:
                            try
                            {
                                bl.AddTester(uiFunc.AddTester());
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            //Console.WriteLine((bl.GetTestersList())[0]);
                            break;
                        case 2:
                            try
                            {
                                bl.AddTrainee(uiFunc.AddTrainee());
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            Console.WriteLine(bl.GetTraineeList()[0]);
                            break;
                        case 3:

                            break;
                        case 4:

                            break;
                        case 5:

                            break;
                        case 6:

                            break;
                        case 7:
                            Console.WriteLine("Enter ID:");
                            flag = int.TryParse(Console.ReadLine(), out id);
                            if (flag)
                            {
                                try
                                {
                                    Console.WriteLine(bl.GetTesterByID(Convert.ToString(id)));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            else
                            {
                                Console.WriteLine("input was wrong.");
                            }
                            break;
                        case 8:
                            Console.WriteLine("Enter ID:");
                            flag = int.TryParse(Console.ReadLine(), out id);
                            if (flag)
                            {
                                try
                                {
                                    Console.WriteLine(bl.GetTraineeByID(Convert.ToString(id)));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            else
                            {
                                Console.WriteLine("input was wrong.");
                            }
                            break;
                        case 9:

                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Input was incorrect. try again.");
                }
            } while (choice != 0);
        }
        
    }
}
