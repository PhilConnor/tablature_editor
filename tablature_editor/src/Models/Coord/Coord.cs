using System.Windows;

namespace PFE.Models
{
    /// <summary>
    /// Represents a 2d coordinate.
    /// </summary>
    public abstract class Coord
    {
        public int x, y;

        public bool Equals(Coord coord)
        {
            return coord.x == x && coord.y == y;
        }

        public Point AsPoint()
        {
            return new Point(x, y);
        }

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            return "[" + x + "," + y + "]";
        }
    }
}
