// Guids.cs
// MUST match guids.h
using System;

namespace Sharomank.RegexTester
{
    static class GuidList
    {
        public const string guidRegexTesterPkgString = "dcad2d98-1e91-4090-81a0-ec0641347bf8";
        public const string guidRegexTesterCmdSetString = "78a72454-296c-4156-8e32-a91a3cec11a5";
        public const string guidToolWindowPersistanceString = "25c278ac-0bc4-4884-8513-3bd56ee7af18";

        public static readonly Guid guidRegexTesterCmdSet = new Guid(guidRegexTesterCmdSetString);
    };
}