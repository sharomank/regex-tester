using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Sharomank.RegexTester.Common
{
    public class KeyboardHelper
    {
        public static bool IsKeyDown(KeyEventArgs e, Key key)
        {
            bool result = e.KeyboardDevice.IsKeyDown(key);
            if (result == true)
            {
                e.Handled = true;
            }
            return result;
        }

        public static bool IsKeyDown(KeyEventArgs e, Key key, params bool[] preconditions)
        {
            if (preconditions.All(c => c == true))
            {
                return IsKeyDown(e, key);
            }
            return false;
        }
    }
}
