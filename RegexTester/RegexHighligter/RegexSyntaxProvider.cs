using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharomank.RegexTester.RegexHighligter
{
    public static class RegexSyntaxProvider
    {
        private static List<string> strs = new List<string>{
                "\\S", "\\s",
                "\\W", "\\w",
                "\\D", "\\d",
                "\\B", "\\b",
                "\\t", "\\n", "\\r", "\\v", "\\f", "\\0"
        };

        private static List<char> specialChars = new List<char> { '^', '$', '*', '+', '?', '|', '.' };

        private static List<char> bracketsBegin = new List<char> { '(', '[', '{' };
        private static List<char> bracketsEnd = new List<char> { ')', ']', '}' };

        public enum BracketType
        {
            /// <summary>
            /// Фигурные скобки { }
            /// </summary>
            Braces,
            /// <summary>
            /// Квадратные скобки [ ]
            /// </summary>
            Square,
            /// <summary>
            /// Скобки ( )
            /// </summary>
            Brackets
        }

        public static List<string> GetCharacterClasses
        {
            get { return strs; }
        }

        public static List<char> GetSpecialChars
        {
            get { return specialChars; }
        }

        public static List<char> GetBeginBrackets
        {
            get { return bracketsBegin; }
        }

        public static List<char> GetEndBrackets
        {
            get { return bracketsEnd; }
        }

        public static BracketType GetBracketType(char ch)
        {
            if (ch == '{' || ch == '}')
                return BracketType.Braces;
            if (ch == '[' || ch == ']')
                return BracketType.Square;
                
            return BracketType.Brackets;
        }
    }
}