using System;
using TablatureEditor.Configs;

namespace TablatureEditor.Models
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
    }
}
