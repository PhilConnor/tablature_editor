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
    class TablatureEditor : IObservable
    {
        private Tablature Tablature;
        private Cursor Cursor;
        private WriteModes WriteMode;
        private SkipModes SkipMode;

        // Number of staffs.

        public int NStrings
        {
            get
            {
                return Tablature.NStrings;
            }
        }
        public int NStaff
        {
            get
            {
                return Tablature.NStaff;
            }
        }
        public int StaffLength
        {
            get
            {
                return Tablature.StaffLength;
            }
        }

        public int TabLength
        {
            get
            {
                return Tablature.TabLength;
            }
        }

        public TablatureEditor(Tablature tablature, Cursor cursor)
        {
            WriteMode = WriteModes.Unity;
            SkipMode = SkipModes.One;

            Tablature = tablature;
            Cursor = cursor;

            NotifyObserver();
        }

        //@TODO : fix number over 9 by hand bug
        public void WriteCharAtCursor(string keyChar)
        {
            keyChar = ApplyWriteMode(keyChar);

            // Fill the cursor selection with appropriate input            
            TabCoord tabCoord = Cursor.TopLeftCoord();

            for (int x = tabCoord.x; x <= tabCoord.x + Cursor.Width - 1; ++x)
            {
                for (int y = tabCoord.y; y <= tabCoord.y + Cursor.Height - 1; ++y)
                {
                    Tablature.setTextAt(new TabCoord(x, y), keyChar);
                }
            }

            Tablature.setTextAt(new TabCoord(tabCoord.x, tabCoord.y), keyChar);

            ApplyCursorMovementBaseOnInput(keyChar);

            NotifyObserver();
        }

        public void MoveCursorWithoutNotifyingObservers(CursorMovements mouvement)
        {
            switch (mouvement)
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

        public void MoveCursor(CursorMovements mouvement)
        {
            MoveCursorWithoutNotifyingObservers(mouvement);
            NotifyObserver();
        }

        public void Select(TabCoord tabCoord)
        {
            Cursor.SetPositions(tabCoord);
            NotifyObserver();

        }

        public void SelectUpTo(TabCoord tabCoord)
        {
            Cursor.DragableCoord = tabCoord;
            NotifyObserver();
        }

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

        public string GetTextAt(TabCoord tabCoord)
        {
            return Tablature.GetTextAt(tabCoord);
        }

        public List<TabCoord> GetSelectedTabCoords()
        {
            return Cursor.GetSelectedTabCoords();
        }

        #region private
        private void ApplyCursorMovementBaseOnInput(string keyChar)
        {
            bool isWritingTwoNumber = WriteMode != WriteModes.Unity && Util.isNumber(keyChar);

            // move cursor to the next char position
            MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            // move cursor again is input was more than one char at the same time (ex: 10)
            if (isWritingTwoNumber)
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            // move cursor again if we are in skipModes.One
            if (SkipMode == SkipModes.One)
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);
        }

        private string ApplyWriteMode(string keyChar)
        {
            // Concat 1, 2 or 3 depending on the write mode
            bool isWritingTwoNumber = WriteMode != WriteModes.Unity && Util.isNumber(keyChar);
            if (isWritingTwoNumber)
            {
                keyChar = String.Concat((int)WriteMode, keyChar);
            }
            return keyChar;
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

        /////////////

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

        public List<int> GetStaffsTouchingNumbers()
        {
            List<int> staffNumbers = new List<int>();
            int firstX = Cursor.TopLeftCoord().x;
            int lastX = firstX + Cursor.Width - 1;

            staffNumbers[0] = firstX / Tablature.TabLength;

            for (var i = firstX + 1; i <= lastX; i++)
            {
                int currentStaffNumber = i / Tablature.TabLength;
                int lastStaffNumberInArray = staffNumbers[staffNumbers.Count - 1];

                if (currentStaffNumber != lastStaffNumberInArray)
                    staffNumbers.Add(currentStaffNumber);
            }
            return staffNumbers;
        }

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
            if (Cursor.BaseCoord.x >= Tablature.TabLength - 1 || Cursor.DragableCoord.x >= Tablature.TabLength - 1)
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
            Cursor.DragableCoord.x = Math.Min(++Cursor.DragableCoord.x, Tablature.TabLength - 1);
        }

        public bool IsTouchingLastStaff()
        {
            return GetStaffsTouchingNumbers().IndexOf(Tablature.NStaff - 1) != -1;
        }

        public bool isCursorTouchingFirstStaff()
        {
            return GetStaffsTouchingNumbers().IndexOf(0) != -1;
        }

        public bool isCursorTouchingLastString()
        {
            return Math.Max(Cursor.BaseCoord.y, Cursor.DragableCoord.y) == Tablature.NStrings - 1;
        }

        public bool isCursorTouchingFirstString()
        {
            return Math.Min(Cursor.BaseCoord.y, Cursor.DragableCoord.y) == 0;
        }

        public bool isCursorTouchingLastPosition()
        {
            return Math.Max(Cursor.BaseCoord.x, Cursor.DragableCoord.x) == Tablature.TabLength - 1;
        }


    }

    public enum WriteModes { Unity, Tenth, Twenyth };
    public enum SkipModes { Zero, One };
}
