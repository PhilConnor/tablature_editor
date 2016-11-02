using PFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Algorithms
{
    public class Transposition
    {
        /// <summary>
        /// Transpose all note currently in cursor by nSemiTones.
        /// Will increase the tablature number of staff and/or add spacings to 
        /// accomodate extra characters introduced in the tablature by a 1-digit
        /// note becoming a 2-digits
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="nSemiTones"></param>
        public static void TransposeSelection(Editor editor, int nSemiTones)
        {
            //Preparing work variables
            TabCoord cursorTopLeft = editor.Cursor.TopLeftTabCoord();
            int cursorWidth = editor.Cursor.Width;
            int cursorHeight = editor.Cursor.Height;
            int noteAddedSpace = 0;

            //Preparing looping variables
            int cursorTopLeftX = cursorTopLeft.x;
            int cursorTopLeftY = cursorTopLeft.y;
            int cursorBottomRightX = cursorTopLeft.x + cursorWidth - 1;
            int cursorBottomRightY = cursorTopLeft.y + cursorHeight - 1;

            //Transposing all notes in cursor
            for (int currentCursorX = cursorTopLeftX; currentCursorX <= cursorBottomRightX; currentCursorX++)
            {
                for (int currentCursorY = cursorTopLeftY; currentCursorY <= cursorBottomRightY; ++currentCursorY)
                {
                    TabCoord tabCoord = new TabCoord(currentCursorX, currentCursorY);
                    Element element = editor.Tablature.ElementAt(tabCoord);

                    if (element.IsNumber())
                    {
                        int val = element.GetNumericalValue();
                        bool spaceAdded = editor.Tablature.ChangeNoteAt(tabCoord, val + nSemiTones);

                        if (spaceAdded)
                        {
                            currentCursorX++;
                            noteAddedSpace++;
                            cursorBottomRightX = Math.Min(editor.TabLength - 1, ++cursorBottomRightX);
                        }
                    }
                }
            }

            //Enlarge cursor by the amount of extra space added
            editor.EnlargeCursorWidth(noteAddedSpace);
        }

    }
}
