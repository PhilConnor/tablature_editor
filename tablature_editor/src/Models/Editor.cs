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

        /// <summary>
        /// Instruct the editor to place the char where the cursor is.
        /// If the cursor is bigger than 1x1, it wills the whole area with the cursor.
        /// </summary>
        /// <param name="chr"></param>
        /// <TODO>Prevent triple num chars from being written.</TODO>
        /// <TODO>Fix write mode.</TODO>
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
                    if (IsWriteModeActivated() && elementOnright != null)
                    {
                        Tablature.SetElementCharAt(tabCoord, GetWriteModeCharacter().Value);
                        Tablature.SetElementCharAt(tabCoordOnRight, chr);
                    }
                    else
                    {
                        Tablature.SetElementCharAt(tabCoord,chr);
                    }
                }
            }
            
            //move the cursor to the next position.
            ApplyCursorMovementBaseOnInput(chr);

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
        public char GetElementChartAt(TabCoord tabCoord)
        {
            return Tablature.GetElementCharAt(tabCoord);
        }

        /// <summary>
        /// Gets the tabCoords currently selected bu the cursor.
        /// </summary>
        public List<TabCoord> GetSelectedTabCoords()
        {
            return Cursor.GetSelectedTabCoords();
        }

        /// <summary>
        /// Move the cursor to the bottom or top staff relative 
        /// to the current position of the cursor.
        /// </summary>
        /// <param name="goDown">true is must go to the staff down.</param>
        public void changeStaff(bool goDown)
        {
            int staffLenght = Tablature.StaffLength;

            if (goDown)
            {
                Cursor.BaseCoord.x
                    = Cursor.BaseCoord.x + staffLenght < staffLenght
                    ? Cursor.BaseCoord.x + staffLenght
                    : Cursor.BaseCoord.x;

                Cursor.DragableCoord.x
                    = Cursor.DragableCoord.x + staffLenght < staffLenght
                    ? Cursor.DragableCoord.x + staffLenght
                    : Cursor.DragableCoord.x;
            }
            else
            {
                Cursor.BaseCoord.x
                    = Cursor.BaseCoord.x - staffLenght >= 0
                    ? Cursor.BaseCoord.x - staffLenght
                    : Cursor.BaseCoord.x;

                Cursor.DragableCoord.x
                    = Cursor.DragableCoord.x - staffLenght >= 0
                    ? Cursor.DragableCoord.x - staffLenght
                    : Cursor.DragableCoord.x;
            }
        }

        //public List<int> GetStaffsTouchingNumbers()
        //{
        //    List<int> staffNumbers = new List<int>();
        //    int firstX = Cursor.TopLeftTabCoord().x;
        //    int lastX = firstX + Cursor.Width - 1;

        //    staffNumbers[0] = firstX / Tablature.TabLength;

        //    for (var i = firstX + 1; i <= lastX; i++)
        //    {
        //        int currentStaffNumber = i / Tablature.TabLength;
        //        int lastStaffNumberInArray = staffNumbers[staffNumbers.Count - 1];

        //        if (currentStaffNumber != lastStaffNumberInArray)
        //            staffNumbers.Add(currentStaffNumber);
        //    }
        //    return staffNumbers;
        //}

        public void CursorMoveUp()
        {
            if (Cursor.BaseCoord.y == 0 || Cursor.DragableCoord.y == 0)
            {
                changeStaff(false);
                return;
            }

            Cursor.BaseCoord.y--;
            Cursor.DragableCoord.y--;
        }

        public void CursorMoveLeft()
        {
            if (Cursor.BaseCoord.x == 0 || Cursor.DragableCoord.x == 0)
                return;

            Cursor.BaseCoord.x--;
            Cursor.DragableCoord.x--;
        }

        public void CursorMoveDown()
        {
            if (Cursor.BaseCoord.y == Tablature.NStrings - 1 || Cursor.DragableCoord.y == Tablature.NStrings - 1)
            {
                changeStaff(true);
                return;
            }

            Cursor.BaseCoord.y++;
            Cursor.DragableCoord.y++;
        }

        public void CursorMoveRight()
        {
            if (Cursor.BaseCoord.x >= Tablature.Length - 1 || Cursor.DragableCoord.x >= Tablature.Length - 1)
                return;

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

        //public bool IsTouchingLastStaff()
        //{
        //    return GetStaffsTouchingNumbers().IndexOf(Tablature.NStaff - 1) != -1;
        //}

        //public bool isCursorTouchingFirstStaff()
        //{
        //    return GetStaffsTouchingNumbers().IndexOf(0) != -1;
        //}

        //public bool isCursorTouchingLastString()
        //{
        //    return Math.Max(Cursor.BaseCoord.y, Cursor.DragableCoord.y) == Tablature.NStrings - 1;
        //}

        //public bool isCursorTouchingFirstString()
        //{
        //    return Math.Min(Cursor.BaseCoord.y, Cursor.DragableCoord.y) == 0;
        //}

        //public bool isCursorTouchingLastPosition()
        //{
        //    return Math.Max(Cursor.BaseCoord.x, Cursor.DragableCoord.x) == Tablature.TabLength - 1;
        //}

        //TODO: clear selection
        #endregion

        #region private
        private void ApplyCursorMovementBaseOnInput(char keyChar)
        {
            bool isWritingTwoNumber = WriteMode != WriteModes.Unity && Util.IsNumber(keyChar);

            // move cursor to the next char position
            MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            // move cursor again is input was more than one char at the same time (ex: 10)
            if (isWritingTwoNumber)
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            // move cursor again if we are in skipModes.One
            if (SkipMode == SkipModes.One)
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);
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
