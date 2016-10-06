using System.Collections.Generic;
using System.Linq;
using PFE.Configs;
using PFE.Utils;

namespace PFE.Models
{
    public class Tablature
    {
        public List<TablaturePosition> positions;

        public Tablature()
        {
            Init();
        }

        public void Init()
        {
            positions = new List<TablaturePosition>(Config_Tab.Inst().TabLength);

            for (int x = 0; x < Config_Tab.Inst().TabLength; ++x)
                positions.Add(new TablaturePosition());

            tabPositionAt(0).ParseTuning(Config_Tab.Inst().Tuning);
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
