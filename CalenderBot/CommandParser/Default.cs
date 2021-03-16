using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace CalenderBot.CommandParser
{
    [Verb("calenderbot", isDefault: true)]
    class Default
    {
        [Option('l', "log", SetName = "log", HelpText = "输出更新日志")]
        public bool UpdateLog { get; set; }

        [Option('n', "next", SetName = "next", HelpText = "获取下一节课信息")]
        public bool Next { get; set; }

        [Option('t', "today", SetName = "tomorrow", HelpText = "获取今天的课程信息")]
        public bool Today { get; set; }
        
        [Option("show-teacher", HelpText = "显示教课老师信息")]
        public bool ShowTeacher { get; set; }

        [Option("show-course-id", HelpText = "显示课程号")]
        public bool ShowCourseId { get; set; }

        [Option("show-class-id", HelpText = "显示班级号")]
        public bool ShowClassId { get; set; }

        [Option('d', "days-after", HelpText = "显示n天后的课程表", Default = 0)]
        public int DaysAfter { get; set; }
    }
}
