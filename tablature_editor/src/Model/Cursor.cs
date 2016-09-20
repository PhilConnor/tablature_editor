using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using tablature_editor.src.Interfaces;

namespace tablature_editor
{
    class Cursor
    {
        private Coord _c1;
        private Coord _c2;

        public Cursor()
        {
            resetPositions();
        }

        public Cursor(Coord tabCoord1, Coord tabCoord2)
        {
            _c1 = tabCoord1;
            _c2 = tabCoord2;
        }

        public int getWidth()
        {
            return Math.Abs(_c2.x - _c1.x) + 1;
        }

        public int getHeight()
        {
            return Math.Abs(_c2.y - _c1.y) + 1;
        }

        public void setPositions(Coord tabCoord)
        {
            _c1 = tabCoord.getClone();
            _c2 = tabCoord.getClone();
        }

        public Coord getTopLeftCoord()
        {
            var x = Math.Min(_c1.x, _c2.x);
            var y = Math.Min(_c1.y, _c2.y);
            return new Coord(x, y);
        }

        public void resetPositions()
        {
            setPositions(new Coord(1, 0));
        }
        
        public List<Coord> getTouchingTabCoords()
        {
            List<Coord> touchingTabCoords = new List<Coord>();

            Coord topLeft = getTopLeftCoord();
            int startX = topLeft.x;
            int startY = topLeft.y;

            for (int x = startX; x <= startX + getWidth() - 1; x++)
            {
                for (int y = startY; y <= startY + getHeight() - 1; y++)
                {
                    touchingTabCoords.Add(new Coord(x, y));
                }
            }

            return touchingTabCoords;
        }

        public Cursor getClone()
        {
            Cursor clone = new Cursor(_c1.getClone(), _c2.getClone());
            return clone;
        }

        public List<int> getStaffsTouchingNumbers()
        {
            List<int> staffNumbers = new List<int>();
            int firstX = getTopLeftCoord().x;
            int lastX = firstX + getWidth() - 1;

            staffNumbers[0] = firstX / Configuration.Inst.staffLength;

            for (var i = firstX + 1; i <= lastX; i++)
            {
                int currentStaffNumber = i / Configuration.Inst.staffLength;
                int lastStaffNumberInArray = staffNumbers[staffNumbers.Count - 1];

                if (currentStaffNumber != lastStaffNumberInArray)
                    staffNumbers.Add(currentStaffNumber);
            }
            return staffNumbers;
        }

        public bool equals(Cursor c)
        {
            return _c1.equals(c._c1) && _c2.equals(c._c2);
        }

        public void moveUp()
        {
            if (_c1.y == 0 || _c2.y == 0)
            {
                changeStaff(false);
                return;
            }
            _c1.y--;
            _c2.y--;
        }

        public void moveLeft()
        {
            if (_c1.x == 0 || _c2.x == 0)
            {
                return;
            }
            _c1.x--;
            _c2.x--;
        }

        public void moveDown()
        {
            if (_c1.y == Configuration.Inst.NStringPerStaff - 1 || _c2.y == Configuration.Inst.NStringPerStaff - 1)
            {
                changeStaff(true);
                return;
            }
            _c1.y++;
            _c2.y++;
        }

        public void moveRight()
        {
            if (_c1.x >= Configuration.Inst.TabLength - 1 || _c2.x >= Configuration.Inst.TabLength - 1)
                return;

            _c1.x++;
            _c2.x++;
        }

        public void expandUp()
        {
            _c2.y = Math.Max(--_c2.y, 0);
        }

        public void expandLeft()
        {
            _c2.x = Math.Max(--_c2.x, 0);
        }

        public void expandDown()
        {
            _c2.y = Math.Min(++_c2.y, Configuration.Inst.NStringPerStaff - 1);
        }

        public void expandRight()
        {
            _c2.x = Math.Min(++_c2.x, Configuration.Inst.TabLength - 1);
        }

        public bool isTouchingLastStaff()
        {
            return getStaffsTouchingNumbers().IndexOf(Configuration.Inst.NStaff - 1) != -1;
        }

        public bool isTouchingFirstStaff()
        {
            return getStaffsTouchingNumbers().IndexOf(0) != -1;
        }

        public bool isTouchingLastString()
        {
            return Math.Max(_c1.y, _c2.y) == Configuration.Inst.NStringPerStaff - 1;
        }

        public bool isTouchingFirstString()
        {
            return Math.Min(_c1.y, _c2.y) == 0;
        }

        public bool isTouchingLastPosition()
        {
            return Math.Max(_c1.x, _c2.x) == Configuration.Inst.TabLength - 1;
        }

        public void changeStaff(bool goDown)
        {
            if (goDown)
            {
                _c1.x = _c1.x + Configuration.Inst.staffLength < Configuration.Inst.TabLength ? _c1.x + Configuration.Inst.staffLength : _c1.x;
                _c2.x = _c2.x + Configuration.Inst.staffLength < Configuration.Inst.TabLength ? _c2.x + Configuration.Inst.staffLength : _c2.x;
            }
            else
            {
                _c1.x = _c1.x - Configuration.Inst.staffLength >= 0 ? _c1.x - Configuration.Inst.staffLength : _c1.x;
                _c2.x = _c2.x - Configuration.Inst.staffLength >= 0 ? _c2.x - Configuration.Inst.staffLength : _c2.x;
            }
        }
    } /**/
}