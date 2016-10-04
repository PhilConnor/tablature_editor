using System;
using System.Collections.Generic;
using TablatureEditor.Controllers;
using TablatureEditor.Interfaces;
using TablatureEditor.Utils;

namespace TablatureEditor.Models
{
    class TabEditor : IObserverable
    {
        public Tablature _tablature;
        public Cursor _cursor;
        public WriteModes writeMode;
        public SkipModes skipMode;

        public TabEditor(Tablature tablature, Cursor cursor)
        {
            writeMode = WriteModes.Unity;
            skipMode = SkipModes.One;

            _tablature = tablature;
            _cursor = cursor;

            NotifyObserver();
        }

        //@TODO : fix number over 9 by hand bug
        public void WriteCharAtCursor(string keyChar)
        {
            keyChar = ApplyWriteMode(keyChar);

            // Fill the cursor selection with appropriate input            
            TabCoord tabCoord = _cursor.UpperLeftCoord;
            
            for (int x = tabCoord.x; x <= tabCoord.x + _cursor.Logic.Width; ++x)
            {
                for (int y = tabCoord.y; y <= tabCoord.y + _cursor.Logic.Height; ++y)
                {
                    _tablature.setTextAt(new TabCoord(x, y), keyChar);
                }
            }
            
            _tablature.setTextAt(new TabCoord(tabCoord.x, tabCoord.y), keyChar);


            ApplyCursorMovementBaseOnInput(keyChar);

            NotifyObserver();
        }

        private void ApplyCursorMovementBaseOnInput(string keyChar)
        {
            bool isWritingTwoNumber = writeMode != WriteModes.Unity && Util.isNumber(keyChar);

            // move cursor to the next char position
            MoveCursor(CursorMovements.Right);

            // move cursor again is input was more than one char at the same time (ex: 10)
            if (isWritingTwoNumber)
                MoveCursor(CursorMovements.Right);

            // move cursor again if we are in skipModes.One
            if (skipMode == SkipModes.One)
                MoveCursor(CursorMovements.Right);
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

        public void MoveCursor(CursorMovements mouvement)
        {
            switch (mouvement)
            {
                case CursorMovements.Left:
                    _cursor.Logic.MoveLeft();
                    break;

                case CursorMovements.Up:
                    _cursor.Logic.MoveUp();
                    break;

                case CursorMovements.Right:
                    _cursor.Logic.MoveRight();
                    break;

                case CursorMovements.Down:
                    _cursor.Logic.MoveDown();
                    break;

                case CursorMovements.ExpandLeft:
                    _cursor.Logic.ExpandLeft();
                    break;

                case CursorMovements.ExpandUp:
                    _cursor.Logic.ExpandUp();
                    break;

                case CursorMovements.ExpandRight:
                    _cursor.Logic.ExpandRight();
                    break;

                case CursorMovements.ExpandDown:
                    _cursor.Logic.ExpandDown();
                    break;
            }

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
