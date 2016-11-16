using System;
using PFE.Configs;

namespace PFE.Models
{
    /// <summary>
    /// Represents a 2d coordinate of a tablature element in the tablature.
    /// </summary>
    public class TabCoord : Coord
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public TabCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Returns a clone.
        /// </summary>
        /// <returns></returns>
        public TabCoord Clone()
        {
            return new TabCoord(x, y);
        }

        /// <summary>
        /// Returns the TabCoord that is appart from one unit on the left of this one.
        /// </summary>
        public TabCoord CoordOnLeft()
        {
            return new TabCoord(x - 1, y);
        }

        /// <summary>
        /// Returns the TabCoord that is appart from one unit on the right of this one.
        /// </summary>
        public TabCoord CoordOnRight()
        {
            return new TabCoord(x + 1, y);
        }




    }
}
