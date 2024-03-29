﻿using System;
using Mirai_CSharp.Models;
using Mirai_CSharp;
using System.Threading.Tasks;
using static CalenderBot.Config;

namespace CalenderBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var options = new MiraiHttpSessionOptions(Host, Port, AuthKey);
            await using var session = new MiraiHttpSession();
            await session.ConnectAsync(options, QQNum);
            session.GroupMessageEvt += MessageQueueListener.Listener;
            session.AddPlugin(new Commands.GroupCommandForwarder());
            session.AddPlugin(new Commands.TempCommandForwarder());
            Tests.TestRuner.Run();
            while(true)
            {
                if (await Console.In.ReadLineAsync() == "quit")
                {
                    return;
                }
            }
        }
    }
}
