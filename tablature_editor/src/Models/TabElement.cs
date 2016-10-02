namespace TablatureEditor.Models
{
    public class TabElement
    {
        // Properties.
        public string Text { get; set; }

        //Constructors.
        public TabElement()
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
