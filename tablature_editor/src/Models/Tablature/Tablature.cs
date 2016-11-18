using System.Collections.Generic;
using System.Linq;
using PFE.Configs;
using PFE.Utils;
using PFE.Algorithms;
using System.Windows.Media;

namespace PFE.Models
{
    /// <summary>
    /// Represents the tablature itself. 
    /// Contains all tab elements and the tuning.
    /// Provided public methods protects the integretigy ot the tablature. 
    /// Example 1), trying to insert a 1234 note will result in nothing being inserted. 
    /// Example 2) Trying to insert a  4 next to a 12 will result in nothing.
    /// </summary>
    public partial class Tablature
    {
        #region properties
        /// <summary>
        /// The tab positions from left to right.
        /// Index 0 is the left most tab position.
        /// </summary>
        public List<Position> positions;

        /// <summary>
        /// The current tuning as a list of notes.
        /// </summary>
        public Tuning Tuning { get; set; }

        public SongInfo SongInfo { get; set; }

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
        /// Default Constructor
        /// </summary>
        public Tablature()
        {
            Init(4, 80, new Tuning(), new SongInfo());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nStaff">Number of initial staffs</param>
        /// <param name="staffLength">Initial lenght of staffs</param>
        /// <param name="tuning">Initial tuning</param>
        public Tablature(int nStaff, int staffLength, Tuning tuning)
        {
            Init(nStaff, staffLength, tuning, new SongInfo());
        }

        public Tablature(int nStaff, int staffLength, Tuning tuning, SongInfo songInfo)
        {
            Init(nStaff, staffLength, tuning, songInfo);
        }

        /// <summary>
        /// Inits the tablature to a black tablature with standard
        /// tuning and some other default values.
        /// </summary>
        public void Init(int nStaff, int staffLength, Tuning tuning, SongInfo songInfo)
        {
            Tuning = tuning;
            StaffLength = staffLength;
            NStaff = nStaff;

            //fills the tablature with clean blank.
            positions = new List<Position>();
            for (int x = 0; x < nStaff * staffLength; ++x)
                positions.Add(new Position(NStrings));

            positions.ElementAt(0).ParseTuning(Tuning);

            SongInfo = new SongInfo();
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
        /// Set the modifier (non-numerical char) at tabCoord
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <param name="modifierChar"></param>
        public void AttemptSetModifierCharAt(TabCoord tabCoord, char modifierChar)
        {
            if (Util.IsNumber(modifierChar))
            {
                AttemptSetNoteCharAt(tabCoord, modifierChar);
                return;
            }

            //preparing work variables
            Element lmnt = ElementAt(tabCoord);
            Element lmntOnRight = ElementAt(tabCoord.CoordOnRight());
            Element lmntOnLeft = ElementAt(tabCoord.CoordOnLeft());

            //if we are setting on the right char of num over 9
            if (lmnt.IsNoteOver9())
            {
                lmntOnLeft.ClearText();
                lmntOnLeft.RightChar = lmnt.LeftChar.Value;
                lmnt.ClearText();
                lmnt.RightChar = modifierChar;
            }
            //if we are setting on the left char of a num over 9
            else if (lmntOnRight != null && lmntOnRight.IsNoteOver9())
            {
                lmnt.ClearText();
                lmnt.RightChar = modifierChar;
                lmntOnRight.LeftChar = null;
            }
            //if we are setting over a non-num char or a num under 9
            else if (!lmnt.IsNote() || lmnt.IsNoteUnder10())
            {
                lmnt.ClearText();
                lmnt.RightChar = modifierChar;
            }
        }

        /// <summary>
        /// Attempt to set the note (numerical char) at tabCoord
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <param name="noteChar"></param>
        public void AttemptSetNoteCharAt(TabCoord tabCoord, char noteChar)
        {
            //preparing work variables
            Element lmnt = ElementAt(tabCoord);
            Element lmntOnRight = ElementAt(tabCoord.CoordOnRight());
            Element lmntOnLeft = ElementAt(tabCoord.CoordOnLeft());

            bool isANumCharOnLeft = IsANoteCharThere(tabCoord.CoordOnLeft());
            bool isANumCharOnRight = IsANoteCharThere(tabCoord.CoordOnRight());

            //if no numerical chars are surrounding this coord
            if (!isANumCharOnLeft && !isANumCharOnRight)
            {

                lmnt.ClearText();
                lmnt.RightChar = noteChar;
            }
            //if there is no num char on left and a num under 9 on right
            else if (IsElementOnRightUnder10(tabCoord)
                && !isANumCharOnLeft)
            {
                lmnt.ClearText();
                lmntOnRight.LeftChar = noteChar;
            }
            //if there is no num char on left and a num over 9 on right
            else if (IsElementOnRightOver9(tabCoord)
                && !isANumCharOnLeft)
            {
                lmnt.ClearText();
                lmntOnRight.LeftChar = noteChar;
            }
            //if there is no num char on right and a num under 9 on left
            else if (!isANumCharOnRight
                && IsElementOnLeftUnder10(tabCoord))
            {
                lmnt.LeftChar = lmntOnLeft.RightChar;
                lmntOnLeft.ClearText();
                lmnt.RightChar = noteChar;
            }
            //if there is no num char on right and a num over 9 on this coord
            else if (!isANumCharOnRight
                && lmnt.IsNoteOver9())
            {
                lmnt.RightChar = noteChar;
            }
        }


        public void AttemptSetCharAt(TabCoord tabCoord, char chr)
        {
            if (Util.IsNumber(chr))
            {
                AttemptSetModifierCharAt(tabCoord, chr);
            }
            else
            {
                AttemptSetModifierCharAt(tabCoord, chr);
            }
        }

        /// <summary>
        /// Returns the char value of the element at tabCoord
        /// </summary>
        public char GetCharAt(TabCoord tabCoord)
        {
            Element lmnt = ElementAt(tabCoord);
            Element lmntOnRight = ElementAt(tabCoord.CoordOnRight());

            if (lmntOnRight != null && lmntOnRight.IsNoteOver9())
                return lmntOnRight.LeftChar.Value;
            else
                return lmnt.RightChar;
        }


        /// <summary>
        /// Returns the element at tabCoord. 
        /// Returns Null if an invalid tabCoord has been provided.
        /// </summary>
        public Element ElementAt(TabCoord tabCoord)
        {
            if (tabCoord == null || !IsValid(tabCoord))
                return null;

            return positions.ElementAt(tabCoord.x).elements.ElementAt(tabCoord.y);
        }

        /// <summary>
        /// Inserts a new blank staff at the end of the tablature.
        /// </summary>
        public void AddNewStaff()
        {
            NStaff++;
            for (int i = 0; i < StaffLength; i++)
            {
                positions.Add(new Position(NStrings));
            }
        }

        /// <summary>
        /// Add a new string as the new biggest string if atEnd is true.
        /// Otherwise add the new string as the new smallest string.
        /// </summary>
        /// <param name="atEnd"></param>
        /// <param name="newStringNote"></param>
        public void AddString(bool atEnd, Note newStringNote)
        {
            foreach (Position p in positions)
                p.AddBlankElement(atEnd);

            Tuning.AddString(atEnd, newStringNote);
        }

        /// <summary>
        /// Attempts removing a string and preserve notes by passing them to next string if possible.
        /// </summary>
        /// <param name="atEnd"></param>
        /// <param name="attemptKeepingNotes"></param>
        public void RemoveString(bool atEnd, bool desctructive)
        {
            if (!desctructive && atEnd)
                StringChanging.MoveLastStringNotesUp(this);

            if (!desctructive && !atEnd)
                StringChanging.MoveFirstStringNotesDown(this);

            foreach (Position p in positions)
                p.RemoveElement(atEnd);

            Tuning.RemoveString(atEnd);
        }
        
        internal void ChangeTuning(Tuning newTuning, bool scaleNotes)
        {
            Retuning.AttemptRetuneTablature(this, newTuning, scaleNotes);
        }

        /// <summary>
        /// Returns true if both tablatures are equivalent.
        /// </summary>
        /// <param name="tablature"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a clone of this tablature.
        /// </summary>
        /// <returns></returns>
        public Tablature Clone()
        {
            Tablature clone = new Tablature(NStaff, StaffLength, Tuning.Clone());

            for (int i = 0; i < Length; i++)
            {
                clone.positions[i] = positions[i].Clone();
            }

            return clone;
        }

        /// <summary>
        /// Change the element to the tabCoord as a numerical value under 100.
        /// It will add spaced at appropriates places if needed to accomodate 
        /// a numerical value changing from being 1 digit to 2 digits.
        /// If there is no note (numerical char) at the element pointed by tabCoord,
        /// This method will do nothing.
        /// </summary>
        /// <param name="tabCoord"></param>
        /// <param name="note"></param>
        /// <returns>
        /// Returns true if a space has been added to accomodate a newly 
        /// added digit.
        /// </returns>
        public bool ChangeNoteAt(TabCoord tabCoord, int note)
        {
            bool spaceHasBeenAdded = false;

            Element lmnt = ElementAt(tabCoord);

            //if the element did not contain a note, we cannot change 
            // its numerical value so we do nothing          
            if (!lmnt.IsNote())
                return false;

            //prevent adding notes higher than 99 or notes lower than 0.
            note = Util.Clamp(note, 0, 99);

            Element lmntAtLeft = ElementAt(tabCoord.CoordOnLeft());
            Element lmntAtLeftLeft = ElementAt(tabCoord.CoordOnLeft().CoordOnLeft());

            //Verifying the if we need to add a spacing to accomodate a new char in the case that 
            //something is already at the location of the new char to be added.
            bool isAddingAChar = note > 9 && lmnt.IsNoteUnder10();
            bool isSomethingAtLeft = lmntAtLeft == null || lmntAtLeft != null && !lmntAtLeft.IsEmpty();
            bool isANoteBefore = lmntAtLeftLeft != null && lmntAtLeftLeft.IsNote();

            //if a note was 1 digit and is about to become two digit
            //we add a space to accomodate it
            if (isAddingAChar && isSomethingAtLeft || isAddingAChar && isANoteBefore)
            {
                InsertSpaceAt(tabCoord);
                spaceHasBeenAdded = true;
            }

            lmnt.ParseInt(note);

            return spaceHasBeenAdded;
        }
        #endregion


    }
}
