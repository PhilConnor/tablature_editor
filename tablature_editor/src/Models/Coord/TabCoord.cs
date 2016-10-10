using System;
using PFE.Configs;

namespace PFE.Models
{
    //Coordinates in the tablature.
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

        public TabCoord CoordOnLeft()
        {
            return new TabCoord(x - 1, y);
        }

        public TabCoord CoordOnRight()
        {
            return new TabCoord(x + 1, y);
        }

        //Checks if the coord inbound the tablature.
        public bool IsValid(Tablature tablature)
        {
            bool c1 = x >= 0 && y >= 0;
            bool c2 = x < tablature.StaffLength * tablature.NStaff;
            bool c3 = y < tablature.NStrings;

            return c1 && c2 && c3;
        }
    }
}
