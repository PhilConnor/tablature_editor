using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFE.Configs;
using PFE.Models;

namespace tablature_editor.Utils
{
    class CoordConverter
    {
        // Converts a tablature coord to a canvas pixel position on the canvas.
        // Used mainly to figure out where to draw the tab chars on the canvas.
        public static CanvasCoord ToCanvasCoord(TabCoord tabCoord, TablatureEditor tablatureEditor)
        {
            CanvasCoord canvasCoord = new CanvasCoord(0, 0);

            canvasCoord.x = (tabCoord.x % tablatureEditor.StaffLength) * Config_DrawSurface.Inst().GridUnitWidth;
            canvasCoord.x += Config_DrawSurface.Inst().MarginX;

            canvasCoord.y = (int)Math.Floor((double)tabCoord.x / tablatureEditor.StaffLength);
            canvasCoord.y = canvasCoord.y *
                (tablatureEditor.NStrings + Config_DrawSurface.Inst().SpacingBetweenStaff) + tabCoord.y;
            canvasCoord.y = canvasCoord.y * Config_DrawSurface.Inst().GridUnitHeight;
            canvasCoord.y += Config_DrawSurface.Inst().MarginY;

            return canvasCoord;
        }

        // Converts a canvas coord to an actual position in the tablature.
        // Used mainly to track mouse position on the tablature.
        public static TabCoord ToTabCoord(CanvasCoord canvasCoord, TablatureEditor tablatureEditor)
        {
            TabCoord tabCoord = new TabCoord(0, 0);

            // Remove margin and scale
            tabCoord.x = (int)Math.Floor((double)(canvasCoord.x - Config_DrawSurface.Inst().MarginX)
                / Config_DrawSurface.Inst().GridUnitWidth);
            tabCoord.y = (int)Math.Floor((double)(canvasCoord.y - Config_DrawSurface.Inst().MarginY)
                / Config_DrawSurface.Inst().GridUnitHeight);


            //TODO: Implement support for bigger spacing between staffs (+1-1)
            //   If position is on a blankspace between staffs, make it on last string
            if (tabCoord.y % (tablatureEditor.NStrings + 1) == tablatureEditor.NStrings)
            {
                tabCoord.y = tabCoord.y - 1;
            }

            // If position is out of bound, null
            if (tabCoord.y < 0
                || tabCoord.y >= (tablatureEditor.NStrings + 1) * tablatureEditor.NStaff - 1
                || tabCoord.x < 0
                || tabCoord.x >= tablatureEditor.StaffLength)
            {
                return null;
            }

            // Convert to x = char position, y = string number
            tabCoord.x 
                = (int)(tabCoord.x
                + ((tablatureEditor.StaffLength
                * Math.Floor((double)tabCoord.y
                / (tablatureEditor.NStrings + 1)))));

            tabCoord.y = tabCoord.y % (tablatureEditor.NStrings + 1);

            return tabCoord;
        }
    }
}
