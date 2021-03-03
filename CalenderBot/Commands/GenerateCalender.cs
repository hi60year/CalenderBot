using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Mirai_CSharp;
using Mirai_CSharp.Models;

namespace CalenderBot.Commands
{
    static class GenerateCalender
    {
        public static async Task Run(CommandParser.GenerateCalender args, MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            if (System.IO.File.Exists(@$"{Config.RootPath}\Users\{e.Sender.Id}\calender.json"))
            {
                try
                {
                    System.IO.File.Delete(@$"{Config.RootPath}\Users\{e.Sender.Id}\calender.json");
                }
                catch
                {
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage("原有的课程表文件存在，但删除失败，请稍后再试。" +
                        "如果这个问题反复存在，请联系管理员。"));
                    return;
                }
            }
            var process = new Process();
            process.StartInfo.FileName = "powershell";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            var user = await User.TryLoad(e.Sender.Id);
            if (!user.Configed)
            {
                await session.SendGroupMessageAsync(e.Sender.Group.Id,
                                                    new PlainMessage("您未正确配置信息，请使用cl config命令配置后再尝试生成。"));
                return;
            }
            if (user == null)
            {
                await session.SendGroupMessageAsync(e.Sender.Group.Id,
                                    new PlainMessage("无法正确加载用户信息，请稍后重试，如果该问题持续存在，请联系管理员。"));
                return;
            }
            process.Start();
            process.StandardInput.WriteLine(@$"cd {Config.CMHttpPath}");
            //process.StandardInput.WriteLine($@"python getTimeTable.py");
            process.StandardInput.WriteLine(
$@"echo ""{user.Username} {user.Password}
{Config.RootPath}\Users\{e.Sender.Id}\calender.json"" | python getTimeTable.py
");
            process.StandardInput.WriteLine("exit");
            int msgid = await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage("正在获取课表，这可能需要一定的时间。"));
            await Task.Run(
                async () => {
                    process.WaitForExit();
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, new IMessageBase[] {
                                                        new PlainMessage(
                                                            System.IO.File.Exists(@$"{Config.RootPath}\Users\{e.Sender.Id}\calender.json") ?
                                                            "课表获取成功！" :
                                                            "课表获取失败，请重试，并检查配置的用户名和密码是否正确。"
                                                        )}, msgid);
                }
            );
        }
    }
}
