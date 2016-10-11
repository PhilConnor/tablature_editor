using System;
using System.Collections.Generic;

namespace PFE.Models
{
    /// <summary>
    /// Represents the selected elements on the tablature.
    /// </summary>
    public class Cursor
    {
        #region properties
        /// <summary>
        /// The main main coord of the cursor where selection starts.
        /// Cannot be dragged.
        /// </summary>
        public TabCoord BaseCoord { get; set; }

        /// <summary>
        /// The secondary coord of the cursor where selection ends.
        /// Can be dragged.
        /// </summary>
        public TabCoord DragableCoord { get; set; }

        /// <summary>
        /// The width of the cursor as calculated from coords.
        /// </summary>
        public int Width
        {
            get { return Math.Abs(DragableCoord.x - BaseCoord.x) + 1; }
        }
        
        /// <summary>
        /// The height of the cursor as calculated from coords.
        /// </summary>
        public int Height
        {
            get { return Math.Abs(DragableCoord.y - BaseCoord.y) + 1; }
        }
        #endregion

        #region public
        /// <summary>
        /// Default constructor, creates a 1x1 cursor at [1,0]
        /// </summary>
        public Cursor()
        {
            SetTabCoords(new TabCoord(1, 0));
        }

        /// <summary>
        /// Constructor, creates a ?x? cursor at [tabCoord1,tabCoord2]
        /// </summary>
        public Cursor(TabCoord tabCoord1, TabCoord tabCoord2)
        {
            BaseCoord = tabCoord1;
            DragableCoord = tabCoord2;
        }
        
        /// <summary>
        /// Sets both cursor tabCoords to clone of the input tabCoord.
        /// </summary>
        public void SetTabCoords(TabCoord tabCoord)
        {
            BaseCoord = tabCoord.Clone();
            DragableCoord = tabCoord.Clone();
        }

        /// <summary>
        /// Returns the coord that correspond to the top left coord of the 2d cursor.
        /// </summary>
        public TabCoord TopLeftTabCoord()
        {
            int x = Math.Min(BaseCoord.x, DragableCoord.x);
            int y = Math.Min(BaseCoord.y, DragableCoord.y);
            return new TabCoord(x, y);
        }
        
        /// <summary>
        /// Returns a clone instance.
        /// </summary>
        public Cursor Clone()
        {
            Cursor clone = new Cursor(BaseCoord.Clone(), DragableCoord.Clone());
            return clone;
        }
        
        /// <summary>
        /// Returns true if the cursor has the same values.
        /// </summary>
        public bool Equals(Cursor c)
        {
            return BaseCoord.Equals(c.BaseCoord) && DragableCoord.Equals(c.DragableCoord);
        }


        /// <summary>
        /// Returns all tabCoords in located in the cursor area.
        /// </summary>
        public List<TabCoord> GetSelectedTabCoords()
        {
            List<TabCoord> touchingTabCoords = new List<TabCoord>();

            TabCoord topLeft = TopLeftTabCoord();
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
        #endregion
    }

    public enum CursorMovements { Left, Up, Right, Down, ExpandLeft, ExpandUp, ExpandRight, ExpandDown };
}