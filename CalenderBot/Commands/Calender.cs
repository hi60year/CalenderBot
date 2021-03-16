using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mirai_CSharp;
using Mirai_CSharp.Models;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CalenderBot.Commands
{
    static class Calender
    {
        public static async Task Run(CommandParser.Default args, MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            if (args.Next)
            {
                await GetNext(args, session, e);
            }
            else if (args.Today)
            {
                await GetToday(args, session, e);
            }
        }
        private static async Task GetNext(CommandParser.Default args, MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            using var reader = new StreamReader($@"{Config.RootPath}\Users\{e.Sender.Id}\calender.json");
            string calenderstr = await reader.ReadToEndAsync();
            Response response = null;
            try
                {response = JsonConvert.DeserializeObject<Response>(calenderstr); }
            catch(Exception ex)
                { await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage($"发生了错误：{ex.Message}")); }
            var calender = response.data;
            var now = DateTime.Now;
            int period = 0;
            switch (now.TimeOfDay)
            {
                case TimeSpan s when s < new TimeSpan( 8, 30, 0):
                    period = 0;
                    break;
                case TimeSpan s when s < new TimeSpan( 9, 15, 0):
                    period = 1;
                    break;
                case TimeSpan s when s < new TimeSpan(10, 10, 0):
                    period = 2;
                    break;
                case TimeSpan s when s < new TimeSpan(11, 15, 0):
                    period = 3;
                    break;
                case TimeSpan s when s < new TimeSpan(12, 10, 0):
                    period = 4;
                    break;
                case TimeSpan s when s < new TimeSpan(14, 15, 0):
                    period = 5;
                    break;
                case TimeSpan s when s < new TimeSpan(15, 10, 0):
                    period = 6;
                    break;
                case TimeSpan s when s < new TimeSpan(16,  5, 0):
                    period = 7;
                    break;
                case TimeSpan s when s < new TimeSpan(17, 10, 0):
                    period = 8;
                    break;
                case TimeSpan s when s < new TimeSpan(18,  5, 0):
                    period = 9;
                    break;
                case TimeSpan s when s < new TimeSpan(19, 45, 0):
                    period = 10;
                    break;
                case TimeSpan s when s < new TimeSpan(20, 40, 0):
                    period = 11;
                    break;
                case TimeSpan s when s < new TimeSpan(21, 35, 0):
                    period = 12;
                    break;
            }
            var courses = calender
                          .Where(c => c.period != null && c.teachingWeek != null
                                 && c.teachingWeek.PadRight(30, '0')[(int)((now - new DateTime(2021, 3, 1)).TotalDays/7)] == '1'
                                 && (period == 0 || c.period.PadRight(12, '0')[period-1] == '0') 
                                 && c.period.PadRight(12, '0')[period..]
                                             .Any(s => s == '1')
                                 && c.weekDay == ((int)now.DayOfWeek).ToString())
                          .ToArray();
            Array.Sort(courses, (lhs, rhs) => rhs.period.PadRight(12, '0').CompareTo(lhs.period.PadRight(12, '0')));

            if (courses.Length != 0)
            {
                await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage($"您的下一节课是：\r\n{await CourseMessageSender(args, courses[0])}"));
            }
            else
                await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage($"您今天已经没有课了哦~"));
        }

        private static async Task GetToday(CommandParser.Default args, MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            using var reader = new StreamReader($@"{Config.RootPath}\Users\{e.Sender.Id}\calender.json");
            string calenderstr = await reader.ReadToEndAsync();
            Response response = null;
            try
            { response = JsonConvert.DeserializeObject<Response>(calenderstr); }
            catch (Exception ex)
            { await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage($"发生了错误：{ex.Message}")); }
            var calender = response.data;
            var today = DateTime.Now.AddDays(args.DaysAfter);
            var courses = calender
                          .Where(c => c.teachingWeek != null
                                 && c.teachingWeek.PadRight(30, '0')[(int)((today - new DateTime(2021, 3, 1)).TotalDays / 7)] == '1'
                                 && c.weekDay == ((int)today.DayOfWeek).ToString());
            if (courses.Any())
            {
                await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage($"您{(args.DaysAfter == 0 ? "今天" : $"{args.DaysAfter}天后")}的课程列表如下:\r\n{courses.Aggregate("", (lhs, rhs) => lhs + "\r\n" + CourseMessageSender(args, rhs).GetAwaiter().GetResult())}"));
            }
            else
            {
                await session.SendGroupMessageAsync(e.Sender.Group.Id, new PlainMessage($"您{(args.DaysAfter == 0 ? "今天" : $"{ args.DaysAfter }天后")}没有课程哦~"));
            }

        }

        static async private Task<string> CourseMessageSender(CommandParser.Default args, CalenderData course)
        {
            using var tableReader = new StreamReader($@"{Config.RootPath}\periodTable.json");
            var periodTable = JsonConvert.DeserializeObject<string[][]>(await tableReader.ReadToEndAsync());
            string[] statAndEnd = course.periodFormat.Split('-', StringSplitOptions.RemoveEmptyEntries);
            var (stat, end) = (int.Parse(statAndEnd[0]), int.Parse(statAndEnd[1]));
            return
@$"
{course.courseName}
位置：{course.roomBuildingCampusName ?? "未知"}
时间：{periodTable[stat][0]}到{periodTable[end][1]}
{(args.ShowTeacher ? $"任课老师列表如下：\r\n{course.classTimetableInstrVOList.Select(teacher => $"{teacher.instructorId} {teacher.instructorName} {teacher.instrRoleCategory}").Aggregate((a, b) => a + "\r\n" + b)}" : "")}
{(args.ShowCourseId ? $"课程号：{course.courseCode}" : "")}
{(args.ShowClassId ? $"班级号：{course.classNbr}" : "")}
".Trim();
        }
    }
}
