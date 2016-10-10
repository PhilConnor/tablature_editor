using System.Collections.Generic;
using System.Linq;
using PFE.Configs;
using PFE.Utils;

namespace PFE.Models
{
    public class Tablature
    {
        public List<Position> positions;

        // Tuning and number of string.
        public string Tuning { get; set; } // Tablature parameters.
        public int NStrings
        {
            get
            {
                return Tuning.Length;
            }
        }

        // Number of staffs.
        public int NStaff { get; set; }
        public int StaffLength { get; set; }

        public int TabLength
        {
            get
            {
                return NStaff * StaffLength;
            }
        }

        public Tablature()
        {
            Init();
        }

        public void Init()
        {
            //DEFAULT VALUES
            Tuning = "EADGBe";
            NStaff = 3;
            StaffLength = 80;

            positions = new List<Position>(TabLength);

            for (int x = 0; x < TabLength; ++x)
                positions.Add(new Position(NStrings));

            tabPositionAt(0).ParseTuning(Tuning);
        }

        public int Length()
        {
            return positions.Count();
        }

        public int StringCount()
        {
            return positions.ElementAt(0).elements.Count();
        }
        
        public void SetElementCharAt(TabCoord tabCoord, char elementChar)
        {
            if (tabCoord == null)
                return;          

            tabElementAt(tabCoord).Character = elementChar;
        }

        public char GetElementCharAt(TabCoord tabCoord)
        {
            return tabElementAt(tabCoord).Character;
        }

        private Element tabElementAt(TabCoord tabCoord)
        {
            if (!tabCoord.IsValid(this))
                return null;

            return positions.ElementAt(tabCoord.x).elements.ElementAt(tabCoord.y);
        }

        private Position tabPositionAt(int tabCoord_X)
        {
            return positions.ElementAt(tabCoord_X);
        }

        public bool isElementAtNumberGreatherThan10(TabCoord tabCoord)
        {
            return Util.IsNumber(GetElementCharAt(tabCoord));
        }

        public void removePosition(int tabCoord_X)
        {
            positions.RemoveAt(tabCoord_X);
        }
    }
}
