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
        public char Character { get; set; }

        public Element()
        {
            ClearText();
        }

        public void ClearText()
        {
            Character = '-';
        }

        public bool IsNumber()
        {
            return Util.IsNumber(Character);
        }
    }
}
