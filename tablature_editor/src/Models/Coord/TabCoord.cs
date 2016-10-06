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

        // Checks if the coord inbound.
        //public bool IsValid()
        //{
        //    var c3 = this.x >= 0 && this.y >= 0;
        //    var c4 = this.x < (Config_Tab.StaffLength * Config_Tab.NStaff)
        //        && this.y < Config_Tab.NumberOfStrings;

        //    return c3 && c4;
        //}
    }
}
