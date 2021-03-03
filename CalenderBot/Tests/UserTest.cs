using System;
using System.Collections.Generic;
using System.Text;
using CalenderBot;
using static CalenderBot.Config;
using System.IO;

namespace CalenderBot.Tests
{
    static class UserTest
    {
        public static readonly string Name = "User Class";
        public static (bool ok, string message) Run()
        {
            var user = new User(1234567)
            {
                Username = "123",
                Password = "456"
            };
            user.TrySave().GetAwaiter().GetResult();
            if (!File.Exists($@"{RootPath}\Users\{user.QQNum}\data.json"))
                return (false, "文件不存在");
            else
            {
                using var reader = new StreamReader($@"{RootPath}\Users\{user.QQNum}\data.json");
                Console.WriteLine(reader.ReadToEnd().Trim());
            }
            if (User.TryLoad(user.QQNum).GetAwaiter().GetResult() == null) 
                return (false, "加载失败");

            try
            {
                User.Delete(user);
            }
            catch (DirectoryNotFoundException)
            {
                return (false, "指定的用户信息不存在");
            }
            catch (UnauthorizedAccessException)
            {
                return (false, "拒绝访问");
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }

            return (true, "test passed.");
        }
    }
}
