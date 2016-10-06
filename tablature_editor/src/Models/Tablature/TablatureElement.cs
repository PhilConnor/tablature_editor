namespace PFE.Models
{
    public class TablatureElement
    {
        // Properties.
        public string Text { get; set; }

        //Constructors.
        public TablatureElement()
        {
            ClearText();
        }

        //Public Methods.
        public void ClearText()
        {
            Text = "-";
        }
    }
}
