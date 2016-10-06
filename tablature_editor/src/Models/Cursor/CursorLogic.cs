using System;
using System.Collections.Generic;
using PFE.Models;
using PFE.Configs;

namespace PFE.Models
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

        public List<TabCoord> GetSelectionTabCoords()
        {
            List<TabCoord> touchingTabCoords = new List<TabCoord>();

            TabCoord topLeft = _cursor.TopLeftCoord();
            int startX = topLeft.x;
            int startY = topLeft.y;

            for (int x = startX; x <= startX + _cursor.Width - 1; x++)
            {
                for (int y = startY; y <= startY + _cursor.Height - 1; y++)
                {
                    touchingTabCoords.Add(new TabCoord(x, y));
                }
            }

            return touchingTabCoords;
        }
        public List<int> GetStaffsTouchingNumbers()
        {
            List<int> staffNumbers = new List<int>();
            int firstX = _cursor.TopLeftCoord().x;
            int lastX = firstX + _cursor.Width - 1;

            staffNumbers[0] = firstX / Config_Tab.TabLength;

            for (var i = firstX + 1; i <= lastX; i++)
            {
                int currentStaffNumber = i / Config_Tab.TabLength;
                int lastStaffNumberInArray = staffNumbers[staffNumbers.Count - 1];

                if (currentStaffNumber != lastStaffNumberInArray)
                    staffNumbers.Add(currentStaffNumber);
            }
            return staffNumbers;
        }

        public void MoveUp()
        {
            if (_cursor._c1.y == 0 || _cursor._c2.y == 0)
            {
                changeStaff(false);
                return;
            }

            _cursor._c1.y--;
            _cursor._c2.y--;
        }

        public void MoveLeft()
        {
            if (_cursor._c1.x == 0 || _cursor._c2.x == 0)
                return;

            _cursor._c1.x--;
            _cursor._c2.x--;
        }

        public void MoveDown()
        {
            if (_cursor._c1.y == Config_Tab.NumberOfStrings - 1 || _cursor._c2.y == Config_Tab.NumberOfStrings - 1)
            {
                changeStaff(true);
                return;
            }

            _cursor._c1.y++;
            _cursor._c2.y++;
        }

        public void MoveRight()
        {
            if (_cursor._c1.x >= Config_Tab.TabLength - 1 || _cursor._c2.x >= Config_Tab.TabLength - 1)
                return;

            _cursor._c1.x++;
            _cursor._c2.x++;
        }

        public void ExpandUp()
        {
            _cursor._c2.y = Math.Max(--_cursor._c2.y, 0);
        }

        public void ExpandLeft()
        {
            _cursor._c2.x = Math.Max(--_cursor._c2.x, 0);
        }

        public void ExpandDown()
        {
            _cursor._c2.y = Math.Min(++_cursor._c2.y, Config_Tab.NumberOfStrings);
        }

        public void ExpandRight()
        {
            _cursor._c2.x = Math.Min(++_cursor._c2.x, Config_Tab.TabLength - 1);
        }

        public bool IsTouchingLastStaff()
        {
            return GetStaffsTouchingNumbers().IndexOf(Config_Tab.NStaff - 1) != -1;
        }

        public bool IsTouchingFirstStaff()
        {
            return GetStaffsTouchingNumbers().IndexOf(0) != -1;
        }

        public bool isTouchingLastString()
        {
            return Math.Max(_cursor._c1.y, _cursor._c2.y) == Config_Tab.NumberOfStrings - 1;
        }

        public bool isTouchingFirstString()
        {
            return Math.Min(_cursor._c1.y, _cursor._c2.y) == 0;
        }

        public bool isTouchingLastPosition()
        {
            return Math.Max(_cursor._c1.x, _cursor._c2.x) == Config_Tab.TabLength - 1;
        }

        public void changeStaff(bool goDown)
        {
            int staffLenght = Config_Tab.StaffLength;

            if (goDown)
            {
                _cursor._c1.x = _cursor._c1.x + staffLenght < staffLenght 
                    ? _cursor._c1.x + staffLenght : _cursor._c1.x;

                _cursor._c2.x = _cursor._c2.x + staffLenght < staffLenght 
                    ? _cursor._c2.x + staffLenght : _cursor._c2.x;
            }
            else
            {
                _cursor._c1.x = _cursor._c1.x - staffLenght >= 0 
                    ? _cursor._c1.x - staffLenght 
                    : _cursor._c1.x;

                _cursor._c2.x = _cursor._c2.x - staffLenght >= 0 
                    ? _cursor._c2.x - staffLenght 
                    : _cursor._c2.x;
            }
        }

    }
}