using System.Collections.Generic;
using System.Linq;
using TablatureEditor.Configs;
using TablatureEditor.Utils;

namespace TablatureEditor.Models
{
    public class Tablature
    {
        public List<TabPosition> positions;

        public Tablature()
        {
            init();
        }

        public void init()
        {
            positions = new List<TabPosition>(Config_Tab.TabLength);

            for (int x = 0; x < Config_Tab.TabLength; ++x)
                positions.Add(new TabPosition());

            tabPositionAt(0).ParseTuning(Config_Tab.Tuning);
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

        public string getTextAt(TabCoord tabCoord)
        {
            return positions.ElementAt(tabCoord.x).elements.ElementAt(tabCoord.y).Text;
        }

        private TabElement tabElementAt(TabCoord tabCoord)
        {
            return positions.ElementAt(tabCoord.x).elements.ElementAt(tabCoord.y);
        }

        private TabPosition tabPositionAt(int tabCoord_X)
        {
            return positions.ElementAt(tabCoord_X);
        }

        public bool isElementAtNumberGreatherThan10(TabCoord tabCoord)
        {
            return Util.isNumber(getTextAt(tabCoord));
        }

        public void removePosition(int tabCoord_X)
        {
            positions.RemoveAt(tabCoord_X);
        }
    }
}
