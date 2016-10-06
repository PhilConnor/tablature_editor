using System;

namespace PFE.Models
{
    //Contains information about cursor position.
    public class Cursor
    {
        //Properties.
        public TabCoord _c1 { get; set; }
        public TabCoord _c2 { get; set; }
        public CursorLogic Logic { get; set; }

        //Constructors.
        public Cursor(CursorLogic cursorLogic)
        {
            Init(cursorLogic, new TabCoord(1, 0), new TabCoord(1, 0));
        }

        public Cursor(CursorLogic cursorLogic, TabCoord tabCoord1, TabCoord tabCoord2)
        {
            Init(cursorLogic, tabCoord1, tabCoord2);
        }

        private void Init(CursorLogic cursorLogic, TabCoord tabCoord1, TabCoord tabCoord2)
        {
            _c1 = tabCoord1;
            _c2 = tabCoord2;
            Logic = cursorLogic;

            //give the ref of this cursor on wich logic will be applied
            Logic.SetCursor(this);
        }

        public int Width
        {
            get { return Math.Abs(_c2.x - _c1.x) + 1; }
        }

        public int Height
        {
            get { return Math.Abs(_c2.y - _c1.y) + 1; }
        }

        public void setPositions(TabCoord tabCoord)
        {
            _c1 = tabCoord.Clone();
            _c2 = tabCoord.Clone();
        }

        public TabCoord TopLeftCoord()
        {
            var x = Math.Min(_c1.x, _c2.x);
            var y = Math.Min(_c1.y, _c2.y);
            return new TabCoord(x, y);
        }

        public void resetPositions()
        {
            setPositions(new TabCoord(1, 0));
        }

        public Cursor Clone()
        {
            Cursor clone = new Cursor(Logic, _c1.Clone(), _c2.Clone());
            return clone;
        }

        public bool Equals(Cursor c)
        {
            return _c1.Equals(c._c1) && _c2.Equals(c._c2);
        }
    }

    public enum CursorMovements { Left, Up, Right, Down, ExpandLeft, ExpandUp, ExpandRight, ExpandDown };
}