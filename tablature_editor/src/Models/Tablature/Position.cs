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

        /// <summary>
        /// Construct a possition of nStrings elements
        /// </summary>
        /// <param name="nStrings"></param>
        public Position(int nStrings)
        {
            elements = new List<Element>(nStrings);

            for (int y = 0; y < nStrings; ++y)
            {
                elements.Add(new Element());
            }
        }

        /// <summary>
        /// Add a blank element at first or last.
        /// </summary>
        /// <param name="atLast"></param>
        public void AddBlankElement(bool atLast)
        {
            if (atLast)
                AddNewLastElement(new Element());
            else
                AddNewFirstElement(new Element());
        }

        /// <summary>
        /// Remove first or last element.
        /// </summary>
        /// <param name="atLast"></param>
        public void RemoveElement(bool atLast)
        {
            if (atLast)
                elements.Remove(elements.Last());
            else
                elements.Remove(elements.First());
        }

        /// <summary>
        /// Adds a blank first element.
        /// </summary>
        /// <param name="element"></param>
        private void AddNewFirstElement(Element element)
        {
            elements.Insert(0, element);
        }

        /// <summary>
        /// Adds a blank last element.
        /// </summary>
        /// <param name="element"></param>
        private void AddNewLastElement(Element element)
        {
            elements.Add(element);
        }

        /// <summary>
        /// Remove first element.
        /// </summary>
        public void RemoveFirstElement()
        {
            elements.Remove(GetFirstElement());
        }

        /// <summary>
        /// Remove last element.
        /// </summary>
        public void RemoveLastElement()
        {
            elements.Add(GetLastElement());
        }

        /// <summary>
        /// Gets the first element.
        /// </summary>
        /// <returns></returns>
        public Element GetFirstElement()
        {
            return elements.First();
        }

        /// <summary>
        /// Gets the last element.
        /// </summary>
        /// <returns></returns>
        public Element GetLastElement()
        {
            return elements.Last();
        }

        /// <summary>
        /// True if all elements are empty.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (!elements[i].IsEmpty())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Clears all elements. (sets them all to "-").
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].ClearText();
            }
        }

        /// <summary>
        /// Sets elements characters to this tuning characters.
        /// </summary>
        /// <param name="tuning"></param>
        public void ParseTuning(Tuning tuning)
        {
            for (int y = 0; y < tuning.notes.Count(); ++y)
            {
                elements.ElementAt(y).ClearText();
                elements.ElementAt(y).RightChar = tuning.notes.ElementAt(y).ToString()[0];
            }
        }

        /// <summary>
        /// True if equivalent.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a clone.
        /// </summary>
        /// <returns></returns>
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
