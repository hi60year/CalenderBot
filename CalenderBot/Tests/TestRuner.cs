using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CalenderBot.Tests
{
    static class TestRuner
    {
        public static int SuccessNum;
        public static int FailNum;
        public static List<(Func<(bool ok, string msg)> run, string name)> Tests;
        static TestRuner()
        {
            SuccessNum = FailNum = 0;
            Tests = new List<(Func<(bool ok, string msg)> run, string name)>
            {
                (UserTest.Run, UserTest.Name)
            };
        }
        public static void Run()
        {
            Console.WriteLine("Tests started.");
            foreach (var (run, name) in Tests)
            {
                Console.WriteLine($"Testing: {name}");
                var (ok, msg) = run();
                if (ok)
                {
                    Console.Write("[  OK]");
                    SuccessNum++;
                }
                else
                {
                    Console.Write("[FAIL]");
                    FailNum++;
                }

                Console.WriteLine($" {name}:{msg}");
                Console.WriteLine("----------------------------");
                Console.WriteLine($"Test Over. Success:{SuccessNum}, Fail:{FailNum}");
            }
        }
    }
}
