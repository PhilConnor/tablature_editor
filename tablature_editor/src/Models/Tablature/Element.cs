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

        // the main char, if it's a number over9 the second digit is stored in leftchar
        private char rightChar;
        public char RightChar
        {
            get { return this.rightChar; }
            set { this.rightChar = value; HasRightCharChanged = true; }
        }

        private char? leftChar;
        public char? LeftChar
        {
            get { return this.leftChar; }
            set { this.leftChar = value; HasLeftCharChanged = true; }
        }


        public Element()
        {
            ClearText();
        }

        public void ClearText()
        {
            LeftChar = null;
            RightChar = '-';
        }

        public bool IsEmpty()
        {
            return LeftChar == null && RightChar == '-';
        }

        public bool IsNumber()
        {
            return Util.IsNumber(RightChar);
        }

        public bool IsNumberOver9()
        {
            return LeftChar != null;
        }

        public bool IsNumberUnder9()
        {
            return Util.IsNumber(RightChar) && LeftChar == null;
        }

        public void ParseInteger(int value)
        {
            this.ClearText();

            string valueString = value.ToString();
            RightChar = valueString[valueString.Length - 1];

            if (valueString.Length > 1)
                LeftChar = valueString[valueString.Length - 2];
        }

        public int GetNumericalValue()
        {
            int value = 0;

            if (!IsNumber())
                throw new System.Exception();

            value += int.Parse(RightChar.ToString());

            if (IsNumberOver9())
                value += int.Parse(LeftChar.ToString()) * 10;

            return value;
        }

        public bool Equals(Element element)
        {
            return LeftChar == element.LeftChar && RightChar == element.RightChar;
        }

        public Element Clone()
        {
            Element clone = new Element();
            clone.LeftChar = this.LeftChar;
            clone.RightChar = this.RightChar;
            return clone;
        }
    }
}
