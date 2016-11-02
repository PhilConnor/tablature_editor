using System;
using PFE.Configs;
using System.Windows;

namespace PFE.Models
{
    /// <summary>
    /// Represents a 2d coordinate of a pixel on the DrawSurface.
    /// </summary>
    public class DrawSurfaceCoord : Coord
    {
        public DrawSurfaceCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public DrawSurfaceCoord Clone()
        {
            return new DrawSurfaceCoord(x, y);
        }
        
        /// <summary>
        /// Converts a Point to a DrawSurfaceCoord.
        /// Mainly used because mouse events from WPF returns Point objects.
        /// </summary>
        public static DrawSurfaceCoord PointToDrawSurfaceCoord(Point p)
        {
            return new DrawSurfaceCoord((int)p.X, (int)p.Y);
        }
    }
}
