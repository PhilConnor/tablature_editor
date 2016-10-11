using System.Collections.Generic;
using System.Linq;
using PFE.Configs;
using PFE.Utils;

namespace PFE.Models
{
    /// <summary>
    /// Represents the tablature itself. 
    /// Contains all tab elements and the tuning.
    /// </summary>
    public class Tablature
    {
        #region properties
        /// <summary>
        /// The tab positions from left to right.
        /// Index 0 is the left most tab position.
        /// </summary>
        public List<Position> positions;

        /// <summary>
        /// The current tuning as a string.
        /// </summary>
        public string Tuning { get; set; } // Tablature parameters.

        /// <summary>
        /// The number of strings.
        /// </summary>
        public int NStrings { get { return Tuning.Length; } }

        /// <summary>
        /// The number of staffs in the tablature.
        /// </summary>
        public int NStaff { get; set; }

        /// <summary>
        /// The length of a staff.
        /// </summary>
        public int StaffLength { get; set; }

        /// <summary>
        /// The total length of the tablature.
        /// </summary>
        public int Length { get { return positions.Count(); } }
        #endregion

        #region public
        /// <summary>
        /// Constructor
        /// </summary>
        public Tablature()
        {
            Init(3, 80, "EADGBe");
        }

        /// <summary>
        /// Inits the tablature to a black tablature with standard
        /// tuning and some other default values.
        /// </summary>
        public void Init(int nStaff, int staffLength, string tuning)
        {
            Tuning = tuning;
            StaffLength = staffLength;
            NStaff = nStaff;

            //fills the tablature with clean blank.
            positions = new List<Position>();
            for (int x = 0; x < nStaff * staffLength; ++x)
                positions.Add(new Position(NStrings));

            positions.ElementAt(0).ParseTuning(Tuning);
        }

        /// <summary>
        /// Set the char value of the element at tabCoord
        /// </summary>
        public void SetElementCharAt(TabCoord tabCoord, char elementChar)
        {
            if (tabCoord == null)
                return;

            ElementAt(tabCoord).Character = elementChar;
        }

        /// <summary>
        /// Returns the char value of the element at tabCoord
        /// </summary>
        public char GetElementCharAt(TabCoord tabCoord)
        {
            return ElementAt(tabCoord).Character;
        }

        /// <summary>
        /// Returns the element at tabCoord
        /// </summary>
        public Element ElementAt(TabCoord tabCoord)
        {
            if (!tabCoord.IsValid(this))
                return null;

            return positions.ElementAt(tabCoord.x).elements.ElementAt(tabCoord.y);
        }
        #endregion

        #region private

        #endregion
    }
}
