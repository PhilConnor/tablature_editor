using PFE.Utils;

namespace PFE.Models
{
    public class Element
    {
        // Properties.
        public char Character { get; set; }

        //Constructors.
        public Element()
        {
            ClearText();
        }

        //Public Methods.
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
