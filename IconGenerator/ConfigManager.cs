using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace IconGenerator
{
    class ConfigManager
    {
        public static string Count = "count";
        public static string Value = "value";
        public static List<string> load() {
            string s_count = ConfigurationManager.AppSettings[Count];
            int count=0;
            if (!string.IsNullOrEmpty(s_count))
            {
                count = Convert.ToInt32(s_count);
            }
            List<string> list = new List<string>();
            for (int i = 0; i < count; ++i) {
                list.Add(ConfigurationManager.AppSettings[Value + i]);
            }
            return list;
        }
        public static void save(params string[] values) {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings.Clear();
            int count=0;
            foreach (string value in values) {
                cfa.AppSettings.Settings.Add(Value + count, value);
                count++;
            }
            cfa.AppSettings.Settings.Add(Count, values.Length.ToString());
            cfa.Save();
        }
    }
}
