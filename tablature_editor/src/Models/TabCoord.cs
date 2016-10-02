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

        // Converts a tablature coord to a canvas pixel position on the canvas.
        // Used mainly to figure out where to draw the tab chars on the canvas.
        public CanvasCoord ToCanvasCoord()
        {
            CanvasCoord canvasCoord = new CanvasCoord(0, 0);

            canvasCoord.x = (this.x % Configuration.StaffLength) * Configuration.UnitSizeX;
            canvasCoord.x += Configuration.MarginX;

            canvasCoord.y = (int)Math.Floor((double)this.x / Configuration.StaffLength);
            canvasCoord.y = canvasCoord.y * (Configuration.NumberOfStrings + Configuration.CanvasStaffSpacingUnitY) + this.y;
            canvasCoord.y = canvasCoord.y * Configuration.UnitSizeY;
            canvasCoord.y += Configuration.MarginY;

            return canvasCoord;
        }

        // Checks if the coord is defined and inbound.
        public static bool IsValidTabCoord(Coord coord)
        {
            var c1 = coord != null;
            var c3 = coord.x >= 0 && coord.y >= 0;
            var c4 = coord.x < (Configuration.StaffLength * Configuration.NStaff)
                && coord.y < Configuration.NumberOfStrings;

            return c1 && c3 && c4;
        }
    }
}
