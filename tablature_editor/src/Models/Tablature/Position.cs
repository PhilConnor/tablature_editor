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
                elements.ElementAt(y).ClearText();
                elements.ElementAt(y).RightChar = c.ElementAt(elements.Count() - 1 - y).ToCharArray()[0];
            }
        }

        public void ParseTuning(Tuning tuning)
        {
            for (int y = 0; y < tuning.notes.Count(); ++y)
            {
                elements.ElementAt(y).ClearText();
                elements.ElementAt(y).RightChar = tuning.notes.ElementAt(elements.Count() - 1 - y).GetNoteChar();
            }
        }

        public bool Equals(Position position)
        {
            if (position.elements.Count != elements.Count)
                return false;

            for (int i = 0; i < elements.Count; i++)
            {
                if (!elements[i].Equals(position.elements[i]))
                    return false;
            }

            return true;
        }


        public Position Clone()
        {
            Position clone = new Position(this.elements.Count);
            for (int i = 0; i < elements.Count; i++)
            {
                clone.elements[i] = elements[i].Clone();
            }

            return clone;
        }

    }



}
