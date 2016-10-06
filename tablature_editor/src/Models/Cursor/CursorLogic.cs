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

        public List<TabCoord> GetSelectedTabCoords()
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
            if (_cursor.BaseCoord.y == 0 || _cursor.DragableCoord.y == 0)
            {
                changeStaff(false);
                return;
            }

            _cursor.BaseCoord.y--;
            _cursor.DragableCoord.y--;
        }

        public void MoveLeft()
        {
            if (_cursor.BaseCoord.x == 0 || _cursor.DragableCoord.x == 0)
                return;

            _cursor.BaseCoord.x--;
            _cursor.DragableCoord.x--;
        }

        public void MoveDown()
        {
            if (_cursor.BaseCoord.y == Config_Tab.NStrings - 1 || _cursor.DragableCoord.y == Config_Tab.NStrings - 1)
            {
                changeStaff(true);
                return;
            }

            _cursor.BaseCoord.y++;
            _cursor.DragableCoord.y++;
        }

        public void MoveRight()
        {
            if (_cursor.BaseCoord.x >= Config_Tab.TabLength - 1 || _cursor.DragableCoord.x >= Config_Tab.TabLength - 1)
                return;

            _cursor.BaseCoord.x++;
            _cursor.DragableCoord.x++;
        }

        public void ExpandUp()
        {
            _cursor.DragableCoord.y = Math.Max(--_cursor.DragableCoord.y, 0);
        }

        public void ExpandLeft()
        {
            _cursor.DragableCoord.x = Math.Max(--_cursor.DragableCoord.x, 0);
        }

        public void ExpandDown()
        {
            _cursor.DragableCoord.y = Math.Min(++_cursor.DragableCoord.y, Config_Tab.NStrings);
        }

        public void ExpandRight()
        {
            _cursor.DragableCoord.x = Math.Min(++_cursor.DragableCoord.x, Config_Tab.TabLength - 1);
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
            return Math.Max(_cursor.BaseCoord.y, _cursor.DragableCoord.y) == Config_Tab.NStrings - 1;
        }

        public bool isTouchingFirstString()
        {
            return Math.Min(_cursor.BaseCoord.y, _cursor.DragableCoord.y) == 0;
        }

        public bool isTouchingLastPosition()
        {
            return Math.Max(_cursor.BaseCoord.x, _cursor.DragableCoord.x) == Config_Tab.TabLength - 1;
        }

        public void changeStaff(bool goDown)
        {
            int staffLenght = Config_Tab.StaffLength;

            if (goDown)
            {
                _cursor.BaseCoord.x = _cursor.BaseCoord.x + staffLenght < staffLenght 
                    ? _cursor.BaseCoord.x + staffLenght : _cursor.BaseCoord.x;

                _cursor.DragableCoord.x = _cursor.DragableCoord.x + staffLenght < staffLenght 
                    ? _cursor.DragableCoord.x + staffLenght : _cursor.DragableCoord.x;
            }
            else
            {
                _cursor.BaseCoord.x = _cursor.BaseCoord.x - staffLenght >= 0 
                    ? _cursor.BaseCoord.x - staffLenght 
                    : _cursor.BaseCoord.x;

                _cursor.DragableCoord.x = _cursor.DragableCoord.x - staffLenght >= 0 
                    ? _cursor.DragableCoord.x - staffLenght 
                    : _cursor.DragableCoord.x;
            }
        }

    }
}