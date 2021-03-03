using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace CalenderBot.CommandParser
{   
    [Verb("config", HelpText = "对目前用户配置有关信息")]                                                     
    class Config
    {
        [Option('g', HelpText = "指定可以在群内配置")]
        public bool ConfigInGroup { get; set; }

        [Option('u', "username", Required = true, HelpText = "统一认证平台账号")]
        public string Username { get; set; }

        [Option('p', "password", Required = true, HelpText = "统一认证平台密码")]
        public string Password { get; set; }
    }
}
