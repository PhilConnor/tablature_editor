using System;
using System.Collections.Generic;
using System.Linq;
using PFE.Configs;

namespace PFE.Models
{
    public class TablaturePosition
    {
        // Attributs.
        public List<TablatureElement> elements; // From top to bottom.

        // Constructors.
        public TablaturePosition(int nStrings)
        {
            elements = new List<TablatureElement>(nStrings);

            for (int y = 0; y < nStrings; ++y)
            {
                elements.Add(new TablatureElement());
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
