using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Models
{
    /// <summary>
    /// Represents the tuning of the instruments.
    /// The number of string can be infered from the 
    /// number of notes of the tuning.
    /// </summary>
    public class Tuning
    {
        public List<Note> notes;

        /// <summary>
        /// Constructs a 6 string standard tuning 
        /// </summary>
        public Tuning()
        {
            SetToStandard();
        }

        /// <summary>
        /// Returns the number of strings for this tuning.
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfString()
        {
            return notes.Count();
        }

        /// <summary>
        /// Setting this tuning to standard 6 string tuning
        /// E2-A2-D3-G3-B3-E4
        /// </summary>
        public void SetToStandard()
        {
            notes = new List<Note>();
            notes.Add(new Note(2, Note.NotesEnum.E));
            notes.Add(new Note(2, Note.NotesEnum.A));
            notes.Add(new Note(3, Note.NotesEnum.D));
            notes.Add(new Note(3, Note.NotesEnum.G));
            notes.Add(new Note(3, Note.NotesEnum.B));
            notes.Add(new Note(4, Note.NotesEnum.E));
        }
    }
}
