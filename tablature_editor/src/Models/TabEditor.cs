using System;
using System.Collections.Generic;
using TablatureEditor.Controllers;
using TablatureEditor.Interfaces;
using TablatureEditor.Utils;

namespace TablatureEditor.Models
{
    class TabEditor : IObserverable
    {
        public Tab tablature;
        public CursorController cursorController;
        public WriteModes writeMode;
        public SkipModes skipMode;

        public TabEditor(Tab tablature, CursorController cursorController)
        {
            this.writeMode = WriteModes.Unity;
            this.skipMode = SkipModes.One;

            this.tablature = tablature;
            this.cursorController = cursorController;

            NotifyObserver();
        }

        //@TODO : fix number over 9 by hand bug
        //@TODO : refactor this method
        public void WriteCharAtCursor(string keyChar)
        {
            keyChar = ApplyWriteMode(keyChar);

            // Fill the cursor selection with appropriate input
            TabCoord tabCoord = cursorController.UpperLeftCoord;
            for (int x = tabCoord.x; x <= tabCoord.x + cursorController.Width; ++x)
            {
                for (int y = tabCoord.y; y <= tabCoord.y + cursorController.Height; ++y)
                {
                    tablature.setTextAt(new TabCoord(x, y), keyChar);
                }
            }

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
                    cursorController.MoveLeft();
                    break;

                case CursorMovements.Up:
                    cursorController.MoveUp();
                    break;

                case CursorMovements.Right:
                    cursorController.MoveRight();
                    break;

                case CursorMovements.Down:
                    cursorController.MoveDown();
                    break;

                case CursorMovements.ExpandLeft:
                    cursorController.ExpandLeft();
                    break;

                case CursorMovements.ExpandUp:
                    cursorController.ExpandUp();
                    break;

                case CursorMovements.ExpandRight:
                    cursorController.ExpandRight();
                    break;

                case CursorMovements.ExpandDown:
                    cursorController.ExpandDown();
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
