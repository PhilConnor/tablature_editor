namespace TablatureEditor.Models
{
    //Contains information about cursor position.
    public class Cursor
    {
        //Properties.
        public TabCoord UpperLeft { get; set; }
        public TabCoord LowerRight { get; set; }

        //Constructors.
        public Cursor()
        {
            UpperLeft = new TabCoord(0, 0);
            LowerRight = new TabCoord(0, 0);
        }

        public Cursor(TabCoord upperLeft, TabCoord lowerRight)
        {
            UpperLeft = upperLeft;
            LowerRight = lowerRight;
        }
    }
}