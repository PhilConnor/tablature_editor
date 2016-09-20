using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace tablature_editor
{
    public class Coord
    {
        public int x, y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool equals(Coord coord)
        {
            return this.x == coord.x && this.y == coord.y;
        }

        public Coord getClone()
        {
            return new Coord(this.x, this.y);
        }

        // Converts a tablature coord to a canvas pixel position on the canvas.
        // Used mainly to figure out where to draw the tab chars on the canvas.
        public static Coord toCanvasCoord(Coord tabCoord)
        {
            Coord r = new Coord(0, 0);

            r.x = (tabCoord.x % Configuration.Inst.staffLength) * Configuration.Inst.unitSizeX;
            r.x += Configuration.Inst.marginX;

            r.y = (int)Math.Floor((double)tabCoord.x / Configuration.Inst.staffLength);
            r.y = r.y * (Configuration.Inst.NStringPerStaff+ Configuration.Inst.staffSpacingUnitY) + tabCoord.y;
            r.y = r.y * Configuration.Inst.unitSizeY;
            r.y += Configuration.Inst.marginY;

            return r;
        }

        // Converts a pixel canvas pixel coord to an actual position in the tablature.
        // Used mainly to track mouse position on the tablature.
        public static Coord toTabCoord(Coord canvasCoord)
        {
            var r = new Coord(0, 0);

            // remove margin and scale
            r.x = (int)Math.Floor((double)(canvasCoord.x - Configuration.Inst.marginX) / Configuration.Inst.unitSizeX);
            r.y = (int)Math.Floor((double)(canvasCoord.y - Configuration.Inst.marginY) / Configuration.Inst.unitSizeY);

            // if position is on a blankspace between staffs, make it on last string
            if (r.y % (Configuration.Inst.NStringPerStaff+ 1) == Configuration.Inst.NStringPerStaff)
                r.y = r.y - 1;

            // if position is out of bound, null
            if (r.y < 0
                || r.y >= (Configuration.Inst.NStringPerStaff+ 1) * Configuration.Inst.NStaff - 1
                || r.x < 0
                || r.x >= Configuration.Inst.staffLength)
                return null;

            // convert to x = char position, y = string number
            r.x = (int)(r.x + ((Configuration.Inst.staffLength
                * Math.Floor((double)r.y / (Configuration.Inst.NStringPerStaff+ 1)))));
            r.y = r.y % (Configuration.Inst.NStringPerStaff+ 1);

            return r;
        }

        // checks if the coord is defined and inbound
        public static bool isValidTabCoord(Coord coord)
        {
            var c1 = coord != null;
            var c3 = coord.x >= 0 && coord.y >= 0;
            var c4 = coord.x < (Configuration.Inst.staffLength * Configuration.Inst.NStaff)
                && coord.y < Configuration.Inst.NStringPerStaff;

            return c1 && c3 && c4;
        }
    } //
}
