using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Models
{
    public class Tuning
    {
        public List<Note> notes;

        public Tuning()
        {
            SetToStandard();
        }

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
