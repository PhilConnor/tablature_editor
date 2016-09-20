using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace tablature_editor
{
    /// <summary>
    /// 
    /// </summary>
    public class TabElement
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public TabElement()
        {
            clearText();
        }

        public void clearText()
        {
            Text = "-";
        }

    }
}
