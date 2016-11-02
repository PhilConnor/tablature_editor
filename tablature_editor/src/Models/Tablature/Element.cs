using PFE.Utils;

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
        public char RightChar { get; set; } // the main char, if its a number over9 the second digit is stored in leftchar
        public char LeftChar { get; set; }

        public Element()
        {
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
