using System;
using System.Collections.Generic;
using TablatureEditor.Models;
using TablatureEditor.Configs;

namespace TablatureEditor.Models
{
    //Contains and controls the logic of the cursor.
    public class CursorLogic
    {
        //Attributs.
        private Cursor _cursor;

        public void SetCursor(Cursor cursor)
        {
            _cursor = cursor;
        }

        #region Properties
        // Get the horizontal distance between the lower left and upper right corner of the cursor.
        public TabCoord UpperLeftCoord
        {
            get { return _cursor.UpperLeftCoord; }
        }

        // Get the horizontal distance between the lower left and upper right corner of the cursor.
        public int Width
        {
            get { return _cursor.LowerRightCoord.x - _cursor.UpperLeftCoord.x; }
        }

        // Get the vertical distance between the lower left and upper right corner of the cursor.
        public int Height
        {
            get { return _cursor.LowerRightCoord.y - _cursor.UpperLeftCoord.y; }
        }
        #endregion


        #region Public Methods
        // Move the cursor up.
        public void MoveUp()
        {
            // If we are at the top of a staff, we want to select the staff above.
            if (_cursor.UpperLeftCoord.y == 0)
            {
                SkipStaffUp();
            }
            else // Move the cursor 1 note up.
            {
                --_cursor.UpperLeftCoord.y;
                --_cursor.LowerRightCoord.y;
            }
        }

        // Move the cursor right.
        public void MoveRight()
        {
            if (_cursor.LowerRightCoord.x < Config_Tab.TabLength - 1)
            {
                ++_cursor.UpperLeftCoord.x;
                ++_cursor.LowerRightCoord.x;
            }
        }

        // Move the cursor down.
        public void MoveDown()
        {
            // If we are at the bottom of a staff, we want to select the staff bellow.
            if (_cursor.LowerRightCoord.y == Config_Tab.NumberOfStrings - 1)
            {
                SkipStaffDown();
            }
            else // Move the cursor 1 note down.
            {
                ++_cursor.UpperLeftCoord.y;
                ++_cursor.LowerRightCoord.y;
            }
        }

        // Move the cursor left.
        public void MoveLeft()
        {
            if (_cursor.UpperLeftCoord.x > 0)
            {
                --_cursor.UpperLeftCoord.x;
                --_cursor.LowerRightCoord.x;
            }
        }

        //Expand the cursor 1 note up.
        public void ExpandUp()
        {
            if (_cursor.UpperLeftCoord.y > 0)
            {
                --_cursor.UpperLeftCoord.y;
            }
        }

        //Expand the cursor 1 note right.
        public void ExpandRight()
        {
            if (_cursor.LowerRightCoord.x < Config_Tab.TabLength - 1)
            {
                ++_cursor.LowerRightCoord.x;
            }
        }

        //Expand the cursor 1 note down.
        public void ExpandDown()
        {
            if (_cursor.LowerRightCoord.y < Config_Tab.NumberOfStrings - 1)
            {
                ++_cursor.LowerRightCoord.y;
            }
        }

        //Expand the cursor 1 note left.
        public void ExpandLeft()
        {
            if (_cursor.UpperLeftCoord.x > 0)
            {
                --_cursor.UpperLeftCoord.x;
            }
        }

        // Get a list of every coords of notes currently selected by the cursor.
        public List<TabCoord> GetSelectionTabCoords()
        {
            List<TabCoord> touchingTabCoords = new List<TabCoord>();

            int startX = _cursor.UpperLeftCoord.x;
            int startY = _cursor.UpperLeftCoord.y;

            for (int x = startX; x <= startX + Width; ++x)
            {
                for (int y = startY; y <= startY + Height; ++y)
                {
                    touchingTabCoords.Add(new TabCoord(x, y));
                }
            }

            return touchingTabCoords;
        }
        #endregion

        #region Private Methods
        //Change the cursor 1 staff up or down.
        private void SkipStaffUp()
        {
            if (_cursor.UpperLeftCoord.x - Config_Tab.StaffLength > 0)
            {
                _cursor.UpperLeftCoord.x -= Config_Tab.StaffLength;
                _cursor.LowerRightCoord.x -= Config_Tab.StaffLength;
            }
        }

        private void SkipStaffDown()
        {
            if (_cursor.LowerRightCoord.x + Config_Tab.StaffLength < Config_Tab.TabLength)
            {
                _cursor.UpperLeftCoord.x += Config_Tab.StaffLength;
                _cursor.LowerRightCoord.x += Config_Tab.StaffLength;
            }
        }
        #endregion

       

        #region Enums
        private enum ChangeStaffDirection { Up, Down };
        #endregion
    }
}



/*
       public List<int> getStaffsTouchingNumbers()
       {
           List<int> staffNumbers = new List<int>();
           int firstX = GetCursorTopLeftPosition().x;
           int lastX = firstX + GetCursorWidth() - 1;

           staffNumbers[0] = firstX / Configuration.StaffLength;

           for (var i = firstX + 1; i <= lastX; i++)
           {
               int currentStaffNumber = i / Configuration.StaffLength;
               int lastStaffNumberInArray = staffNumbers[staffNumbers.Count - 1];

               if (currentStaffNumber != lastStaffNumberInArray)
                   staffNumbers.Add(currentStaffNumber);
           }
           return staffNumbers;
       }

       public bool isTouchingLastStaff()
       {
           return getStaffsTouchingNumbers().IndexOf(Configuration.NStaff - 1) != -1;
       }

       public bool isTouchingFirstStaff()
       {
           return getStaffsTouchingNumbers().IndexOf(0) != -1;
       }

       public bool isTouchingLastString()
       {
           return Math.Max(lowerLeft.y, upperRight.y) == Configuration.NumberOfStrings - 1;
       }

       public bool isTouchingFirstString()
       {
           return Math.Min(lowerLeft.y, upperRight.y) == 0;
       }

       public bool isTouchingLastPosition()
       {
           return Math.Max(lowerLeft.x, upperRight.x) == Configuration.TabLength - 1;
       }
       */
