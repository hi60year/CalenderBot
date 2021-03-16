using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CalenderBot
{
    static class Config
    {
        public static readonly string Host;
        public static readonly int Port;
        public static readonly string AuthKey;
        public static readonly string RootPath;
        public static readonly string CMHttpPath;
        public static readonly long Administrator;
        public static readonly long QQNum;
        public static readonly string[] CommandStart;

        static Config()
        {
            using var reader = new StreamReader("D:\\CalenderBot\\CalenderBot\\config.json");
            var configData = (JObject)JsonConvert.DeserializeObject(reader.ReadToEndAsync().GetAwaiter().GetResult());
            Host = (string)configData["Host"];
            Port = (int)configData["Port"];
            AuthKey = (string)configData["AuthKey"];
            RootPath = (string)configData["RootPath"];
            CMHttpPath = (string)configData["CMHttpPath"];
            Administrator = (long)configData["Administrator"];
            QQNum = (long)configData["QQNum"];
            CommandStart = ((JArray)configData["CommandStart"]).ToObject<string[]>();
        }
    }
}
