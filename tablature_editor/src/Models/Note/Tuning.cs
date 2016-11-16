﻿using PFE.Utils;
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
        /// Constructs a nString standard tuning with default E2 notes 
        /// </summary>
        public Tuning(int nString)
        {
            notes = new List<Note>(nString);
            for (int i = 0; i < nString; i++)
            {
                notes.Add(new Note(2, Note.NotesEnum.E));
            }
        }

        /// <summary>
        /// Add a string to the tuning.
        /// </summary>
        /// <param name="atEnd"></param>
        /// <param name="newStringNote"></param>
        public void AddString(bool atEnd, Note newStringNote)
        {
            if (atEnd)
                notes.Add(newStringNote);
            else
                notes.Insert(0, newStringNote);
        }

        /// <summary>
        /// Remove a string from the tuning.
        /// </summary>
        /// <param name="atEnd"></param>
        public void RemoveString(bool atEnd)
        {
            if (atEnd)
                notes.Remove(notes.Last());
            else
                notes.Remove(notes.First());
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
            notes.Add(new Note(4, Note.NotesEnum.E));
            notes.Add(new Note(3, Note.NotesEnum.B));
            notes.Add(new Note(3, Note.NotesEnum.G));
            notes.Add(new Note(3, Note.NotesEnum.D));
            notes.Add(new Note(2, Note.NotesEnum.A));
            notes.Add(new Note(2, Note.NotesEnum.E));
        }

        /// <summary>
        /// True if equivalent.
        /// </summary>
        /// <param name="tuning"></param>
        /// <returns></returns>
        public bool Equals(Tuning tuning)
        {
            if (tuning.notes.Count != notes.Count)
                return false;

            for (int i = 0; i < notes.Count; i++)
            {
                if (!notes[i].Equals(tuning.notes[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a clone.
        /// </summary>
        /// <returns></returns>
        public Tuning Clone()
        {
            Tuning clone = new Tuning(this.notes.Count);

            for (int i = 0; i < notes.Count; i++)
            {
                clone.notes[i] = notes[i].Clone();
            }

            return clone;
        }

        public string ToString()
        {
            string ascii = "";

            foreach (Note n in notes)
                ascii += n.ToStringWithOctave();

            return ascii;
        }

        public static Tuning ParseString(string str)
        {
            Tuning tuning = new Tuning();
            tuning.notes.Clear();

            char[] cs = str.ToCharArray();
            string tmp = "";
            foreach (char c in cs)
            {
                if (!Util.IsNumber(c))
                {
                    tmp += c.ToString();
                }
                else if (Util.IsNumber(c))
                {
                    tmp += c.ToString();
                    tuning.AddString(true, Note.ParseStringWithOctave(tmp));
                    tmp = "";
                }
            }

            return tuning;
        }
    }
}
