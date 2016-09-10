using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyllan.Common
{
    public static class AppSettingHelper
    {
        private static void CheckKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
        }
        public static int GetInteger(string key, int defaultValue)
        {
            CheckKey(key);

            int value = defaultValue;
            string result = ConfigurationManager.AppSettings[key];
            int tempValue;

            if (int.TryParse(result, out tempValue))
            {
                value = tempValue;
            }
            else
            {
                value = defaultValue;
            }

            return value;
        }

        public static T GetEnum<T>(string key, T defaultValue) where T : struct
        {
            CheckKey(key);

            T value = defaultValue;

            string result = ConfigurationManager.AppSettings[key];
            T tempValue;

            if (Enum.TryParse<T>(result, out tempValue))
            {
                value = tempValue;
            }
            else
            {
                value = defaultValue;
            }

            return defaultValue;
        }

        public static bool GetBoolean(string key, bool defaultValue)
        {
            CheckKey(key);

            bool value = defaultValue;
            string result = ConfigurationManager.AppSettings[key];
            bool tempValue;

            if (bool.TryParse(result, out tempValue))
            {
                value = tempValue;
            }
            else
            {
                value = defaultValue;
            }
            return value;
        }

        public static string GetString(string key, string defaultValue)
        {
            CheckKey(key);

            string result = ConfigurationManager.AppSettings[key];

            string value = string.IsNullOrEmpty(result) ? defaultValue : result;
            return value;
        }

        public static string GetConnectionString(string connectionName = null, string defaultValue = null)
        {
            string connectionString = defaultValue;
            if (connectionName == null)
            {
                if (ConfigurationManager.ConnectionStrings.Count > 0)
                    connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            }
            else
            {
                if (ConfigurationManager.ConnectionStrings[connectionName] != null)
                    connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            }
            return connectionString;
        }

        public static void SetValue(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified, false);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
