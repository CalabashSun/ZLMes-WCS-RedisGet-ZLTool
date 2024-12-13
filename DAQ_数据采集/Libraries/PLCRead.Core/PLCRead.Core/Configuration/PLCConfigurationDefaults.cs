using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead.Core.Configuration
{
    public static partial class PLCConfigurationDefaults
    {
        /// <summary>
        /// Gets the path to file that contains app settings
        /// </summary>
        public static string AppSettingsFilePath => "App_Data/appsettings.json";

        /// <summary>
        /// Gets the path to file that contains app settings for specific hosting environment
        /// </summary>
        /// <remarks>0 - Environment name</remarks>
        public static string AppSettingsEnvironmentFilePath => "App_Data/appsettings.{0}.json";
    }
}
