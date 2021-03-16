using System;
using System.Collections.Generic;
using System.Text;
using Nito.Collections;
using Mirai_CSharp;
using Mirai_CSharp.Models;

namespace CalenderBot
{
    class Group
    {
        public static Dictionary<long, Group> Groups;
        public Deque<IGroupMessageEventArgs> MessageQueue { get; set; } = new Deque<IGroupMessageEventArgs>();
        public int MaxMessageNum { get; set; } = 100;
        public long GroupNum;

        static Group()
        {
            Groups = new Dictionary<long, Group>();
        }
        public Group(long gpNum)
        {
            GroupNum = gpNum;
        }
    }
}
