using PFE.Configs;
using PFE.Utils;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace PFE.Models
{
    /// <summary>
    /// Represent a character on the tablature.
    /// Is the basic entity of the tablature.
    /// Event empty spaces are represented by an 
    /// Element with a '-' character value.
    /// </summary>
    public class Element
    {
        #region draw related
        /// <summary>
        /// Code in this region has been added to prevent unecessary creation of FormattedText object at each redraw
        /// </summary>
        public bool HasRightCharChanged { get; set; }

        private FormattedText rightCharFormattedText;
        public FormattedText RightCharFormattedText
        {
            get
            {
                if (HasRightCharChanged)
                    rightCharFormattedText = new FormattedText(
                        RightChar.ToString(),
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        Config_DrawSurface.Inst().TextFont,
                        Config_DrawSurface.Inst().FontSize,
                        Config_DrawSurface.Inst().TextColor);

                return rightCharFormattedText;
            }
        }

        public bool HasLeftCharChanged { get; set; }

        private FormattedText leftCharFormattedText;
        public FormattedText LeftCharFormattedText
        {
            get
            {
                if (HasLeftCharChanged)
                    leftCharFormattedText = new FormattedText(
                        LeftChar.ToString(),
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        Config_DrawSurface.Inst().TextFont,
                        Config_DrawSurface.Inst().FontSize,
                        Config_DrawSurface.Inst().TextColor);

                return leftCharFormattedText;
            }
        }
        #endregion

        //The main character of this element.
        private char rightChar;
        public char RightChar
        {
            get { return this.rightChar; }
            set { this.rightChar = value; HasRightCharChanged = true; }
        }

        //The left character of this element.
        //Is null if there is no left character AKA its not a digit over 9.
        private char? leftChar;
        public char? LeftChar
        {
            get { return this.leftChar; }
            set { this.leftChar = value; HasLeftCharChanged = true; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Element()
        {
            ClearText();
        }

        /// <summary>
        /// Clears the content of this element by settign it back to a "-".
        /// </summary>
        public void ClearText()
        {
            LeftChar = null;
            RightChar = '-';
        }

        /// <summary>
        /// True if this element possess no value.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return LeftChar == null && RightChar == '-';
        }

        /// <summary>
        /// True is this is a numerical note and not a modifier.
        /// </summary>
        /// <returns></returns>
        public bool IsNote()
        {
            return Util.IsNumber(RightChar);
        }

        /// <summary>
        /// True if this note value is greater than 9.
        /// </summary>
        /// <returns></returns>
        public bool IsNoteOver9()
        {
            return LeftChar != null;
        }

        /// <summary>
        /// True if this note has a value under 10.
        /// </summary>
        /// <returns></returns>
        public bool IsNoteUnder10()
        {
            return Util.IsNumber(RightChar) && LeftChar == null;
        }

        /// <summary>
        /// Set this note value to the input if possible.
        /// </summary>
        /// <param name="value"></param>
        public void ParseInt(int value)
        {
            if (!Util.IsValidFret(value))
                throw new System.Exception("Debug");

            this.ClearText();

            string valueString = value.ToString();
            RightChar = valueString[valueString.Length - 1];

            if (valueString.Length > 1)
                LeftChar = valueString[valueString.Length - 2];
        }

        /// <summary>
        /// Returns the numerical value if this element is a note.
        /// Otherwise throws an exception.
        /// </summary>
        /// <returns></returns>
        public int GetNoteNumericalValue()
        {
            int value = 0;

            if (!IsNote())
                throw new System.Exception("Debut");

            value += int.Parse(RightChar.ToString());

            if (IsNoteOver9())
                value += int.Parse(LeftChar.ToString()) * 10;

            return value;
        }

        /// <summary>
        /// True if element is equivalent to this one.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool Equals(Element element)
        {
            return LeftChar == element.LeftChar && RightChar == element.RightChar;
        }

        /// <summary>
        /// Returns a clone.
        /// </summary>
        /// <returns></returns>
        public Element Clone()
        {
            Element clone = new Element();
            clone.LeftChar = this.LeftChar;
            clone.RightChar = this.RightChar;
            return clone;
        }
    }
}
