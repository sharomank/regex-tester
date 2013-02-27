using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharomank.RegexTester.RegexHighligter
{
    public enum RegexType
    {
        /// <summary>
        /// Неизвестный
        /// </summary>
        None,
        /// <summary>
        /// Выражение
        /// </summary>
        Expression,
        /// <summary>
        /// Группа
        /// </summary>
        Group,
        /// <summary>
        /// Конец выражения
        /// </summary>
        End
    }
}
