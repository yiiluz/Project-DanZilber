using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
namespace BO
{
    /// <summary>
    /// BL factory. just return bl instance.
    /// </summary>
    public class Factory
    {
        private static BLImplementation bl = new BLImplementation();
        public static IBL GetBLObj()
        {
            return bl;
        }

    }
}