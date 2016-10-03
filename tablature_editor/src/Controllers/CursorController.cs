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
        public TabCoord UpperLeftCoord
        {
            get { return cursor.UpperLeftCoord; }
        }

        // Get the horizontal distance between the lower left and upper right corner of the cursor.
        public int Width
        {
            get { return cursor.LowerRightCoord.x - cursor.UpperLeftCoord.x; }
        }

        // Get the vertical distance between the lower left and upper right corner of the cursor.
        public int Height
        {
            get { return cursor.LowerRightCoord.y - cursor.UpperLeftCoord.y; }
        }
        #endregion

        //Constructors.
        public CursorController()
        {
            cursor = new Cursor();
        }

        #region Public Methods
        // Move the cursor up.
        public void MoveUp()
        {
            // If we are at the top of a staff, we want to select the staff above.
            if (cursor.UpperLeftCoord.y == 0)
            {
                SkipStaffUp();
            }
            else // Move the cursor 1 note up.
            {
                --cursor.UpperLeftCoord.y;
                --cursor.LowerRightCoord.y;
            }
        }

        // Move the cursor right.
        public void MoveRight()
        {
            if (cursor.LowerRightCoord.x < Configuration.TabLength - 1)
            {
                ++cursor.UpperLeftCoord.x;
                ++cursor.LowerRightCoord.x;
            }
        }

        // Move the cursor down.
        public void MoveDown()
        {
            // If we are at the bottom of a staff, we want to select the staff bellow.
            if (cursor.LowerRightCoord.y == Configuration.NumberOfStrings - 1)
            {
                SkipStaffDown();
            }
            else // Move the cursor 1 note down.
            {
                ++cursor.UpperLeftCoord.y;
                ++cursor.LowerRightCoord.y;
            }
        }

        // Move the cursor left.
        public void MoveLeft()
        {
            if (cursor.UpperLeftCoord.x > 0)
            {
                --cursor.UpperLeftCoord.x;
                --cursor.LowerRightCoord.x;
            }
        }

        //Expand the cursor 1 note up.
        public void ExpandUp()
        {
            if (cursor.UpperLeftCoord.y > 0)
            {
                --cursor.UpperLeftCoord.y;
            }
        }

        //Expand the cursor 1 note right.
        public void ExpandRight()
        {
            if (cursor.LowerRightCoord.x < Configuration.TabLength - 1)
            {
                ++cursor.LowerRightCoord.x;
            }
        }

        //Expand the cursor 1 note down.
        public void ExpandDown()
        {
            if (cursor.LowerRightCoord.y < Configuration.NumberOfStrings - 1)
            {
                ++cursor.LowerRightCoord.y;
            }
        }

        //Expand the cursor 1 note left.
        public void ExpandLeft()
        {
            if (cursor.UpperLeftCoord.x > 0)
            {
                --cursor.UpperLeftCoord.x;
            }
        }

        // Get a list of every coords of notes currently selected by the cursor.
        public List<TabCoord> GetSelectionTabCoords()
        {
            List<TabCoord> touchingTabCoords = new List<TabCoord>();

            int startX = cursor.UpperLeftCoord.x;
            int startY = cursor.UpperLeftCoord.y;

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
            if (cursor.UpperLeftCoord.x - Configuration.StaffLength > 0)
            {
                cursor.UpperLeftCoord.x -= Configuration.StaffLength;
                cursor.LowerRightCoord.x -= Configuration.StaffLength;
            }
        }

        private void SkipStaffDown()
        {
            if (cursor.LowerRightCoord.x + Configuration.StaffLength < Configuration.TabLength)
            {
                cursor.UpperLeftCoord.x += Configuration.StaffLength;
                cursor.LowerRightCoord.x += Configuration.StaffLength;
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
