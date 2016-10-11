using System;
using PFE.Configs;

namespace PFE.Models
{
    /// <summary>
    /// Represents a 2d coordinate of a tablature element in the tablature.
    /// </summary>
    public class TabCoord : Coord
    {
        //Constructors.
        public TabCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

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
        
        /// <summary>
        /// Returns the TabCoord that is appart from two units on the right of this one.
        /// </summary>
        public TabCoord CoordOnRightRight()
        {
            return new TabCoord(x + 2, y);
        }

        /// <summary>
        /// Returns true if this coord is valid in the tablature.
        /// (valid = not negatives && not out of bound)
        /// </summary>
        public bool IsValid(Tablature tablature)
        {
            bool c1 = x >= 0 && y >= 0;
            bool c2 = x < tablature.Length;
            bool c3 = y < tablature.NStrings; 

            return c1 && c2 && c3;
        }
    }
}
