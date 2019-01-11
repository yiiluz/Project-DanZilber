using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class Factory
    {
        /// <summary>
        /// Factory class to get specific type of data layer object..
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDAL GetDLObj(string type)
        {
            switch (type)
            {
                case "lists":
                    return DLObject.GetDLObject;
                default:
                    throw new MissingFieldException("There is no such DL object");
            }

        }
    }
}
