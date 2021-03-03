using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CalenderBot.Config;

namespace CalenderBot
{
    class User
    {
        public long QQNum { get; protected set; }
        public int Privilege { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        // 这个属性由Username和Password的可空性计算而得，不需要手动设置。
        public bool Configed => Password!=null && Username!=null;
        public bool CalenderGenerated { get; set; }
        public User(long qqnum, int privilege = 1)
        {
            QQNum = qqnum;
            Privilege = privilege;
        }
        
        public async Task<bool> TrySave()
        {
            var path = $@"{RootPath}\Users\{QQNum}";
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception)
            {
                return false;
            }
            using var writer = new StreamWriter(path + @"\data.json");
            await writer.WriteLineAsync(JsonConvert.SerializeObject(this, Formatting.Indented));
            return true;
        }

        public static async Task<User> TryLoad(long QQNum)
        {
            var path = $@"{RootPath}\Users\{QQNum}";
            try
            {
                if (!Directory.Exists(path))
                    return null;
                else
                {
                    using var reader = new StreamReader(path + @"\data.json");
                    return JsonConvert.DeserializeObject<User>(await reader.ReadToEndAsync());
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        // 这个操作需要管理员进行。
        // 非异步方法，可能会抛出异常。
        public static void Delete(long QQNum)
        {
            var userDirInfo = new DirectoryInfo($@"{RootPath}\Users\{QQNum}");
            userDirInfo.Delete(recursive:true);
        }

        // 这个操作需要管理员进行。
        // 非异步方法，可能会抛出异常。
        public static void Delete(User user)
        {
            Delete(user.QQNum);
        }
    }
}
