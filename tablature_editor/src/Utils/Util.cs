using System.Text.RegularExpressions;

namespace PFE.Utils
{
    public static class Util
    {
        public static bool IsNumber(string str)
        {
            return Regex.IsMatch(str, @"^\d+$", RegexOptions.Compiled);
        }

        public static bool IsNumber(char c)
        {
            return Regex.IsMatch(c.ToString(), @"^\d+$", RegexOptions.Compiled);
        }

        public static bool IsNumberOver9(string str)
        {
            return Regex.IsMatch(str, @"^\d+$", RegexOptions.Compiled) && str.Length > 1;
        }
    }
}
