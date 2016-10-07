using System;
using System.Collections.Generic;

namespace PFE.Models
{
    //Contains information about cursor position.
    public class Cursor
    {
        //Properties.
        public TabCoord BaseCoord { get; set; }
        public TabCoord DragableCoord { get; set; }

        //Constructors.
        public Cursor()
        {
            Init(new TabCoord(1, 0), new TabCoord(1, 0));
        }

        public Cursor(TabCoord tabCoord1, TabCoord tabCoord2)
        {
            Init(tabCoord1, tabCoord2);
        }

        private void Init(TabCoord tabCoord1, TabCoord tabCoord2)
        {
            BaseCoord = tabCoord1;
            DragableCoord = tabCoord2;
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
            Cursor clone = new Cursor(BaseCoord.Clone(), DragableCoord.Clone());
            return clone;
        }

        public bool Equals(Cursor c)
        {
            return BaseCoord.Equals(c.BaseCoord) && DragableCoord.Equals(c.DragableCoord);
        }

        public List<TabCoord> GetSelectedTabCoords()
        {
            List<TabCoord> touchingTabCoords = new List<TabCoord>();

            TabCoord topLeft = TopLeftCoord();
            int startX = topLeft.x;
            int startY = topLeft.y;

            for (int x = startX; x <= startX + Width - 1; x++)
            {
                for (int y = startY; y <= startY + Height - 1; y++)
                {
                    touchingTabCoords.Add(new TabCoord(x, y));
                }
            }

            return touchingTabCoords;
        }
    }

    public enum CursorMovements { Left, Up, Right, Down, ExpandLeft, ExpandUp, ExpandRight, ExpandDown };
}