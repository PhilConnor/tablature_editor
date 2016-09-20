using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace tablature_editor
{
    public class TabPosition
    {
        //from top to bottom
        public List<TabElement> _elements;

        public TabPosition()
        {
            _elements = new List<TabElement>(Configuration.Inst.NStringPerStaff);

            for (int y = 0; y < Configuration.Inst.NStringPerStaff; y++)
                _elements.Add(new TabElement());
        }

        public void parseString(string stringTabElements)
        {
            string[] separators = { ":" };
            string[] c = stringTabElements.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            for (int y = 0; y < _elements.Count(); y++)
                _elements.ElementAt(y).Text = c.ElementAt(_elements.Count() - 1 - y);
        }

        public void parseTuning(string tuning)
        {
            char[] tuningCharArray = tuning.ToCharArray();

            for (int y = 0; y < tuningCharArray.Count(); y++)
                _elements.ElementAt(y).Text = tuningCharArray.ElementAt(_elements.Count() - 1 - y).ToString();
        }
    }
}
