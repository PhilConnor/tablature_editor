using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablatureEditor.Configs;
using TablatureEditor.Models;

namespace tablature_editor.Utils
{
    class CoordConverter
    {
        // Converts a tablature coord to a canvas pixel position on the canvas.
        // Used mainly to figure out where to draw the tab chars on the canvas.
        public static CanvasCoord ToCanvasCoord(TabCoord tabCoord)
        {
            CanvasCoord canvasCoord = new CanvasCoord(0, 0);

            canvasCoord.x = (tabCoord.x % Configuration.StaffLength) * Configuration.UnitSizeX;
            canvasCoord.x += Configuration.MarginX;

            canvasCoord.y = (int)Math.Floor((double)tabCoord.x / Configuration.StaffLength);
            canvasCoord.y = canvasCoord.y * (Configuration.NumberOfStrings + Configuration.CanvasStaffSpacingUnitY) + tabCoord.y;
            canvasCoord.y = canvasCoord.y * Configuration.UnitSizeY;
            canvasCoord.y += Configuration.MarginY;

            return canvasCoord;
        }

        // Converts a canvas coord to an actual position in the tablature.
        // Used mainly to track mouse position on the tablature.
        public static TabCoord ToTabCoord(CanvasCoord canvasCoord)
        {
            TabCoord tabCoord = new TabCoord(0, 0);

            // Remove margin and scale
            tabCoord.x = (int)Math.Floor((double)(canvasCoord.x - Configuration.MarginX) / Configuration.UnitSizeX);
            tabCoord.y = (int)Math.Floor((double)(canvasCoord.y - Configuration.MarginY) / Configuration.UnitSizeY);

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
