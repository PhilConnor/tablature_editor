using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFE.Configs;
using PFE.Models;
using System.Windows;

namespace tablature_editor.Utils
{
    /// <summary>
    /// Provides method for coord format conversion.
    /// </summary>
    class CoordConverter
    {
        /// <summary>
        /// Converts a TabCoord to a DrawSurfaceCoord.
        /// </summary>
        /// <param name="tabCoord">The tabcoord to convert.</param>
        /// <param name="tablatureEditor">The Editor that the TabCoord is relative to.</param>
        /// <returns></returns>
        public static DrawSurfaceCoord ToDrawSurfaceCoord(TabCoord tabCoord, Editor tablatureEditor)
        {
            DrawSurfaceCoord drawSurfaceCoord = new DrawSurfaceCoord(0, 0);

            drawSurfaceCoord.x = (tabCoord.x % tablatureEditor.StaffLength) * Config_DrawSurface.Inst().GridUnitWidth;
            drawSurfaceCoord.x += Config_DrawSurface.Inst().MarginX;

            drawSurfaceCoord.y = (int)Math.Floor((double)tabCoord.x / tablatureEditor.StaffLength);
            drawSurfaceCoord.y = drawSurfaceCoord.y *
                (tablatureEditor.NStrings + Config_DrawSurface.Inst().SpacingBetweenStaff) + tabCoord.y;
            drawSurfaceCoord.y = drawSurfaceCoord.y * Config_DrawSurface.Inst().GridUnitHeight;
            drawSurfaceCoord.y += Config_DrawSurface.Inst().MarginY;

            return drawSurfaceCoord;
        }

        /// <summary>
        /// Converts a DrawSurfaceCoord to a TabCoord.
        /// </summary>
        /// <param name="drawSurfaceCoord">The DrawSurfaceCoord to convert.</param>
        /// <param name="tablatureEditor">The Editor that the TabCoord will be relative to.</param>
        /// <returns></returns>
        public static TabCoord ToTabCoord(DrawSurfaceCoord drawSurfaceCoord, Editor tablatureEditor)
        {
            TabCoord tabCoord = new TabCoord(0, 0);

            // Remove margin and scale
            tabCoord.x = (int)Math.Floor((double)(drawSurfaceCoord.x - Config_DrawSurface.Inst().MarginX)
                / Config_DrawSurface.Inst().GridUnitWidth);
            tabCoord.y = (int)Math.Floor((double)(drawSurfaceCoord.y - Config_DrawSurface.Inst().MarginY)
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


        /// <summary>
        /// Converts a Point to a TabCoord.
        /// </summary>
        /// <param name="pointOnDrawSurface">A point on the draw surface to be converted</param>
        /// <param name="tablatureEditor">The Editor that the TabCoord will be relative to.</param>
        /// <returns></returns>
        public static TabCoord ToTabCoord(Point pointOnDrawSurface, Editor tablatureEditor)
        {
            DrawSurfaceCoord drawSurfaceCoord = DrawSurfaceCoord.PointToDrawSurfaceCoord(pointOnDrawSurface);
            TabCoord tabCoord = CoordConverter.ToTabCoord(drawSurfaceCoord, tablatureEditor);
            return tabCoord;
        }

    }
}
