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

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public string ToString()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            return "[" + x + "," + y + "]";
        }
    }
}
