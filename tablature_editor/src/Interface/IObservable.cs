using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tablature_editor.src.Interfaces
{
    public interface IObserverable
    {
        void Subscribe(IObserver observer);
    }
}
