using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
namespace PLConsole
{
    class Program
    {
        public static void Main (string [] args)
        {
            BO.IBL bl = BL.Factory.GetBLObj();
            UIFunctions uiFunc = new UIFunctions();
            bool ok;
            int choice;
            Console.WriteLine("Enter 1 to add Tester.\nEnter 2 to add Trainee.\nEnter 3 to update Tester.\nEnter 4 to update Trainee.\n");
            ok = int.TryParse(Console.ReadLine(), out choice);
            if (ok)
            {
                switch (choice)
                {
                    case 1:
                        bl.AddTester(uiFunc.AddTester());
                        Console.WriteLine(bl.GetTestersList()[0]);
                        break;
                }
            }
        }
        
    }
}
