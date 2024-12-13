using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZLOperateTool.Services
{
    public class VersionService
    {
        public static string ReadVersionConfig(string key)
        {
            try
            {
                string configPath = "version.json";

                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    dynamic config = JsonConvert.DeserializeObject(json);
                    return  config.version;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
    }

}
