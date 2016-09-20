using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using tablature_editor.src.Interfaces;

namespace tablature_editor
{
    public class Tab
    {
        public List<TabPosition> _positions;

        public Tab()
        {
            init();
        }

        public void init()
        {
            _positions = new List<TabPosition>(Configuration.Inst.TabLength);

            for (int x = 0; x < Configuration.Inst.TabLength; x++)
                _positions.Add(new TabPosition());

            tabPositionAt(0).parseTuning(Configuration.Inst.Tuning);
        }
        public int Length()
        {
            return _positions.Count();
        }

        public int StringCount()
        {
            return _positions.ElementAt(0)._elements.Count();
        }

        public void setTextAt(Coord tabCoord, string elementText)
        {
            //if we are about to write a 10th or 20th we remove the 
            // char occuring before it to make space for this extra character
            if (elementText.Length > 1 && tabCoord.y > 1)
            {
                _positions.ElementAt(tabCoord.x)._elements.ElementAt(tabCoord.y - 1).clearText();
            }

            _positions.ElementAt(tabCoord.x)._elements.ElementAt(tabCoord.y).Text = elementText;
        }

        public string getTextAt(Coord tabCoord)
        {
            return _positions.ElementAt(tabCoord.x)._elements.ElementAt(tabCoord.y).Text;
        }

        private TabElement tabElementAt(Coord tabCoord)
        {
            return _positions.ElementAt(tabCoord.x)._elements.ElementAt(tabCoord.y);
        }

        private TabPosition tabPositionAt(int tabCoord_X)
        {
            return _positions.ElementAt(tabCoord_X);
        }

        public void removePosition(int tabCoord_X)
        {
            _positions.RemoveAt(tabCoord_X);
        }


    }
}
