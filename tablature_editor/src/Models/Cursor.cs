namespace TablatureEditor.Models
{
    //Contains information about cursor position.
    public class Cursor
    {
        //Properties.
        public TabCoord UpperLeftCoord { get; set; }
        public TabCoord LowerRightCoord { get; set; }

        //Constructors.
        public Cursor()
        {
            UpperLeftCoord = new TabCoord(0, 0);
            LowerRightCoord = new TabCoord(0, 0);
        }

        public Cursor(TabCoord upperLeft, TabCoord lowerRight)
        {
            UpperLeftCoord = upperLeft;
            LowerRightCoord = lowerRight;
        }
    }

    public enum CursorMovements { Left, Up, Right, Down, ExpandLeft, ExpandUp, ExpandRight, ExpandDown };
}