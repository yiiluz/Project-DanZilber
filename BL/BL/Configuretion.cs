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
    /// <summary>
    /// class to save the system configurations
    /// </summary>
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
                throw new FileLoadException("שגיאה! לא מצליח לטעון את קובץ ההגדרות.");
            }
        }
        public Object GetConfiguretion(string s)
        {
            if (ConfiguretionsDictionary[s] == null)
                throw new KeyNotFoundException("שגיאה לא קיים מאפיין קונפיגורציה בשם זה במערכת.");
            else return ConfiguretionsDictionary[s];
        }

        public static void UpdateDictonary()
        {
            ConfiguretionsDictionary = instance.GetConfig();
            lastUpdate = DateTime.Now;
        }
        public static void UpdateSerialNumber()
        {
            ConfiguretionsDictionary["מספר מבחן"] = instance.GetConfig("מספר מבחן");
        }
    }
}




