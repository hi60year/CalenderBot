﻿using Mirai_CSharp;
using Mirai_CSharp.Models;
using System.Threading.Tasks;

namespace CalenderBot.Commands
{
    static class ConfigUser
    {
        public static async Task Run(CommandParser.Config args, MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            if (args.ConfigInGroup == false)
            {
                await session.SendGroupMessageAsync(e.Sender.Group.Id,
                                    new PlainMessage("警告：未提供命令参数-g的情况下在群内试图配置用户信息。\n" +
                                        "为保证账号安全，停止了此操作并尝试撤回了信息。"));
                await session.RevokeMessageAsync(((SourceMessage)e.Chain[0]).Id);
                return;
            }
            await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage("正在保存..."));
            var user = new User(e.Sender.Id){ Username = args.Username, Password = args.Password};
            bool ok = await user.TrySave();
            await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage(ok ? "成功更改了配置。" : "抱歉，遇到了错误，请稍后再试。"));
        }
        public static async Task RunTemp(CommandParser.Config args, MiraiHttpSession session, ITempMessageEventArgs e)
        {
            await session.SendGroupMessageAsync(e.Sender.Group.Id, new AtMessage(e.Sender.Id), new PlainMessage("\r\n正在保存..."));
            var user = new User(e.Sender.Id) { Username = args.Username, Password = args.Password };
            bool ok = await user.TrySave();
            await session.SendGroupMessageAsync(e.Sender.Group.Id, new AtMessage(e.Sender.Id), new PlainMessage(ok ? "\r\n成功更改了配置。" : "抱歉，遇到了错误，请稍后再试。"));
        }
    }
}
