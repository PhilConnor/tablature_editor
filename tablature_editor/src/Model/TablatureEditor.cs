using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using tablature_editor.src.Interfaces;
using static tablature_editor.src.Enums;

namespace tablature_editor.src
{
    class TablatureEditor : src.Interfaces.IObserverable
    {
        public Tab _tablature;
        public Cursor _cursor;
        public WriteModes _writeMode;
        
        public TablatureEditor(Tab tablature, Cursor cursor)
        {
            _writeMode = WriteModes.Unity;

            _tablature = tablature;
            _cursor = cursor;

            NotifyObserver();
        }

        public void writeCharAtCursor(string keyChar)
        {
            // concat 1, 2 or 3 depending on the write mode
            bool isWritingTwoNumber = _writeMode != WriteModes.Unity && Util.isNumber(keyChar);
            if (isWritingTwoNumber)
                keyChar = String.Concat((int)_writeMode, keyChar);

            //set the char
            Coord coord = _cursor.getTopLeftCoord();
            int cursorWidth = _cursor.getWidth() - 1;
            int cursorHeight = _cursor.getHeight() - 1;

            for (int x = coord.x; x <= coord.x + cursorWidth; x++)
            {
                for (int y = coord.y; y <= coord.y + cursorHeight; y++)
                    _tablature.setTextAt(new Coord(x, y), keyChar);
            }

            moveCursor(CursorMovements.Right);
            moveCursor(CursorMovements.Right);
            if (isWritingTwoNumber)
                moveCursor(CursorMovements.Right);

            NotifyObserver();
        }

        public void moveCursor(CursorMovements mouvement)
        {
            switch (mouvement)
            {
                case CursorMovements.Left://left
                    _cursor.moveLeft();
                    break;

                case CursorMovements.Up://up
                    //if (_cursor.isTouchingLastStaff() && this.tablature.isLastStaffEmpty() && _cursor.isTouchingFirstString())
                    //    _tablature.removeLastStaff();
                    _cursor.moveUp();
                    break;

                case CursorMovements.Right://right
                    //if (_cursor.isTouchingLastStaff() && _cursor.isTouchingLastLine())
                    //    _tablature.addNewStaff();
                    _cursor.moveRight();
                    break;

                case CursorMovements.Down://down
                    //if (_cursor.isTouchingLastStaff() && _cursor.isTouchingLastString())
                    //    _tablature.addNewStaff();
                    _cursor.moveDown();
                    break;

                case CursorMovements.ExpandLeft: //expandLeft
                    _cursor.expandLeft();
                    break;

                case CursorMovements.ExpandUp://expandUp
                    _cursor.expandUp();
                    break;

                case CursorMovements.ExpandRight://expandRight
                    _cursor.expandRight();
                    break;

                case CursorMovements.ExpandDown://expandDown
                    _cursor.expandDown();
                    break;
            }

            NotifyObserver();
        }

        public void toggleWriteMode()
        {
            switch (_writeMode)
            {
                case WriteModes.Unity: _writeMode = WriteModes.Tenth; break;
                case WriteModes.Tenth: _writeMode = WriteModes.Twenyth; break;
                case WriteModes.Twenyth: _writeMode = WriteModes.Thirtieth; break;
                case WriteModes.Thirtieth: _writeMode = WriteModes.Unity; break;
            }

            NotifyObserver();
        }

        private List<IObserver> _observers = new List<IObserver>();
        public void NotifyObserver()
        {
            _observers.ForEach(o => o.Notify());
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }
    }
}
