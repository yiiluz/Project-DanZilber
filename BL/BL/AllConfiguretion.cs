﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
namespace BL
{

    internal class AllConfiguretion
    {
        public readonly static bool upDate;
        private static IDAL instance;
        private static AllConfiguretion configurations = null;
        public readonly static Dictionary<string, Object> ConfiguretionsDictionary; 
        public readonly static DateTime updateDate;
        static AllConfiguretion()
        {
            try
            {
                instance = DL.Factory.GetDLObj("lists");
            }
            catch (NotImplementedException e)
            {
                throw e;
            }
            ConfiguretionsDictionary = new Dictionary<string, Object>();
            foreach (var item in instance.GetConfig())
            {
                ConfiguretionsDictionary.Add(item.Key, instance.GetConfig(item.Key));
            }
            upDate = true;
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
            foreach(var item in ConfiguretionsDictionary)
            {
                if (item.Key == s)
                {
                    return item.Value;
                }
            }
           throw new KeyNotFoundException("ERROR! There is no configuration feature with this name");
        }
    }   
}




