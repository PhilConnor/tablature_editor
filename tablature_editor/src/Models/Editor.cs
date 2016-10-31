using System;
using System.Collections.Generic;
using PFE.Controllers;
using PFE.Interfaces;
using PFE.Utils;

namespace PFE.Models
{
    /// <summary>
    /// Acts like a facade, provides unified and simplified access 
    /// points to tablature and cursor models.
    /// </summary>
    class Editor : IObservable
    {
        #region properties
        private Tablature Tablature;
        private Cursor Cursor;
        private Clipboard Clipboard;
        private WriteModes WriteMode;
        private SkipModes SkipMode;

        /// <summary>
        /// Number of strings.
        /// </summary>
        public int NStrings
        {
            get { return Tablature.NStrings; }
        }

        /// <summary>
        /// Number of staffs.
        /// </summary>
        public int NStaff
        {
            get { return Tablature.NStaff; }
        }

        /// <summary>
        /// Lenght of a staff.
        /// </summary>
        public int StaffLength
        {
            get { return Tablature.StaffLength; }
        }

        /// <summary>
        /// Total lenght of the tablature.
        /// </summary>
        public int TabLength
        {
            get { return Tablature.Length; }
        }
        #endregion

        #region public
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tablature">The instance of the tablature.</param>
        /// <param name="cursor">The instance of the cursor.</param>
        public Editor(Tablature tablature, Cursor cursor)
        {
            WriteMode = WriteModes.Unity;
            SkipMode = SkipModes.One;

            Tablature = tablature;
            Cursor = cursor;

            NotifyObserver();
        }

        public void ParseAscii(string ascii)
        {
                var startCoord = Cursor.TopLeftTabCoord();
                var xLimit = TabLength;
                var yLimit = NStrings;

                var nReturn = 0;
                var nCurCharPos = 0;

                for (var i = 0; i <= ascii.Length - 1; i++)
                {
                    // if we are trying to write on an unexisting string, stop
                    if (nReturn >= yLimit)
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
                        Tablature.AddNewStaff();
                        xLimit = TabLength;
                    }

                    // write the clipboard current char on the tab
                    Tablature.AttemptSetCharAt(
                        new TabCoord(startCoord.x + nCurCharPos, startCoord.y + nReturn), 
                        ascii[i]);

                    nCurCharPos++;
                }

            NotifyObserver();
        }

        public string SelectionToAscii()
        {
            string ascii = "";

            var topLeft = Cursor.TopLeftTabCoord();

            for (var j = 0; j < Cursor.Height; j++)
            {
                for (var i = 0; i < Cursor.Width; i++)
                {
                    ascii += Tablature.GetCharAt(new TabCoord(topLeft.x + i, topLeft.y + j));
                }
                ascii += "\r\n";
            }
            ascii += "\r\n";

            return ascii;
        }

        /// <summary>
        /// Instruct the editor to place the char where the cursor is.
        /// If the cursor is bigger than 1x1, it wills the whole area with the cursor.
        /// </summary>
        /// <param name="chr"></param>
        public void WriteCharAtCursor(char chr)
        {
            TabCoord cursorTopLeftCoord = Cursor.TopLeftTabCoord();

            // Fills the cursor selection with appropriate chr.
            for (int x = cursorTopLeftCoord.x; x <= cursorTopLeftCoord.x + Cursor.Width - 1; ++x)
            {
                for (int y = cursorTopLeftCoord.y; y <= cursorTopLeftCoord.y + Cursor.Height - 1; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);
                    TabCoord tabCoordOnRight = tabCoord.CoordOnRight();
                    Element elementOnright = Tablature.ElementAt(tabCoordOnRight);

                    // if we are in 10th or 20th mode we write a 1 or 2 before the char.
                    if (Util.IsNumber(chr) && IsWriteModeActivated() && elementOnright != null)
                    {
                        Tablature.AttemptSetCharAt(tabCoord, GetWriteModeCharacter().Value);
                        Tablature.AttemptSetCharAt(tabCoordOnRight, chr);
                    }
                    else
                    {
                        Tablature.AttemptSetCharAt(tabCoord, chr);
                    }
                }
            }

            //move the cursor to the next position.
            ApplyCursorMovementBaseOnInput(chr);

            NotifyObserver();
        }

        /// <summary>
        /// Instruct the editor to clear all chars on cursor
        /// </summary>
        public void ClearCharsAtCursor()
        {
            TabCoord cursorTopLeftCoord = Cursor.TopLeftTabCoord();

            // Fills the cursor selection with appropriate chr.
            for (int x = cursorTopLeftCoord.x; x <= cursorTopLeftCoord.x + Cursor.Width - 1; ++x)
            {
                for (int y = cursorTopLeftCoord.y; y <= cursorTopLeftCoord.y + Cursor.Height - 1; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);
                    Element element = Tablature.ElementAt(tabCoord);
                    element.ClearText();
                }
            }

            NotifyObserver();
        }


        public bool IsWriteModeActivated()
        {
            return WriteMode == WriteModes.Tenth || WriteMode == WriteModes.Twenyth;
        }

        public char? GetWriteModeCharacter()
        {
            if (WriteMode == WriteModes.Tenth)
                return '1';
            else if (WriteMode == WriteModes.Twenyth)
                return '2';

            return null;
        }

        /// <summary>
        /// Applies the movementType to the cursor without notifiying observers of the editor.
        /// </summary>
        /// <param name="mouvementType"></param>
        public void MoveCursorWithoutNotifyingObservers(CursorMovements mouvementType)
        {
            switch (mouvementType)
            {
                case CursorMovements.Left:
                    CursorMoveLeft();
                    break;

                case CursorMovements.Up:
                    CursorMoveUp();
                    break;

                case CursorMovements.Right:
                    CursorMoveRight();
                    break;

                case CursorMovements.Down:
                    CursorMoveDown();
                    break;

                case CursorMovements.ExpandLeft:
                    CursorExpandLeft();
                    break;

                case CursorMovements.ExpandUp:
                    CursorExpandUp();
                    break;

                case CursorMovements.ExpandRight:
                    CursorExpandRight();
                    break;

                case CursorMovements.ExpandDown:
                    CursorExpandDown();
                    break;

                case CursorMovements.SkipStaffDown:
                    CursorMoveStaffDown();
                    break;

                case CursorMovements.SkipStaffUp:
                    CursorMoveStaffUp();
                    break;
            }
        }

        /// <summary>
        /// Applies the movementType to the cursor and notifies observers of the editor.
        /// </summary>
        /// <param name="mouvementType"></param>
        public void MoveCursor(CursorMovements mouvement)
        {
            MoveCursorWithoutNotifyingObservers(mouvement);
            NotifyObserver();
        }

        /// <summary>
        /// Makes the cursor select the area at tabCoord
        /// </summary>
        public void Select(TabCoord tabCoord)
        {
            Cursor.SetTabCoords(tabCoord);
            NotifyObserver();
        }

        /// <summary>
        /// Drags the secondary cursor tabCoord to select an area greater than 1x1.
        /// </summary>
        public void SelectUpTo(TabCoord dragableCoord)
        {
            Cursor.DragableCoord = dragableCoord;
            NotifyObserver();
        }

        /// <summary>
        /// Toggle betweens available writing modes. (units, 10th or 20th)
        /// </summary>
        public void ToggleWriteMode()
        {
            switch (WriteMode)
            {
                case WriteModes.Unity: WriteMode = WriteModes.Tenth; break;
                case WriteModes.Tenth: WriteMode = WriteModes.Twenyth; break;
                case WriteModes.Twenyth: WriteMode = WriteModes.Unity; break;
            }

            NotifyObserver();
        }

        /// <summary>
        /// Returns the TablatureElement at tabCoord.
        /// </summary>
        public char GetElementCharAt(TabCoord tabCoord)
        {
            return Tablature.GetCharAt(tabCoord);
        }

        /// <summary>
        /// Gets the tabCoords currently selected bu the cursor.
        /// </summary>
        public List<TabCoord> GetSelectedTabCoords()
        {
            return Cursor.GetSelectedTabCoords();
        }

        public void CursorMoveStaffUp()
        {
            if (!IsCursorTouchingFirstStaff())
                SkipCursorUp();
        }

        public void CursorMoveStaffDown()
        {
            if (IsCursorTouchingLastStaff())
                Tablature.AddNewStaff();

            SkipCursorDown();
        }

        public void CursorMoveUp()
        {
            if (IsCursorTouchingFirstString())
            {
                SkipCursorUp();
            }
            else
            {
                Cursor.BaseCoord.y--;
                Cursor.DragableCoord.y--;
            }
        }

        public void CursorMoveLeft()
        {
            if (!IsCursorTouchingFirstPosition())
            {
                Cursor.BaseCoord.x--;
                Cursor.DragableCoord.x--;
            }
        }

        public void CursorMoveDown()
        {
            if (IsCursorTouchingLastString() && IsCursorTouchingLastStaff())
            {
                Tablature.AddNewStaff();
                SkipCursorDown();
            }
            else if (IsCursorTouchingLastString())
            {
                SkipCursorDown();
            }
            else
            {
                Cursor.BaseCoord.y++;
                Cursor.DragableCoord.y++;
            }
        }

        public void CursorMoveRight()
        {
            if (IsCursorTouchingLastPosition())
                Tablature.AddNewStaff();

            Cursor.BaseCoord.x++;
            Cursor.DragableCoord.x++;
        }

        public void CursorExpandUp()
        {
            Cursor.DragableCoord.y = Math.Max(--Cursor.DragableCoord.y, 0);
        }

        public void CursorExpandLeft()
        {
            Cursor.DragableCoord.x = Math.Max(--Cursor.DragableCoord.x, 0);
        }

        public void CursorExpandDown()
        {
            Cursor.DragableCoord.y = Math.Min(++Cursor.DragableCoord.y, Tablature.NStrings);
        }

        public void CursorExpandRight()
        {
            Cursor.DragableCoord.x = Math.Min(++Cursor.DragableCoord.x, Tablature.Length - 1);
        }

        public void SkipCursorUp()
        {
            int staffLenght = Tablature.StaffLength;

            Cursor.BaseCoord.x
                = Cursor.BaseCoord.x - staffLenght >= 0
                ? Cursor.BaseCoord.x - staffLenght
                : Cursor.BaseCoord.x;

            Cursor.DragableCoord.x
                = Cursor.DragableCoord.x - staffLenght >= 0
                ? Cursor.DragableCoord.x - staffLenght
                : Cursor.DragableCoord.x;
        }

        public void SkipCursorDown()
        {
            int staffLenght = Tablature.StaffLength;

            Cursor.BaseCoord.x
                = Cursor.BaseCoord.x + staffLenght < Tablature.Length
                ? Cursor.BaseCoord.x + staffLenght
                : Cursor.BaseCoord.x;

            Cursor.DragableCoord.x
                = Cursor.DragableCoord.x + staffLenght < Tablature.Length
                ? Cursor.DragableCoord.x + staffLenght
                : Cursor.DragableCoord.x;
        }

        // private
        #endregion

        #region private

        private void ApplyCursorMovementBaseOnInput(char ch)
        {
            bool isWritingTwoNumber = WriteMode != WriteModes.Unity && Util.IsNumber(ch);

            // move cursor to the next char position
            MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            // move cursor again is input was more than one char at the same time (ex: 10)
            if (isWritingTwoNumber)
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            // move cursor again if we are in skipModes.One
            if (SkipMode == SkipModes.One)
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);
        }

        private List<int> GetStaffsTouchingNumbers()
        {
            List<int> staffNumbers = new List<int>();
            int firstX = Cursor.FirstXValue();
            int lastX = Cursor.LastXValue();

            for (var i = firstX; i <= lastX; i++)
            {
                int currentStaffNumber = i / Tablature.StaffLength;
                staffNumbers.Add(currentStaffNumber);
            }
            return staffNumbers;
        }

        private bool IsCursorTouchingLastStaff()
        {
            return GetStaffsTouchingNumbers().IndexOf(Tablature.NStaff - 1) != -1;
        }

        private bool IsCursorTouchingFirstStaff()
        {
            return GetStaffsTouchingNumbers().IndexOf(0) != -1;
        }

        private bool IsCursorTouchingLastString()
        {
            return Math.Max(Cursor.BaseCoord.y, Cursor.DragableCoord.y) == Tablature.NStrings - 1;
        }

        private bool IsCursorTouchingFirstString()
        {
            return Math.Min(Cursor.BaseCoord.y, Cursor.DragableCoord.y) == 0;
        }

        private bool IsCursorTouchingLastPosition()
        {
            return Math.Max(Cursor.BaseCoord.x, Cursor.DragableCoord.x) == Tablature.Length - 1;
        }

        private bool IsCursorTouchingFirstPosition()
        {
            return Math.Min(Cursor.BaseCoord.x, Cursor.DragableCoord.x) <= 0;
        }

        #endregion

        #region observer
        private List<IObserver> observers = new List<IObserver>();

        public void NotifyObserver()
        {
            observers.ForEach(o => o.Notify());
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }
        #endregion

    }

    public enum WriteModes { Unity, Tenth, Twenyth };
    public enum SkipModes { Zero, One };
}
