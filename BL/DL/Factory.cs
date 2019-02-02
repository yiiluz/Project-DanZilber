using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Factory
    {
        private static string typeOfDLObject = null;
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
                    typeOfDLObject = "lists";
                    return DL.DLObject.GetDLObject;
                case "xml":
                    typeOfDLObject = "xml";
                    return DL.DAL_XML_imp.GetAL_XML_Imp;
                default:
                    throw new MissingFieldException("There is no such DL object");
            }
        }

        public static void AddConfigUpdatedObserver(Action action)
        {
            if (typeOfDLObject != null)
            {
                if (typeOfDLObject == "xml")
                    DL.DAL_XML_imp.ConfigUpdatedAddEvent(action);
                if (typeOfDLObject == "lists")
                    DL.DLObject.ConfigUpdatedAddEvent(action);
            }
        }
    }
}
