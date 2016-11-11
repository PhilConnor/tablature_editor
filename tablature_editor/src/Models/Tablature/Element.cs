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
    //    new FormattedText(
    //chr.ToString(),
    //               CultureInfo.GetCultureInfo("en-us"),
    //               FlowDirection.LeftToRight,
    //               Config_DrawSurface.Inst().TextFont,
    //               Config_DrawSurface.Inst().FontSize,
    //               Brushes.White);

        #region draw related
        public bool HasChanged { get; set; }

        private FormattedText rightCharFormattedText;
        public FormattedText RightCharFormattedText
        {
            get
            {
                if(HasChanged)
                    rightCharFormattedText = new FormattedText(
                        RightChar.ToString(),
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        Config_DrawSurface.Inst().TextFont,
                        Config_DrawSurface.Inst().FontSize,
                        Brushes.White);
                
                return rightCharFormattedText;
            }
        }

        private FormattedText leftCharFormattedText;
        public FormattedText LeftCharFormattedText
        {
            get
            {
                if (HasChanged)
                    leftCharFormattedText = new FormattedText(
                        LeftChar.ToString(),
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        Config_DrawSurface.Inst().TextFont,
                        Config_DrawSurface.Inst().FontSize,
                        Brushes.White);

                return leftCharFormattedText;
            }
        }
        #endregion

        // the main char, if its a number over9 the second digit is stored in leftchar
        private char rightChar;
        public char RightChar 
        {
            get { return this.rightChar; }
            set { this.rightChar = value; HasChanged = true; }
        }

        private char leftChar;
        public char LeftChar
        {
            get { return this.leftChar; }
            set { this.leftChar = value; HasChanged = true; }
        }


        public Element()
        {
            HasChanged = true;
            ClearText();
        }

        public void ClearText()
        {
            LeftChar = '-';
            RightChar = '-';
        }

        public bool IsEmpty()
        {
            return LeftChar == '-' && RightChar == '-';
        }
        
        public bool IsNumber()
        {
            return Util.IsNumber(RightChar);
        }

        public bool IsNumberOver9()
        {
            return Util.IsNumber(LeftChar);
        }

        public bool IsNumberUnder9()
        {
            return Util.IsNumber(RightChar) && !Util.IsNumber(LeftChar);
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
