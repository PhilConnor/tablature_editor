using System.Windows;

namespace PFE.Models
{
    /// <summary>
    /// Represents a 2d coordinate.
    /// </summary>
    public abstract class Coord
    {
        public int x, y;

        /// <summary>
        /// True if equivalent.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public bool Equals(Coord coord)
        {
            return coord.x == x && coord.y == y;
        }

        /// <summary>
        /// Convert as point.
        /// </summary>
        /// <returns></returns>
        public Point AsPoint()
        {
            return new Point(x, y);
        }
        
        /// <summary>
        /// String reprentation.
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            return "[" + x + "," + y + "]";
        }
    }
}
