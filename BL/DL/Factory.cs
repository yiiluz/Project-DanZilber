using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class Factory
    {
        private static IDAL dlWithLists = DLObject.GetDLObject();

        /// <summary>
        /// Factory class to get specific type of data layer object.
        /// Singelton style (implemented at DLObject.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDAL GetDLObj(string type)
        {
            if (type == "lists") //DataSource
                return dlWithLists;
            throw new NotImplementedException("there is no such type of dl");
        }
    }
}
