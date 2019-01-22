using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Factory
    {
        /// <summary>
        /// Factory class to get specific type of data layer object..
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DO.IDAL GetDLObj(string type)
        {
            switch (type)
            {
                case "lists":
                    return DL.DLObject.GetDLObject;
                    break;
                case "xml":
                    return DL.DAL_XML_imp.GetAL_XML_Imp;
                    break;
                default:
                    throw new MissingFieldException("There is no such DL object");
            }

        }
    }
}
