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
    class TablatureEditor : IObserverable
    {
        private Tablature Tablature;
        private Cursor Cursor;

        public WriteModes writeMode;
        public SkipModes skipMode;

        public TablatureEditor(Tablature tablature, Cursor cursor)
        {
            writeMode = WriteModes.Unity;
            skipMode = SkipModes.One;

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

        private void ApplyCursorMovementBaseOnInput(string keyChar)
        {
            bool isWritingTwoNumber = writeMode != WriteModes.Unity && Util.isNumber(keyChar);

            // move cursor to the next char position
            MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            // move cursor again is input was more than one char at the same time (ex: 10)
            if (isWritingTwoNumber)
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            // move cursor again if we are in skipModes.One
            if (skipMode == SkipModes.One)
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);
        }

        private string ApplyWriteMode(string keyChar)
        {
            // Concat 1, 2 or 3 depending on the write mode
            bool isWritingTwoNumber = writeMode != WriteModes.Unity && Util.isNumber(keyChar);
            if (isWritingTwoNumber)
            {
                keyChar = String.Concat((int)writeMode, keyChar);
            }
            return keyChar;
        }

        public void MoveCursorWithoutNotifyingObservers(CursorMovements mouvement)
        {
            switch (mouvement)
            {
                case CursorMovements.Left:
                    Cursor.Logic.MoveLeft();
                    break;

                case CursorMovements.Up:
                    Cursor.Logic.MoveUp();
                    break;

                case CursorMovements.Right:
                    Cursor.Logic.MoveRight();
                    break;

                case CursorMovements.Down:
                    Cursor.Logic.MoveDown();
                    break;

                case CursorMovements.ExpandLeft:
                    Cursor.Logic.ExpandLeft();
                    break;

                case CursorMovements.ExpandUp:
                    Cursor.Logic.ExpandUp();
                    break;

                case CursorMovements.ExpandRight:
                    Cursor.Logic.ExpandRight();
                    break;

                case CursorMovements.ExpandDown:
                    Cursor.Logic.ExpandDown();
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
            switch (writeMode)
            {
                case WriteModes.Unity: writeMode = WriteModes.Tenth; break;
                case WriteModes.Tenth: writeMode = WriteModes.Twenyth; break;
                case WriteModes.Twenyth: writeMode = WriteModes.Unity; break;
            }

            NotifyObserver();
        }

        public string GetTextAt(TabCoord tabCoord)
        {
            return Tablature.GetTextAt(tabCoord);
        }

        public List<TabCoord> GetSelectedTabCoords()
        {
            return Cursor.Logic.GetSelectedTabCoords();
        }
        
        private List<IObserver> observers = new List<IObserver>();
        public void NotifyObserver()
        {
            observers.ForEach(o => o.Notify());
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }
    }

    public enum WriteModes { Unity, Tenth, Twenyth };
    public enum SkipModes { Zero, One };
}
