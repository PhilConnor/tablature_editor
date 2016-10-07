using System.Collections.Generic;
using System.Linq;
using PFE.Configs;
using PFE.Utils;

namespace PFE.Models
{
    public class Tablature
    {
        public List<TablaturePosition> positions;

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
                    
            positions = new List<TablaturePosition>(TabLength);

            for (int x = 0; x < TabLength; ++x)
                positions.Add(new TablaturePosition(NStrings));

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

        public void setTextAt(TabCoord tabCoord, string elementText)
        {
            //if we are about to write a 10th or 20th we remove the 
            // char occuring before it to make space for this extra character
            if (elementText.Length > 1 && tabCoord.y > 1)
            {
                positions.ElementAt(tabCoord.x).elements.ElementAt(tabCoord.y - 1).ClearText();
            }

            positions.ElementAt(tabCoord.x).elements.ElementAt(tabCoord.y).Text = elementText;
        }

        public string GetTextAt(TabCoord tabCoord)
        {
            return positions.ElementAt(tabCoord.x).elements.ElementAt(tabCoord.y).Text;
        }

        private TablatureElement tabElementAt(TabCoord tabCoord)
        {
            return positions.ElementAt(tabCoord.x).elements.ElementAt(tabCoord.y);
        }

        private TablaturePosition tabPositionAt(int tabCoord_X)
        {
            return positions.ElementAt(tabCoord_X);
        }

        public bool isElementAtNumberGreatherThan10(TabCoord tabCoord)
        {
            return Util.isNumber(GetTextAt(tabCoord));
        }

        public void removePosition(int tabCoord_X)
        {
            positions.RemoveAt(tabCoord_X);
        }
    }
}
