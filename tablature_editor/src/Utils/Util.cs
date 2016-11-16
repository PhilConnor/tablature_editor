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

        /// <summary>
        /// Returns true if fretNumber is a valid fret number on the fret board.
        /// </summary>
        /// <param name="fretNumber"></param>
        /// <returns></returns>
        public static bool IsValidFret(int fretNumber)
        {
            return (fretNumber >= 0 && fretNumber <= 99);
        }

        public static int Clamp(int val, int minVal, int maxVal)
        {
            if (val > maxVal)
                val = maxVal;

            if (val < minVal)
                val = minVal;

            return val;
        }
    }
}
