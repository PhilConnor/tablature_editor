using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tablature_editor.src
{
    class Util
    {
        public static bool isNumber(string str)
        {
            return Regex.IsMatch(str, @"^\d+$", RegexOptions.Compiled);
        }
    }
}
