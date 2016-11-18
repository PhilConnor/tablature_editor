using PFE.Models;
using PFE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Algorithms
{
    /// <summary>
    /// Retuning related algorithms.
    /// </summary>
    public static class Retuning
    {
        /// <summary>
        /// Attempts to retune the fret based on initial and new tuning of the string.
        /// Returns null if retuning is impossible.
        /// Returns the new fret number if retuning is possible.
        /// </summary>
        /// <param name="fret"></param>
        /// <param name="initialStringTuning"></param>
        /// <param name="newStringTuning"></param>
        /// <returns></returns>
        public static int? AttemptRetuneFret(int fret, Note initialStringTuning, Note newStringTuning)
        {
            int deltaNoteSemiTones = initialStringTuning.CalculateDistance(newStringTuning);
            int newFretNumber = fret + deltaNoteSemiTones;

            if (!Util.IsValidFret(newFretNumber))
                return null;
            else
                return newFretNumber;
        }


        public static void AttemptRetuneTablature(Tablature tablature, Tuning newTuning, bool scaleNotes)
        {
            for (int i = 0; i < tablature.Tuning.notes.Count; i++)
            {
                AttemptRetuneString(tablature, i, newTuning.notes[i], scaleNotes);
            }
        }

        public static void AttemptRetuneString(Tablature tablature, int stringIndex, Note newStringTuning, bool scaleNotes)
        {
            if (scaleNotes)
            {
                for (int i = 0; i < tablature.Length; i++)
                {
                    TabCoord tc = new TabCoord(i, stringIndex);
                    if (tablature.ElementAt(tc).IsNote())
                    {
                        int fret = tablature.ElementAt(tc).GetFretNumber();
                        int? newFret = AttemptRetuneFret(fret, tablature.Tuning.notes[stringIndex], newStringTuning);

                        if (newFret != null)
                        {
                            tablature.ChangeNoteAt(tc, newFret.Value);
                        }
                    }
                }
            }
            
            tablature.Tuning.notes[stringIndex] = newStringTuning;
        }
    }
}
