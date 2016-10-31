using System.Collections.Generic;
using System.Linq;
using PFE.Configs;
using PFE.Utils;

namespace PFE.Models
{
    public class Clipboard
    {
        public List<List<char>> Chars;

        public Clipboard()
        {

        }

        //// For this example, the data to be placed on the clipboard is a simple
        //// string.
        //string textData = "I want to put this string on the clipboard.";

        //// After this call, the data (string) is placed on the clipboard and tagged
        //// with a data format of "Text".
        //Clipboard.SetData(DataFormats.Text, (Object)textData)
    }
}
