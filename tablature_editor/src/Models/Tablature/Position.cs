using System;
using System.Collections.Generic;
using System.Linq;
using PFE.Configs;

namespace PFE.Models
{
    public class Position
    {
        // Attributs.
        public List<Element> elements; // From top to bottom.

        // Constructors.
        public Position(int nStrings)
        {
            elements = new List<Element>(nStrings);

            for (int y = 0; y < nStrings; ++y)
            {
                elements.Add(new Element());
            }
        }

        // Public Methods.
        public void ParseString(string stringTabElements)
        {
            string[] separators = { ":" };
            string[] c = stringTabElements.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            for (int y = 0; y < elements.Count(); ++y)
            {
                elements.ElementAt(y).Character = c.ElementAt(elements.Count() - 1 - y).ToCharArray()[0];
            }
        }

        public void ParseTuning(string tuning)
        {
            char[] tuningCharArray = tuning.ToCharArray();

            for (int y = 0; y < tuningCharArray.Count(); ++y)
            {
                elements.ElementAt(y).Character = tuningCharArray.ElementAt(elements.Count() - 1 - y);
            }
        }
    }
}
