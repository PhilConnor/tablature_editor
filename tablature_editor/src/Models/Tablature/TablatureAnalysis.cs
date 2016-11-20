using System.Collections.Generic;
using System.Linq;
using PFE.Configs;
using PFE.Utils;
using PFE.Algorithms;
using System.Windows.Media;
using System;

namespace PFE.Models
{
    /// <summary>
    /// See other Tablature partial class for information.
    /// </summary>
    public partial class Tablature
    {
        #region Elements analysis related.
        /// <summary>
        /// Returns true if a the note can be setted at this position.
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool CanSetNoteAt(TabCoord tc, int note)
        {
            if (note > 9)
                return CanSetNoteOver9At(tc, note);
            else
                return CanSetNoteUnder10At(tc, note);
        }

        /// <summary>
        /// Returns true if a note over 9 can be setted at tc.
        /// Is there is no room to place it there, returns false.
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool CanSetNoteOver9At(TabCoord tc, int note)
        {
            if (!IsACharThere(tc)
                && !IsOnFirstPosition(tc)
                && !IsACharThere(tc.CoordOnLeft())
                && !IsANoteCharThere(tc.CoordOnLeft().CoordOnLeft())
                && !IsANoteCharThere(tc.CoordOnRight()))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if a note under 10 can be setted at tc.
        /// Is there is no room to place it there, returns false.
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool CanSetNoteUnder10At(TabCoord tc, int note)
        {
            if (!IsACharThere(tc)
                && !IsANoteCharThere(tc.CoordOnLeft())
                && !IsANoteCharThere(tc.CoordOnRight()))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if an element is a note or modifier character is located at this tabCoord.
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        public bool IsACharThere(TabCoord tc)
        {
            if (ElementAt(tc).RightChar != '-')
                return true;

            if (!IsOnLastPosition(tc))
            {
                TabCoord tabCoordOnRight = tc.CoordOnRight();
                if (ElementAt(tabCoordOnRight).IsNoteOver9())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if a char belonging to a note element 
        /// is occupying the space at tabCoord.
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        private bool IsANoteCharThere(TabCoord tc)
        {
            if (!IsValid(tc))
                return false;

            if (ElementAt(tc).IsNote())
                return true;

            if (!IsOnLastPosition(tc))
            {
                TabCoord tabCoordOnRight = tc.CoordOnRight();
                if (ElementAt(tabCoordOnRight).IsNoteOver9())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true of the element two positions to the right this tabCoord is a note of value under 10.
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        private bool IsElementOnRightUnder10(TabCoord tc)
        {
            TabCoord tabCoordOnRight = tc.CoordOnRight();
            if (IsValid(tabCoordOnRight) && ElementAt(tabCoordOnRight).IsNoteUnder10())
                return true;

            return false;
        }

        /// <summary>
        /// Returns true of the element to the right this tabCoord is a note of value over 9.
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        private bool IsElementOnRightOver9(TabCoord tc)
        {
            TabCoord tabCoordOnRight = tc.CoordOnRight();
            if (IsValid(tabCoordOnRight) && ElementAt(tabCoordOnRight).IsNoteOver9())
                return true;

            return false;
        }

        /// <summary>
        /// Returns true of the element two positions to the right this tabCoord is a note of value over 9.
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        private bool IsElementOnRightRightOver9(TabCoord tc)
        {
            TabCoord tabCoordOnRightRight = tc.CoordOnRight().CoordOnRight();
            if (IsValid(tabCoordOnRightRight) && ElementAt(tabCoordOnRightRight).IsNoteOver9())
                return true;

            return false;
        }

        /// <summary>
        /// Returns true of the element on the left of this tabCoord is a note of value under 10.
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        private bool IsElementOnLeftUnder10(TabCoord tc)
        {
            TabCoord tabCoordOnLeft = tc.CoordOnLeft();
            if (IsValid(tabCoordOnLeft) && ElementAt(tabCoordOnLeft).IsNoteUnder10())
                return true;

            return false;
        }


        #endregion

        #region TabCoord analysis related
        /// <summary>
        /// True if this tabCoord is at the right edge of the tablature.
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        public bool IsOnLastPosition(TabCoord tc)
        {
            if (!IsValid(tc))
                return false;

            if (!IsValid(tc.CoordOnRight()))
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if this coord is valid in the tablature.
        /// (valid = not negatives && not out of bound)
        /// </summary>
        public bool IsValid(TabCoord tc)
        {
            bool c1 = tc.x >= 0 && tc.y >= 0;
            bool c2 = tc.x < this.Length;
            bool c3 = tc.y < this.NStrings;

            return c1 && c2 && c3;
        }

        /// <summary>
        /// True if this tabCoord is at the first position of the tablature.
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        public bool IsOnFirstPosition(TabCoord tc)
        {
            if (!IsValid(tc))
                return false;

            if (!IsValid(tc.CoordOnLeft()))
                return true;

            return false;
        }
        #endregion
    }
}
