using System;
using TablatureEditor.Configs;

namespace TablatureEditor.Models
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

        // Checks if the coord inbound.
        public bool IsValid()
        {
            var c3 = this.x >= 0 && this.y >= 0;
            var c4 = this.x < (Configuration.StaffLength * Configuration.NStaff)
                && this.y < Configuration.NumberOfStrings;

            return c3 && c4;
        }
    }
}
