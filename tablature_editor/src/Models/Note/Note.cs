using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Models
{
    //TODO commenting this class
    public class Note
    {
        public int Octave = 0;
        public NotesEnum Value = NotesEnum.A;
        
        public Note(int Octave, NotesEnum NotesEnumValue)
        {
            this.Octave = Octave;
            this.Value = NotesEnumValue;
        }

        public int CalculateDistance(Note note)
        {
            return this.GetNumericalEquivalent() - note.GetNumericalEquivalent();
        }

        public int GetNumericalEquivalent()
        {
            return Octave * 12 + (int)Value;
        }

        public char GetNoteChar()
        {
            return Enum.GetName(typeof(NotesEnum), Value)[0];
        }

        public enum NotesEnum { A, As, B, C, Cs, D, Ds, E, F, Fs, G, Gs };
    }
}
