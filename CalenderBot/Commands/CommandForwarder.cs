using CommandLine;
using Mirai_CSharp;
using Mirai_CSharp.Models;
using Mirai_CSharp.Plugin.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace CalenderBot.Commands
{
    class GroupCommandForwarder : IGroupMessage
    {
        public async Task<bool> GroupMessage(MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            if (e.Chain.Length != 2 || !(e.Chain[1] is PlainMessage))
                return false;
            var cmd = ((PlainMessage)e.Chain[1]).Message.Split(' ').Where(s => s != string.Empty).ToArray();
            if (!CalenderBot.Config.CommandStart.Contains(cmd[0]))
                return false;
            var helpMsg = new StringBuilder();
            using var writer = new StringWriter(helpMsg);
            var parser = new Parser(config => config.HelpWriter = writer);
            await parser.ParseArguments<CommandParser.Default,
                                                CommandParser.Config,
                                                CommandParser.GenerateCalender>(cmd.Skip(1))
                                                .WithParsedAsync<CommandParser.Config>(async with => await ConfigUser.Run(with, session, e)).GetAwaiter().GetResult()
                                                .WithParsedAsync<CommandParser.GenerateCalender>(async with => await GenerateCalender.Run(with, session, e)).GetAwaiter().GetResult()
                                                .WithParsedAsync<CommandParser.Default>(async with => await Calender.Run(with, session, e));

            if(helpMsg.Length != 0)
            {
                await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage(helpMsg.ToString()));
            }
            return false;
        }
    }
    class TempCommandForwarder : ITempMessage
    {
        public async Task<bool> TempMessage(MiraiHttpSession session, ITempMessageEventArgs e)
        {
            if (e.Chain.Length != 2 || !(e.Chain[1] is PlainMessage))
                return false;
            var cmd = ((PlainMessage)e.Chain[1]).Message.Split(' ').Where(s => s != string.Empty).ToArray();
            if (!CalenderBot.Config.CommandStart.Contains(cmd[0]))
                return false;
            var helpMsg = new StringBuilder();
            using var writer = new StringWriter(helpMsg);
            var parser = new Parser(config => config.HelpWriter = writer);
            await parser.ParseArguments<CommandParser.Default,
                                                CommandParser.Config,
                                                CommandParser.GenerateCalender>(cmd.Skip(1))
                                                .WithParsedAsync<CommandParser.Config>(async with => await ConfigUser.RunTemp(with, session, e));

            if (helpMsg.Length != 0)
            {
                await session.SendGroupMessageAsync(e.Sender.Group.Id, new IMessageBase[]{ new AtMessage(e.Sender.Id) , new PlainMessage("\r\n"+helpMsg.ToString()) });
            }
            return false;
        }
    }
}
