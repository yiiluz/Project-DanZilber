using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.IO;
using DO;
namespace BL
{

    internal class AllConfiguretion
    {
        public readonly static bool upDate;
        private static IDAL instance;
        private static AllConfiguretion configurations = null;
        public readonly static Dictionary<string, Object> ConfiguretionsDictionary;
        static AllConfiguretion()
        {
            try
            {
                instance = DO.Factory.GetDLObj("xml");
                ConfiguretionsDictionary = instance.GetConfig();                                
                upDate = true;
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
           
        }
        public static AllConfiguretion ConfigurationFactory()
        {
            if (configurations == null)
            {
                configurations = new AllConfiguretion();
            }
            return configurations;
        }
        public Object GetConfiguretion(string s)
        {
            foreach (var item in ConfiguretionsDictionary)
            {
                if (item.Key == s)
                {
                    return item.Value;
                }
            }
            throw new KeyNotFoundException("ERROR! There is no configuration feature with this name. from AllConfig");
        }
        public void UpdateSerialNumber()
        {
            ConfiguretionsDictionary["Serial Number Test"] = instance.GetConfig("Serial Number Test");
        }
    }
}




