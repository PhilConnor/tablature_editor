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

        public void writeCharAtCursor(string keyChar)
        {
            // Concat 1, 2 or 3 depending on the write mode
            bool isWritingTwoNumber = writeMode != WriteModes.Unity && Util.isNumber(keyChar);
            if (isWritingTwoNumber)
            {
                keyChar = String.Concat((int)writeMode, keyChar);
            }

            // Set the char.
            TabCoord tabCoord = cursorController.GetCursorTopLeft;
            int cursorWidth = cursorController.GetCursorWidth;
            int cursorHeight = cursorController.GetCursorHeight;

            for (int x = tabCoord.x; x <= tabCoord.x + cursorWidth; ++x)
            {
                for (int y = tabCoord.y; y <= tabCoord.y + cursorHeight; ++y)
                {
                    tablature.setTextAt(new CanvasCoord(x, y), keyChar);
                }
            }

            moveCursor(CursorMovements.Right);
            moveCursor(CursorMovements.Right);

            if (isWritingTwoNumber)
            {
                moveCursor(CursorMovements.Right);
            }

            NotifyObserver();
        }

        public void moveCursor(CursorMovements mouvement)
        {
            switch (mouvement)
            {
                case CursorMovements.Left:
                    cursorController.MoveCursorLeft();
                    break;

                case CursorMovements.Up:
                    cursorController.MoveCursorUp();
                    break;

                case CursorMovements.Right:
                    cursorController.MoveCursorRight();
                    break;

                case CursorMovements.Down:
                    cursorController.MoveCursorDown();
                    break;

                case CursorMovements.ExpandLeft:
                    cursorController.ExpandCursorLeft();
                    break;

                case CursorMovements.ExpandUp:
                    cursorController.ExpandCursorUp();
                    break;

                case CursorMovements.ExpandRight:
                    cursorController.ExpandCursorRight();
                    break;

                case CursorMovements.ExpandDown:
                    cursorController.ExpandCursorDown();
                    break;
            }

            NotifyObserver();
        }

        public void toggleWriteMode()
        {
            switch (writeMode)
            {
                case WriteModes.Unity: writeMode = WriteModes.Tenth; break;
                case WriteModes.Tenth: writeMode = WriteModes.Twenyth; break;
                case WriteModes.Twenyth: writeMode = WriteModes.Thirtieth; break;
                case WriteModes.Thirtieth: writeMode = WriteModes.Unity; break;
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
}
