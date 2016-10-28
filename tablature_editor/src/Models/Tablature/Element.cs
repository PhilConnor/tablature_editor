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
    }
}
