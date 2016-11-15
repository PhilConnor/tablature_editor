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

        public bool CanAddNoteAt(TabCoord tabCoord, int note)
        {
            if (note > 9)
                return CanAddNoteOver9At(tabCoord, note);
            else
                return CanAddNoteUnder9At(tabCoord, note);
        }

        public bool CanAddNoteOver9At(TabCoord tc, int note)
        {
            if (!IsACharThere(tc)
                && !IsOnLeftEdge(tc)
                && !IsACharThere(tc.CoordOnLeft())
                && !IsANoteCharThere(tc.CoordOnLeft().CoordOnLeft())
                && !IsANoteCharThere(tc.CoordOnRight()))
                return true;
            else
                return false;
        }

        public bool CanAddNoteUnder9At(TabCoord tc, int note)
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
        /// <param name="tabCoord"></param>
        /// <returns></returns>
        public bool IsACharThere(TabCoord tabCoord)
        {
            if (ElementAt(tabCoord).RightChar != '-')
                return true;

            if (!IsOnRightEdge(tabCoord))
            {
                TabCoord tabCoordOnRight = tabCoord.CoordOnRight();
                if (ElementAt(tabCoordOnRight).IsNumberOver9())
                    return true;
            }

            return false;
        }

        #region private
        /// <summary>
        /// Returns true if a char belonging to a note element 
        /// is occupying the space at tabCoord.
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <returns></returns>
        private bool IsANoteCharThere(TabCoord tabCoord)
        {
            if (!IsValid(tabCoord))
                return false;

            if (ElementAt(tabCoord).IsNumber())
                return true;

            if (!IsOnRightEdge(tabCoord))
            {
                TabCoord tabCoordOnRight = tabCoord.CoordOnRight();
                if (ElementAt(tabCoordOnRight).IsNumberOver9())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true of the element two positions to the right this tabCoord is a note of value under 9.
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <returns></returns>
        private bool isElementOnRightUnder9(TabCoord tabCoord)
        {
            TabCoord tabCoordOnRight = tabCoord.CoordOnRight();
            if (IsValid(tabCoordOnRight) && ElementAt(tabCoordOnRight).IsNumberUnder9())
                return true;

            return false;
        }

        /// <summary>
        /// Returns true of the element to the right this tabCoord is a note of value over 9.
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <returns></returns>
        private bool isElementOnRightOver9(TabCoord tabCoord)
        {
            TabCoord tabCoordOnRight = tabCoord.CoordOnRight();
            if (IsValid(tabCoordOnRight) && ElementAt(tabCoordOnRight).IsNumberOver9())
                return true;

            return false;
        }

        /// <summary>
        /// Returns true of the element two positions to the right this tabCoord is a note of value over 9.
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <returns></returns>
        private bool isElementOnRightRightOver9(TabCoord tabCoord)
        {
            TabCoord tabCoordOnRightRight = tabCoord.CoordOnRight().CoordOnRight();
            if (IsValid(tabCoordOnRightRight) && ElementAt(tabCoordOnRightRight).IsNumberOver9())
                return true;

            return false;
        }

        /// <summary>
        /// Returns true of the element on the left of this tabCoord is a note of value under 9.
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <returns></returns>
        private bool isElementOnLeftUnder9(TabCoord tabCoord)
        {
            TabCoord tabCoordOnLeft = tabCoord.CoordOnLeft();
            if (IsValid(tabCoordOnLeft) && ElementAt(tabCoordOnLeft).IsNumberUnder9())
                return true;

            return false;
        }
        #endregion

        #region TabCoord verification
        public bool IsOnRightEdge(TabCoord tc)
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

        public bool IsOnLeftEdge(TabCoord tc)
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
