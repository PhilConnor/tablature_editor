using System;
using System.Collections.Generic;
using System.Linq;
using PFE.Configs;

namespace PFE.Models
{
    /// <summary>
    /// Represents a column(horizontal) position on the tablature.
    /// Contains an element for each string of the tablature for 
    /// this position.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// The elements of this tablature from top to bottom.
        /// Index 0 is the element on the first string (biggest string).
        /// </summary>
        public List<Element> elements;

        public Position(int nStrings)
        {
            elements = new List<Element>(nStrings);

            for (int y = 0; y < nStrings; ++y)
            {
                elements.Add(new Element());
            }
        }
        
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
