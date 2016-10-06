using System;
using PFE.Configs;
using System.Windows;

namespace PFE.Models
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

        public CanvasCoord Clone()
        {
            return new CanvasCoord(x, y);
        }

        public static CanvasCoord PointToCanvasCoord(Point p)
        {
            return new CanvasCoord((int)p.X, (int)p.Y);
        }
    }
}
