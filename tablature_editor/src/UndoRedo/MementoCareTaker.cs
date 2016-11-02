using PFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.UndoRedo
{
    public class MementoCareTaker
    {
        private int capacity = 10;
        private int maxIndex;
        private int currentIndex;
        private int lastIndex;
        private Memento[] mementos;

        public MementoCareTaker(Memento initialMemento)
        {
            mementos = new Memento[capacity];
            maxIndex = capacity - 1;
            lastIndex = 0;
            currentIndex = 0;
            mementos[currentIndex] = initialMemento;
        }

        public Memento CurrentMemento
        {
            get
            {
                return mementos[currentIndex];
            }
        }

        public void AddMemento(Memento newMemento)
        {
            // if the current memento is equal to new memento, do nothing
            Memento currentMemento = mementos[currentIndex];
            if (currentMemento.Equals(newMemento))
            {
                return;
            }

            if (currentIndex == maxIndex)
            {
                Array.Copy(mementos, 1, mementos, 0, mementos.Length - 1);
            }
            else if (currentIndex < lastIndex)
            {
                //if currentIndex is behind lastIndex at the moment of adding a 
                //new memento, we clear mementos after currentIndex
                for (int i = currentIndex + 1; i <= maxIndex; i++)
                    mementos[i] = null;

                currentIndex++;
                lastIndex = currentIndex;
            }
            else if (currentIndex < maxIndex)
            {
                currentIndex++;
                lastIndex = currentIndex;
            }

            mementos[currentIndex] = newMemento;
        }

        public Memento Undo()
        {
            currentIndex = Math.Max(0, --currentIndex);

            return CurrentMemento;
        }

        public Memento Redo()
        {
            currentIndex = Math.Min(lastIndex, ++currentIndex);

            return CurrentMemento;
        }


    }
}
