using PFE.Models;
using PFE.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Algorithms
{
    /// <summary>
    /// Notes and fret related algorithms.
    /// </summary>
    public static class NoteConversion
    {
        /// <summary>
        /// Returns the number of the fret that the note could be played on this string 
        /// based on this it's tuning.
        /// </summary>
        /// <param name="note"></param>
        /// <param name="stringTuning"></param>
        /// <returns></returns>
        public static int? NoteToFret(Note note, Note stringTuning)
        {
            int? fretNumber = null;

            // note to low for string
            if (!IsNoteContainedInString(note, stringTuning))
                return null;

            fretNumber = note.GetNumericalEquivalent()
                - stringTuning.GetNumericalEquivalent();

            return fretNumber;
        }

        /// <summary>
        /// True if the note can be played on this string according to it's tuning.
        /// </summary>
        /// <param name="note"></param>
        /// <param name="stringTuning"></param>
        /// <returns></returns>
        public static bool IsNoteContainedInString(Note note, Note stringTuning)
        {
            // note to low for string
            if (note.GetNumericalEquivalent()
                < stringTuning.GetNumericalEquivalent())
                return false;

            // note to high for conventional guitar/bass (24 frets)
            if (note.GetNumericalEquivalent()
                > stringTuning.GetNumericalEquivalent() + 24)
                return false;

            return true;
        }

        /// <summary>
        /// Returns a list of the strings from the tuning containing the note.
        /// </summary>
        /// <param name="note"></param>
        /// <param name="tuning"></param>
        /// <returns></returns>
        public static List<Note> GetStringsContainingNotes(Note note, Tuning tuning)
        {
            List<Note> strings = new List<Note>();

            foreach (Note stringTuning in tuning.notes)
            {
                if (IsNoteContainedInString(note, stringTuning))
                    strings.Add(stringTuning.Clone());
            }

            return strings;
        }

        public static List<int> GetStringsIndexContainingNotes(Note note, Tuning tuning)
        {
            List<int> strings = new List<int>();

            int y = 0;
            foreach (Note stringTuning in tuning.notes)
            {
                if (IsNoteContainedInString(note, stringTuning))
                    strings.Add(y);

                y++;
            }

            return strings;
        }
    }
}
