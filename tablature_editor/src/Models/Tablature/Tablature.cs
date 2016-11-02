using System.Collections.Generic;
using System.Linq;
using PFE.Configs;
using PFE.Utils;

namespace PFE.Models
{
    /// <summary>
    /// Represents the tablature itself. 
    /// Contains all tab elements and the tuning.
    /// </summary>
    public class Tablature
    {
        #region properties
        /// <summary>
        /// The tab positions from left to right.
        /// Index 0 is the left most tab position.
        /// </summary>
        public List<Position> positions;

        /// <summary>
        /// The current tuning as a string.
        /// </summary>
        public Tuning Tuning { get; set; }

        /// <summary>
        /// The number of strings.
        /// </summary>
        public int NStrings { get { return Tuning.GetNumberOfString(); } }

        /// <summary>
        /// The number of staffs in the tablature.
        /// </summary>
        public int NStaff { get; set; }

        /// <summary>
        /// The length of a staff.
        /// </summary>
        public int StaffLength { get; set; }

        /// <summary>
        /// The total length of the tablature.
        /// </summary>
        public int Length { get { return positions.Count(); } }
        #endregion

        #region public
        /// <summary>
        /// Constructor
        /// </summary>
        public Tablature()
        {
            Init(3, 80, new Tuning());
        }

        public Tablature(int nStaff, int staffLength, Tuning tuning)
        {
            Init(nStaff, staffLength, tuning);
        }

        /// <summary>
        /// Inits the tablature to a black tablature with standard
        /// tuning and some other default values.
        /// </summary>
        public void Init(int nStaff, int staffLength, Tuning tuning)
        {
            Tuning = tuning;
            StaffLength = staffLength;
            NStaff = nStaff;

            //fills the tablature with clean blank.
            positions = new List<Position>();
            for (int x = 0; x < nStaff * staffLength; ++x)
                positions.Add(new Position(NStrings));

            positions.ElementAt(0).ParseTuning(Tuning);
        }

        /// <summary>
        /// Inserts a space at tabCoordX position and 
        /// shift all elements to the right.
        /// If this makes an elements getting out of bound of the tablature, 
        /// it will att a new staff to hold it.
        /// </summary>
        /// <param name="tabCoord"></param>
        public void InsertSpaceAt(TabCoord tabCoord)
        {
            int x = tabCoord.x;
            Position positionAtX = positions[x];
            Position tmpPosition;

            for (int i = x; i < Length; i++)
            {
                bool isTabEnd = (i == Length - 1);

                if (isTabEnd && !positions[i].IsEmpty())
                    AddNewStaff();

                tmpPosition = positions[i];
                positions[i] = positionAtX;
                positionAtX = tmpPosition;
            }
            positions[x] = positions[x].Clone();
            positions[x].Clear();
        }

        /// <summary>
        /// Set the non numerical char at tabCoord
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <param name="ch"></param>
        public void AttemptSetCharAt(TabCoord tabCoord, char ch)
        {
            //exit if invalid coord
            if (tabCoord == null || !tabCoord.IsValid(this))
                return;

            if (Util.IsNumber(ch))
            {
                AttemptSetNumericalCharAt(tabCoord, ch);
                return;
            }

            //preparing work variables
            Element lmnt = ElementAt(tabCoord);
            Element lmntOnRight = ElementAt(tabCoord.CoordOnRight());
            Element lmntOnLeft = ElementAt(tabCoord.CoordOnLeft());

            //if we are setting on the right char of num over 9
            if (lmnt.IsNumberOver9())
            {
                lmntOnLeft.ClearText();
                lmntOnLeft.RightChar = lmnt.LeftChar;
                lmnt.ClearText();
                lmnt.RightChar = ch;
            }
            //if we are setting on the left char of a num over 9
            else if (lmntOnRight != null && lmntOnRight.IsNumberOver9())
            {
                lmnt.ClearText();
                lmnt.RightChar = ch;
                lmntOnRight.LeftChar = '-';
            }
            //if we are setting over a non-num char or a num under 9
            else if (!lmnt.IsNumber() || lmnt.IsNumberUnder9())
            {
                lmnt.ClearText();
                lmnt.RightChar = ch;
            }
        }

        /// <summary>
        /// Set the numerical char at tabCoord
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <param name="ch"></param>
        public void AttemptSetNumericalCharAt(TabCoord tabCoord, char ch)
        {
            //exit if invalid coord
            if (tabCoord == null || !tabCoord.IsValid(this))
                return;

            //preparing work variables
            Element lmnt = ElementAt(tabCoord);
            Element lmntOnRight = ElementAt(tabCoord.CoordOnRight());
            Element lmntOnLeft = ElementAt(tabCoord.CoordOnLeft());

            bool isANumCharOnLeft = isANumericalCharThere(tabCoord.CoordOnLeft());
            bool isANumCharOnRight = isANumericalCharThere(tabCoord.CoordOnRight());

            //if no numerical chars are surrounding this coord
            if (!isANumCharOnLeft && !isANumCharOnRight)
            {
                lmnt.ClearText();
                lmnt.RightChar = ch;
            }
            //if there is no num char on left and a num under 9 on right
            else if (isElementOnRightUnder9(tabCoord)
                && !isANumCharOnLeft)
            {
                lmnt.ClearText();
                lmntOnRight.LeftChar = ch;
            }
            //if there is no num char on left and a num over 9 on right
            else if (isElementOnRightOver9(tabCoord)
                && !isANumCharOnLeft)
            {
                lmnt.ClearText();
                lmntOnRight.LeftChar = ch;
            }
            //if there is no num char on right and a num under 9 on left
            else if (!isANumCharOnRight
                && isElementOnLeftUnder9(tabCoord))
            {
                lmnt.LeftChar = lmntOnLeft.RightChar;
                lmntOnLeft.ClearText();
                lmnt.RightChar = ch;
            }
            //if there is no num char on right and a num over 9 on this coord
            else if (!isANumCharOnRight
                && lmnt.IsNumberOver9())
            {
                lmnt.RightChar = ch;
            }
        }

        /// <summary>
        /// Set the element to the tabCoord as a numerical value under 100.
        /// It will add spaced at appropriates places if needed to accomodate 
        /// a numerical value changing from being 1 digit to 2 digits.
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <param name="note"></param>
        /// <returns>
        /// Returns true if a space has been added to accomodate a newly 
        /// added digit.
        /// </returns>
        //public bool AttemptSetNumericalAt(TabCoord tabCoord, int note)
        //{
        //    //exit if invalid coord
        //    if (tabCoord == null || !tabCoord.IsValid(this))
        //        return;

        //    //prevent adding notes higher than 99
        //    if (note >= 100)
        //        note = 99;



            //preparing work variables
            //Element lmnt = ElementAt(tabCoord);
            //Element lmntOnRight = ElementAt(tabCoord.CoordOnRight());
            //Element lmntOnLeft = ElementAt(tabCoord.CoordOnLeft());

            //bool isANumCharOnLeft = isANumericalCharThere(tabCoord.CoordOnLeft());
            //bool isANumCharOnRight = isANumericalCharThere(tabCoord.CoordOnRight());

            //if no numerical chars are surrounding this coord
            //if (!isANumCharOnLeft && !isANumCharOnRight)
            //{
            //    lmnt.ClearText();
            //    lmnt.RightChar = ch;
            //}
            //if there is no num char on left and a num under 9 on right
            //else if (isElementOnRightUnder9(tabCoord)
            //    && !isANumCharOnLeft)
            //{
            //    lmnt.ClearText();
            //    lmntOnRight.LeftChar = ch;
            //}
            //if there is no num char on left and a num over 9 on right
            //else if (isElementOnRightOver9(tabCoord)
            //    && !isANumCharOnLeft)
            //{
            //    lmnt.ClearText();
            //    lmntOnRight.LeftChar = ch;
            //}
            //if there is no num char on right and a num under 9 on left
            //else if (!isANumCharOnRight
            //    && isElementOnLeftUnder9(tabCoord))
            //{
            //    lmnt.LeftChar = lmntOnLeft.RightChar;
            //    lmntOnLeft.ClearText();
            //    lmnt.RightChar = ch;
            //}
            //if there is no num char on right and a num over 9 on this coord
            //else if (!isANumCharOnRight
            //    && lmnt.IsNumberOver9())
            //{
            //    lmnt.RightChar = ch;
            //}
      //  }




        /// <summary>
        /// Returns the char value of the element at tabCoord
        /// </summary>
        public char GetCharAt(TabCoord tabCoord)
        {
            Element lmnt = ElementAt(tabCoord);
            Element lmntOnRight = ElementAt(tabCoord.CoordOnRight());

            if (lmntOnRight != null && lmntOnRight.IsNumberOver9())
                return lmntOnRight.LeftChar;
            else
                return lmnt.RightChar;
        }

        public bool isACharThere(TabCoord tabCoord)
        {
            if (ElementAt(tabCoord).RightChar != '-')
                return true;

            if (!tabCoord.IsOnRightEdge(this))
            {
                TabCoord tabCoordOnRight = tabCoord.CoordOnRight();
                if (ElementAt(tabCoordOnRight).IsNumberOver9())
                    return true;
            }

            return false;
        }

        public bool isANumericalCharThere(TabCoord tabCoord)
        {
            if (!tabCoord.IsValid(this))
                return false;

            if (ElementAt(tabCoord).IsNumber())
                return true;

            if (!tabCoord.IsOnRightEdge(this))
            {
                TabCoord tabCoordOnRight = tabCoord.CoordOnRight();
                if (ElementAt(tabCoordOnRight).IsNumberOver9())
                    return true;
            }

            return false;
        }

        public bool isElementOnRightUnder9(TabCoord tabCoord)
        {
            TabCoord tabCoordOnRight = tabCoord.CoordOnRight();
            if (tabCoordOnRight.IsValid(this) && ElementAt(tabCoordOnRight).IsNumberUnder9())
                return true;

            return false;
        }

        public bool isElementOnRightOver9(TabCoord tabCoord)
        {
            TabCoord tabCoordOnRight = tabCoord.CoordOnRight();
            if (tabCoordOnRight.IsValid(this) && ElementAt(tabCoordOnRight).IsNumberOver9())
                return true;

            return false;
        }

        public bool isElementOnRightRightOver9(TabCoord tabCoord)
        {
            TabCoord tabCoordOnRightRight = tabCoord.CoordOnRight().CoordOnRight();
            if (tabCoordOnRightRight.IsValid(this) && ElementAt(tabCoordOnRightRight).IsNumberOver9())
                return true;

            return false;
        }

        public bool isElementOnLeftUnder9(TabCoord tabCoord)
        {
            TabCoord tabCoordOnLeft = tabCoord.CoordOnLeft();
            if (tabCoordOnLeft.IsValid(this) && ElementAt(tabCoordOnLeft).IsNumberUnder9())
                return true;

            return false;
        }

        /// <summary>
        /// Returns the element at tabCoord
        /// </summary>
        public Element ElementAt(TabCoord tabCoord)
        {
            if (tabCoord == null || !tabCoord.IsValid(this))
                return null;

            return positions.ElementAt(tabCoord.x).elements.ElementAt(tabCoord.y);
        }

        public void AddNewStaff()
        {
            NStaff++;
            for (int i = 0; i < StaffLength; i++)
            {
                positions.Add(new Position(NStrings));
            }
        }

        public bool Equals(Tablature tablature)
        {
            if (tablature.Tuning != Tuning)
                return false;

            if (tablature.NStaff != NStaff)
                return false;

            if (tablature.StaffLength != StaffLength)
                return false;

            for (int i = 0; i < tablature.Length; i++)
            {
                if (!tablature.positions[i].Equals(positions[i]))
                    return false;
            }

            return true;
        }

        public Tablature Clone()
        {
            Tablature clone = new Tablature(NStaff, StaffLength, Tuning.Clone());

            for (int i = 0; i < Length; i++)
            {
                clone.positions[i] = positions[i].Clone();
            }

            return clone;
        }

        #endregion

        #region private

        #endregion
    }
}
