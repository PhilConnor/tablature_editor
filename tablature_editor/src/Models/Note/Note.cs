using System;
using System.Collections.Generic;
using System.Linq;

namespace PFE.Models
{
    //TODO commenting this class
    public class Note
    {
        //List of all of the possible notes.
        public enum NotesEnum { C, Cs, D, Ds, E, F, Fs, G, Gs, A, As, B, };

        public int Octave = 0;
        public NotesEnum Value = NotesEnum.C;

        public Note()
        {
            this.Octave = 0;
            this.Value = NotesEnum.C;
        }

        public Note(int Octave, NotesEnum NotesEnumValue)
        {
            this.Octave = Octave;
            this.Value = NotesEnumValue;
        }

        /// <summary>
        /// Returns the distance in semi-tones between the notes.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public int CalculateDistance(Note note)
        {
            return this.GetNumericalEquivalent() - note.GetNumericalEquivalent();
        }

        /// <summary>
        /// Get the absolute pitch number startingconsidering C0 = 0.
        /// </summary>
        /// <returns></returns>
        public int GetNumericalEquivalent()
        {
            return Octave * 12 + (int)Value;
        }

        /// <summary>
        /// Set this note based on a pitch absolute number considering C0 = 0.
        /// </summary>
        /// <param name="value"></param>
        public void SetNumericalEquivalent(int value)
        {
            Octave = value / 12;
            Value = (NotesEnum)(value % 12);
        }

        /// <summary>
        /// Get note in display format with or without the octave number.
        /// </summary>
        /// <param name="withOctave"></param>
        /// <returns></returns>
        public string GetNoteDisplayFormat(bool withOctave)
        {
            string result = "";

            //Get note in string format.
            switch (Value)
            {
                case NotesEnum.A:
                    result = "A";
                    break;
                case NotesEnum.As:
                    result = "A#";
                    break;
                case NotesEnum.B:
                    result = "B";
                    break;
                case NotesEnum.C:
                    result = "C";
                    break;
                case NotesEnum.Cs:
                    result = "C#";
                    break;
                case NotesEnum.D:
                    result = "D";
                    break;
                case NotesEnum.Ds:
                    result = "D#";
                    break;
                case NotesEnum.E:
                    result = "E";
                    break;
                case NotesEnum.F:
                    result = "F";
                    break;
                case NotesEnum.Fs:
                    result = "F#";
                    break;
                case NotesEnum.G:
                    result = "G";
                    break;
                case NotesEnum.Gs:
                    result = "G#";
                    break;
            }

            //Add octave value if needed.
            if (withOctave)
            {
                result += Octave;
            }

            return result;
        }

        /// <summary>
        /// String reprentation without octave number included.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetNoteDisplayFormat(false);
        }


        /// <summary>
        /// String reprentation with octave number included.
        /// </summary>
        /// <returns></returns>
        public string ToStringWithOctave()
        {
            return GetNoteDisplayFormat(true);
        }

        /// <summary>
        /// Returns a clone.
        /// </summary>
        /// <returns></returns>
        public Note Clone()
        {
            return new Note(Octave, Value);
        }

        /// <summary>
        /// True if equivalent.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool Equals(Note note)
        {
            return note.Octave == Octave && Value == note.Value;
        }

        /// <summary>
        /// Get a list of all the possible notes (in string format) to build a combobox. 
        /// The octave number is not important.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Note> GetListNotes()
        {
            List<Note> notes = new List<Note>();

            notes.Add(new Note(0, NotesEnum.C));
            notes.Add(new Note(0, NotesEnum.Cs));
            notes.Add(new Note(0, NotesEnum.D));
            notes.Add(new Note(0, NotesEnum.Ds));
            notes.Add(new Note(0, NotesEnum.E));
            notes.Add(new Note(0, NotesEnum.F));
            notes.Add(new Note(0, NotesEnum.Fs));
            notes.Add(new Note(0, NotesEnum.G));
            notes.Add(new Note(0, NotesEnum.Gs));
            notes.Add(new Note(0, NotesEnum.A));
            notes.Add(new Note(0, NotesEnum.As));
            notes.Add(new Note(0, NotesEnum.B));

            return notes.AsEnumerable<Note>();
        }
    }
}
