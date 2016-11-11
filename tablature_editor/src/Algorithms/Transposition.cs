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
        /// Will also increase cursor size so it will still be selecting the same notes.
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="nSemiTones"></param>
        public static void TransposeSelection(Editor editor, int nSemiTones)
        {
            //Preparing work variables
            TabCoord cursorTopLeft = editor.Cursor.TopLeftTabCoord();
            int cursorWidth = editor.Cursor.Width;
            int cursorHeight = editor.Cursor.Height;
            int nSpaceAdded = 0;

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

                        //Add the nSemiTones to the current value of the note at tabCoord
                        bool spaceAdded = editor.Tablature.ChangeNoteAt(tabCoord, val + nSemiTones);

                        if (spaceAdded)
                        {
                            //Keep track of the total of space added
                            nSpaceAdded++;

                            //We must skip to next currentCursorX in the loop to not increment the same element note twice
                            //becasue adding a space shifted to the right the tabCoord of the current element.
                            currentCursorX++;

                            //We must loop further, since a space has been added
                            cursorBottomRightX = Math.Min(editor.TabLength - 1, ++cursorBottomRightX);
                        }
                    }
                }
            }

            //Enlarge cursor by the amount of extra space added
            editor.EnlargeCursorWidth(nSpaceAdded);
        }

        /// <summary>
        /// Will transpose all notes on the string indicated by stringNumber by nSemiTones number of semitones.
        /// Will do nothing to notes that are impossible to transpose on the same string.
        /// Will increase staff size if new space is required because of added characters.
        /// </summary>
        /// <param name="tablature"></param>
        /// <param name="stringNumber"></param>
        /// <param name="nSemiTones"></param>
        public void TransposeString(Tablature tablature, int stringNumber, int nSemiTones)
        {


        }

        /// <summary>
        /// Will transpose all notes in the tablature by nSemiTones number of semitones.
        /// Will do nothing to notes that are impossible to transpose on the same string.
        /// Will increase staff size if new space is required because of added characters.
        /// </summary>
        /// <param name="tablature"></param>
        /// <param name="nSemiTones"></param>
        public void TransposeTablature(Tablature tablature, int nSemiTones)
        {
            //uses TransposeString(Tablature tablature, int stringNumber, int nSemiTones) for all strings
        }

    }
}
