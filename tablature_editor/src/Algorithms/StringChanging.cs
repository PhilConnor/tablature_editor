using PFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Algorithms
{
    // in progress
    public class StringChanging
    {

        public static void MoveStringNotesToOtherString(Tablature tablature, int initialStringIndex, int targetStringIndex)
        {
            if (tablature.NStrings < 2
                || initialStringIndex < 0
                || initialStringIndex >= tablature.Length
                || targetStringIndex < 0
                || targetStringIndex >= tablature.Length)
                throw new Exception("Debug");

            Note initialStringTuning = tablature.Tuning.notes[initialStringIndex];
            Note targetStringTuning = tablature.Tuning.notes[targetStringIndex];

            for (int x = 0; x < tablature.Length; x++)
            {
                TabCoord currentInitialStringTC = new TabCoord(x, initialStringIndex);
                TabCoord currentTargetStringTC = new TabCoord(x, targetStringIndex);
                Element elementThere = tablature.ElementAt(currentInitialStringTC);

                if (elementThere.IsNumber())
                {
                    int? newFretNumberTarget = Retuning.AttemptRetuneFret(
                        elementThere.GetNumericalValue(),
                        initialStringTuning,
                        targetStringTuning);

                    if (newFretNumberTarget.HasValue &&
                        tablature.CanAddNoteAt(currentTargetStringTC, newFretNumberTarget.Value))
                    {
                        tablature.ElementAt(currentTargetStringTC).ParseInteger(newFretNumberTarget.Value);
                    }
                }
            }
        }


        /// <summary>
        /// Move all notes from the first string to the second string if possible.
        /// Will do nothing if the move failed because of a string already under this one.
        /// TODO: allow insertion and tabAdjustement if a note is already there.
        /// </summary>
        /// <param name="tablature"></param>
        public static void MoveFirstStringNotesDown(Tablature tablature)
        {
            MoveStringNotesToOtherString(tablature, 0, 1);
        }

        /// <summary>
        /// Move all notes from the last string to the second-last string if possible.
        /// Will do nothing if the move failed because of a string already over this one.
        /// TODO: allow insertion and tabAdjustement if a note is already there.
        /// </summary>
        /// <param name="tablature"></param>
        public static void MoveLastStringNotesUp(Tablature tablature)
        {
            MoveStringNotesToOtherString(tablature, tablature.NStrings - 1, tablature.NStrings - 2);
        }

        /// <summary>
        /// Changes the tuning of the tablature for this new tuning without changing any notes.
        /// Will do nothing if the new tuning has a different number of string than the current one.
        /// </summary>
        /// <param name="tablature"></param>
        /// <param name="tuning"></param>
        public static void ChangeTuning(Tablature tablature, Tuning tuning)
        {
        }

        /// <summary>
        /// Changes the tuning of the tablature for this new tuning and attemps to transpose notes on the same string to keep the same pitch.
        /// Will do nothing to notes that are impossible to transpose on the same string.
        /// Will do nothing if the new tuning has a different number of string than the current one.
        /// </summary>
        /// <param name="tablature"></param>
        /// <param name="tuning"></param>
        public static void ChangeTuningWithTransposition(Tablature tablature, Tuning tuning)
        {
        }
    }
}
