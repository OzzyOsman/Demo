using System;
using System.Configuration;

namespace GitHub.Helpers
{
    public static class ConfigManager
    {
        public static string GetConnectionString(string key)
        {
            return Convert.ToString(ConfigurationManager.ConnectionStrings[key]);
        }

        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}

