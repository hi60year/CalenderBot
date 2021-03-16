using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mirai_CSharp.Models;
using Mirai_CSharp;

namespace CalenderBot
{
    static class MessageQueueListener
    {
        public static async Task<bool> Listener(MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            _ = Group.Groups.TryAdd(e.Sender.Group.Id, new Group(e.Sender.Group.Id));
            var curgp = Group.Groups[e.Sender.Group.Id];
            if (curgp.MessageQueue.Count <= curgp.MaxMessageNum)
            {
                curgp.MessageQueue.AddToBack(e);
            }
            else
            {
                curgp.MessageQueue.RemoveFromFront();
                curgp.MessageQueue.AddToBack(e);
            }
            return false;
        }
    }
}
