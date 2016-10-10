using System;
using PFE.Configs;
using System.Windows;

namespace PFE.Models
{
    //Coordinates in the main canvas of the application.
    public class DrawSurfaceCoord : Coord
    {
        //Constructors.
        public DrawSurfaceCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public DrawSurfaceCoord Clone()
        {
            return new DrawSurfaceCoord(x, y);
        }

        public static DrawSurfaceCoord PointToCanvasCoord(Point p)
        {
            return new DrawSurfaceCoord((int)p.X, (int)p.Y);
        }
    }
}
