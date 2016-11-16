using PFE.Models;
using PFE.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Algorithms
{
    /// <summary>
    /// Retuning related algorithms.
    /// </summary>
    public static class AsciiManipulation
    {
        public static Tablature TablatureFromAscii(string ascii)
        {
            List<string> strList = new List<string>();
            StringReader strReader = new StringReader(ascii);
            string line;

            while (true)
            {
                line = strReader.ReadLine();

                if (line != null)
                    strList.Add(line);
                else if (line == null)
                    break;
                else if (line == "\r\n")
                    strList.Add("\r\n");
            }

            return TablatureFromStringList(strList);
        }

        public static Tablature TablatureFromStringList(List<string> sl)
        {
            string tt = sl[3].Split(new[] { ": " }, StringSplitOptions.None)[1].TrimEnd(new[] { '\r' });
            Tuning tuning = Tuning.ParseString(tt);
            SongInfo songInfo = SongInfo.FromStringList(sl.GetRange(0, 3));
            
            sl.RemoveRange(0, 5);

            int nStaff = (sl.Count - 1) / tuning.notes.Count();
            Tablature tablature = new Tablature(nStaff, 80, tuning, songInfo);

            TabCoord tc;
            int y = 0;
            int nS = 0;

            foreach (string l in sl)
            {
                if (l != "")
                {
                    for (int x = 0; x < 80; x++)
                    {
                        tablature.AttemptSetCharAt(new TabCoord(x + (nS * 80), y), l[x]);
                    }

                    y++;
                }
                else
                {
                    y = 0;
                    nS++;
                }
            }

            return tablature;
        }

        public static string AsciiFromTablature(Tablature tablature)
        {
            int x = 0;
            int s = 0;
            int y = 0;

            string ascii = "";

            List<string> listString = new List<string>();

            for (; s < tablature.NStaff; s++)
            {
                for (; y < tablature.NStrings; y++)
                {
                    string str = "";
                    for (; x < tablature.StaffLength; x++)
                    {
                        str += tablature.GetCharAt(new TabCoord(s * tablature.StaffLength + x, y));
                    }
                    x = 0;
                    ascii += str + "\r\n";
                }
                y = 0;
                ascii += "\r\n";
            }

            return ascii;
        }

        public static List<string> StringListFromTablature(Tablature tablature)
        {
            int x = 0;
            int s = 0;
            int y = 0;

            List<string> listString = new List<string>();

            for (; s < tablature.NStaff; s++)
            {
                for (; y < tablature.NStrings; y++)
                {
                    string str = "";
                    for (; x < tablature.StaffLength; x++)
                    {
                        str += tablature.GetCharAt(new TabCoord(s * tablature.StaffLength + x, y));
                    }
                    x = 0;
                    listString.Add(str);
                }
                y = 0;
            }

            return listString;
        }

        public static void PasteAsciiAtCursor(Editor editor, string ascii)
        {
            var startCoord = editor.Cursor.TopLeftTabCoord();
            var xLimit = editor.TabLength;
            var yLimit = editor.NStrings;

            var nReturn = 0;
            var nCurCharPos = 0;

            for (var i = 0; i <= ascii.Length - 1; i++)
            {
                // if we are trying to write on an unexisting string, stop
                if (startCoord.y + nReturn >= yLimit)
                    break;

                // if we are at a line return, we
                if (ascii[i] == '\r')
                {
                    i++; // skipping the \n after the \r
                    nCurCharPos = 0;
                    nReturn++;
                    continue;
                }

                // if we are about to write outsite the xLimit, we add a new staff before
                // and get the new xLimit
                if (startCoord.x + nCurCharPos >= xLimit)
                {
                    editor.Tablature.AddNewStaff();
                    xLimit = editor.TabLength;
                }

                // write the clipboard current char on the tab
                editor.Tablature.AttemptSetModifierCharAt(
                    new TabCoord(startCoord.x + nCurCharPos, startCoord.y + nReturn),
                    ascii[i]);

                nCurCharPos++;
            }
        }

        public static string SelectionToAscii(Editor editor)
        {
            string ascii = "";

            var topLeft = editor.Cursor.TopLeftTabCoord();

            for (var j = 0; j < editor.Cursor.Height; j++)
            {
                for (var i = 0; i < editor.Cursor.Width; i++)
                {
                    ascii += editor.Tablature.GetCharAt(new TabCoord(topLeft.x + i, topLeft.y + j));
                }
                ascii += "\r\n";
            }
            ascii += "\r\n";

            return ascii;
        }
    }
}
