using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharomank.RegexTester.RegexHighligter;

namespace RegexTesterConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RegexHighligter rh = new RegexHighligter();
            rh.Process("(test)[1231456]");
        }
    }
}
