using System.Windows;

namespace PFE.Models
{
    //Base class for Canvas and Tab coordinates.
    public abstract class Coord
    {
        //Attributs.
        public int x, y;

        public bool Equals(Coord coord)
        {
            return coord.x == x && coord.y == y;
        }

        public string ToString()
        {
            return "[" + x + "," + y + "]";
        }
    }
}
