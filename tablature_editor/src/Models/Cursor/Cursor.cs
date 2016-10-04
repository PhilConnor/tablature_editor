namespace TablatureEditor.Models
{
    //Contains information about cursor position.
    public class Cursor
    {
        //Properties.
        public TabCoord UpperLeftCoord { get; set; }
        public TabCoord LowerRightCoord { get; set; }
        public CursorLogic Logic { get; set; }

        //Constructors.
        public Cursor(CursorLogic cursorLogic)
        {
            Init(cursorLogic, new TabCoord(0, 0), new TabCoord(0, 0));
        }

        public Cursor(CursorLogic cursorLogic, TabCoord upperLeft, TabCoord lowerRight)
        {
            Init(cursorLogic, upperLeft, lowerRight);
        }

        private void Init(CursorLogic cursorLogic, TabCoord upperLeft, TabCoord lowerRight)
        {
            UpperLeftCoord = upperLeft;
            LowerRightCoord = lowerRight;
            Logic = cursorLogic;
            Logic.SetCursor(this);
        }
    }

    public enum CursorMovements { Left, Up, Right, Down, ExpandLeft, ExpandUp, ExpandRight, ExpandDown };
}