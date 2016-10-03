using System;
using TablatureEditor.Configs;

namespace TablatureEditor.Models
{
    //Coordinates in the main canvas of the application.
    public class CanvasCoord : Coord
    {
        //Constructors.
        public CanvasCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // Converts a canvas coord to an actual position in the tablature.
        // Used mainly to track mouse position on the tablature.
        public TabCoord toTabCoord()
        {
            TabCoord tabCoord = new TabCoord(0, 0);

            // Remove margin and scale
            tabCoord.x = (int)Math.Floor((double)(this.x - Configuration.MarginX) / Configuration.UnitSizeX);
            tabCoord.y = (int)Math.Floor((double)(this.y - Configuration.MarginY) / Configuration.UnitSizeY);

            // If position is on a blankspace between staffs, make it on last string
            if (tabCoord.y % (Configuration.NumberOfStrings + 1) == Configuration.NumberOfStrings)
            {
                tabCoord.y = tabCoord.y - 1;
            }

            // If position is out of bound, null
            if (tabCoord.y < 0
                || tabCoord.y >= (Configuration.NumberOfStrings + 1) * Configuration.NStaff - 1
                || tabCoord.x < 0
                || tabCoord.x >= Configuration.StaffLength)
            {
                return null;
            }

            // Convert to x = char position, y = string number
            tabCoord.x = (int)(tabCoord.x + ((Configuration.StaffLength * Math.Floor((double)tabCoord.y / (Configuration.NumberOfStrings + 1)))));
            tabCoord.y = tabCoord.y % (Configuration.NumberOfStrings + 1);

            return tabCoord;
        }
    }
}
