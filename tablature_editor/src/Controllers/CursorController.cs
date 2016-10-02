using System;
using System.Collections.Generic;
using TablatureEditor.Models;
using TablatureEditor.Configs;

namespace TablatureEditor.Controllers
{
    //Contains and controls the logic of the cursor.
    public class CursorController
    {
        #region Enums
        private enum ChangeStaffDirection
        {
            Up,
            Down
        };
        #endregion

        //Attributs.
        private Cursor cursor;

        #region Properties
        // Get the horizontal distance between the lower left and upper right corner of the cursor.
        public TabCoord GetCursorTopLeft
        {
            get { return cursor.UpperLeft; }
        }

        // Get the horizontal distance between the lower left and upper right corner of the cursor.
        public int GetCursorWidth
        {
            get { return cursor.LowerRight.x - cursor.UpperLeft.x; }
        }

        // Get the vertical distance between the lower left and upper right corner of the cursor.
        public int GetCursorHeight
        {
            get { return cursor.LowerRight.y - cursor.UpperLeft.y; }
        }
        #endregion

        //Constructors.
        public CursorController()
        {
            cursor = new Cursor();
        }

        #region Public Methods
        // Move the cursor up.
        public void MoveCursorUp()
        {
            // If we are at the top of a staff, we want to select the staff above.
            if (cursor.UpperLeft.y == 0)
            {
                ChangeStaff(ChangeStaffDirection.Up);
            }
            else // Move the cursor 1 note up.
            {
                --cursor.UpperLeft.y;
                --cursor.LowerRight.y;
            }
        }

        // Move the cursor right.
        public void MoveCursorRight()
        {
            if (cursor.LowerRight.x < Configuration.TabLength - 1)
            {
                ++cursor.UpperLeft.x;
                ++cursor.LowerRight.x;
            }
        }

        // Move the cursor down.
        public void MoveCursorDown()
        {
            // If we are at the bottom of a staff, we want to select the staff bellow.
            if (cursor.LowerRight.y == Configuration.NumberOfStrings - 1)
            {
                ChangeStaff(ChangeStaffDirection.Down);
            }
            else // Move the cursor 1 note down.
            {
                ++cursor.UpperLeft.y;
                ++cursor.LowerRight.y;
            }
        }

        // Move the cursor left.
        public void MoveCursorLeft()
        {
            if (cursor.UpperLeft.x > 0)
            {
                --cursor.UpperLeft.x;
                --cursor.LowerRight.x;
            }
        }

        //Expand the cursor 1 note up.
        public void ExpandCursorUp()
        {
            if (cursor.UpperLeft.y > 0)
            {
                --cursor.UpperLeft.y;
            }
        }

        //Expand the cursor 1 note right.
        public void ExpandCursorRight()
        {
            if (cursor.LowerRight.x < Configuration.TabLength - 1)
            {
                ++cursor.LowerRight.x;
            }
        }

        //Expand the cursor 1 note down.
        public void ExpandCursorDown()
        {
            if (cursor.LowerRight.y < Configuration.NumberOfStrings - 1)
            {
                ++cursor.LowerRight.y;
            }
        }

        //Expand the cursor 1 note left.
        public void ExpandCursorLeft()
        {
            if (cursor.UpperLeft.x > 0)
            {
                --cursor.UpperLeft.x;
            }
        }

        // Get a list of every notes currently selected by the cursor.
        public List<TabCoord> GetTouchingTabCoords()
        {
            List<TabCoord> touchingTabCoords = new List<TabCoord>();

            int startX = cursor.UpperLeft.x;
            int startY = cursor.UpperLeft.y;

            for (int x = startX; x <= startX + GetCursorWidth; ++x)
            {
                for (int y = startY; y <= startY + GetCursorHeight; ++y)
                {
                    touchingTabCoords.Add(new TabCoord(x, y));
                }
            }

            return touchingTabCoords;
        }
        #endregion

        #region Private Methods
        //Change the cursor 1 staff up or down.
        private void ChangeStaff(ChangeStaffDirection direction)
        {
            if (direction == ChangeStaffDirection.Down && 
                cursor.LowerRight.x + Configuration.StaffLength < Configuration.TabLength)
            {
                cursor.UpperLeft.x += Configuration.StaffLength;
                cursor.LowerRight.x += Configuration.StaffLength;
            }
            else if (direction == ChangeStaffDirection.Up  &&
                cursor.UpperLeft.x - Configuration.StaffLength > 0)
            {
                cursor.UpperLeft.x -= Configuration.StaffLength;
                cursor.LowerRight.x -= Configuration.StaffLength;
            }
        }
        #endregion

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
    }
}
