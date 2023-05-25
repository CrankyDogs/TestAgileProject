using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using TestProjLarge.Entities;

namespace TestProjLarge.Infrastructures
{
    public class ConfigJSON
    {
        private static string _path = "config.json";

        public IConfiguration Configuration { get; }

        public static NavApiConfig Read()
        {
            using (StreamReader r = new StreamReader(_path))
            {
                string json = r.ReadToEnd();
                NavApiConfig item = JsonConvert.DeserializeObject<NavApiConfig>(json);
                return item;
            }
        }

        public static void write (NavApiConfig item)
        {
            string json = JsonConvert.SerializeObject(item);
            if (!string.IsNullOrEmpty(json))
            {
                File.WriteAllText(_path, json);
            }
        }
        
    }
}
