using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebApplication1.Views.Tools
{
    public class Helper
    {
        /// <summary>
        /// Es kann der Save-Methode ein Dictionary übergeben werden und es Boxen.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="myDicvtonary"></param>
        public static void Save(string path, Dictionary<int,Object> myDicvtonary)
        {
            try
            {
                string output = JsonConvert.SerializeObject(myDicvtonary);
                System.IO.File.WriteAllText(path, output);

            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                throw;
            }
         
        }



    }
}