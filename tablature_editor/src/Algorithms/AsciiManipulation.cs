using PFE.Models;
using PFE.Utils;
using System;
using System.Collections.Generic;
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
            return null;
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
