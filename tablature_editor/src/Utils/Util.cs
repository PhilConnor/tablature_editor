using System.Text.RegularExpressions;

namespace TablatureEditor.Utils
{
    public static class Util
    {
        public static bool isNumber(string str)
        {
            return Regex.IsMatch(str, @"^\d+$", RegexOptions.Compiled);
        }

        public static bool isNumberOver9(string str)
        {
            return Regex.IsMatch(str, @"^\d+$", RegexOptions.Compiled) && str.Length > 1;
        }
    }
}
