using System;
using System.Collections.Generic;
using System.Linq;
using TablatureEditor.Configs;

namespace TablatureEditor.Models
{
    public class TabPosition
    {
        // Attributs.
        public List<TabElement> elements; // From top to bottom.

        // Constructors.
        public TabPosition()
        {
            elements = new List<TabElement>(Config_Tab.NumberOfStrings);

            for (int y = 0; y < Config_Tab.NumberOfStrings; ++y)
            {
                elements.Add(new TabElement());
            }
        }

        // Public Methods.
        public void ParseString(string stringTabElements)
        {
            string[] separators = { ":" };
            string[] c = stringTabElements.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            for (int y = 0; y < elements.Count(); ++y)
            {
                elements.ElementAt(y).Text = c.ElementAt(elements.Count() - 1 - y);
            }
        }

        public void ParseTuning(string tuning)
        {
            char[] tuningCharArray = tuning.ToCharArray();

            for (int y = 0; y < tuningCharArray.Count(); ++y)
            {
                elements.ElementAt(y).Text = tuningCharArray.ElementAt(elements.Count() - 1 - y).ToString();
            }
        }
    }
}
