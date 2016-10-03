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
        
        public TabEditor(Tab tablature, CursorController cursorController)
        {
            this.writeMode = WriteModes.Unity;
            this.tablature = tablature;
            this.cursorController = cursorController;

            NotifyObserver();
        }

        public void WriteCharAtCursor(string keyChar)
        {
            // Concat 1, 2 or 3 depending on the write mode
            bool isWritingTwoNumber = writeMode != WriteModes.Unity && Util.isNumber(keyChar);
            if (isWritingTwoNumber)
            {
                keyChar = String.Concat((int)writeMode, keyChar);
            }

            // Set the char.
            TabCoord tabCoord = cursorController.UpperLeftCoord;
            int cursorWidth = cursorController.Width;
            int cursorHeight = cursorController.Height;

            for (int x = tabCoord.x; x <= tabCoord.x + cursorWidth; ++x)
            {
                for (int y = tabCoord.y; y <= tabCoord.y + cursorHeight; ++y)
                {
                    tablature.setTextAt(new CanvasCoord(x, y), keyChar);
                }
            }

            MoveCursor(CursorMovements.Right);
            MoveCursor(CursorMovements.Right);

            if (isWritingTwoNumber)
            {
                MoveCursor(CursorMovements.Right);
            }

            NotifyObserver();
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
