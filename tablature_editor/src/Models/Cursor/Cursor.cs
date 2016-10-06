using System;

namespace PFE.Models
{
    //Contains information about cursor position.
    public class Cursor
    {
        //Properties.
        public TabCoord BaseCoord { get; set; }
        public TabCoord DragableCoord { get; set; }
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
            BaseCoord = tabCoord1;
            DragableCoord = tabCoord2;
            Logic = cursorLogic;

            //give the ref of this cursor on wich logic will be applied
            Logic.SetCursor(this);
        }

        public int Width
        {
            get { return Math.Abs(DragableCoord.x - BaseCoord.x) + 1; }
        }

        public int Height
        {
            get { return Math.Abs(DragableCoord.y - BaseCoord.y) + 1; }
        }

        public void SetPositions(TabCoord tabCoord)
        {
            BaseCoord = tabCoord.Clone();
            DragableCoord = tabCoord.Clone();
        }

        public TabCoord TopLeftCoord()
        {
            var x = Math.Min(BaseCoord.x, DragableCoord.x);
            var y = Math.Min(BaseCoord.y, DragableCoord.y);
            return new TabCoord(x, y);
        }

        public void resetPositions()
        {
            SetPositions(new TabCoord(1, 0));
        }

        public Cursor Clone()
        {
            Cursor clone = new Cursor(Logic, BaseCoord.Clone(), DragableCoord.Clone());
            return clone;
        }

        public bool Equals(Cursor c)
        {
            return BaseCoord.Equals(c.BaseCoord) && DragableCoord.Equals(c.DragableCoord);
        }
    }

    public enum CursorMovements { Left, Up, Right, Down, ExpandLeft, ExpandUp, ExpandRight, ExpandDown };
}