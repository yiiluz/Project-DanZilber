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
        private static IBL bl;
        public static IBL GetBLObj()
        {
            if (bl == null)
                bl = new BLImplementation();
            return bl;
        }

    }
}