using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
namespace BL
{
    public class Factory
    {
        private static BLImplementation bl = new BLImplementation();
        public static IBL GetBLObj()
        {
            return bl;
        }

    }
}