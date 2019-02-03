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

    public class Configuretion
    {
        public readonly static bool isConfigUpdated;
        private static IDAL instance;
        private static DateTime lastUpdate;
        public static Dictionary<string, Object> ConfiguretionsDictionary;

        public static DateTime LastUpdate { get => lastUpdate; }
        static Configuretion()
        {
            try
            {
                instance = DO.Factory.GetDLObj("xml");
                UpdateDictonary();
            }
            catch(Exception)
            {
                throw new FileLoadException("Error. Can't Load The Configuration File");
            }
        }

        public Object GetConfiguretion(string s)
        {
            if (ConfiguretionsDictionary[s] == null)
                throw new KeyNotFoundException("ERROR! There is no configuration feature with this name. from AllConfig");
            else return ConfiguretionsDictionary[s];
        }

        public static void UpdateDictonary()
        {
            ConfiguretionsDictionary = instance.GetConfig();
            lastUpdate = DateTime.Now;
        }
        public static void UpdateSerialNumber()
        {
            ConfiguretionsDictionary["Serial Number Test"] = instance.GetConfig("Serial Number Test");
        }
    }
}




