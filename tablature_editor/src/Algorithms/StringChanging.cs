using PFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Algorithms
{
    // in progress
    public class StringChanger
    {
        /// <summary>
        /// Move all notes from the first string to the second string if possible.
        /// Will do nothing if the move failed because of a string already under this one.
        /// TODO: allow insertion and tabAdjustement if a note is already there.
        /// </summary>
        /// <param name="tablature"></param>
        public static void MoveFirstStringNotesDown(Tablature tablature)
        {
            if (tablature.NStrings < 2)
                return;

            Note firstStringTuning = tablature.Tuning.notes[0];
            Note secondStringTuning = tablature.Tuning.notes[1];

            int deltaSemiTones = secondStringTuning.GetNumericalEquivalent()
                - firstStringTuning.GetNumericalEquivalent();

            for (int x = 0; x < tablature.Length; x++)
            {
                TabCoord currentInitialStringTC = new TabCoord(x, 0);
                TabCoord currentTargetStringTC = new TabCoord(x, 1);
                Element elementThere = tablature.ElementAt(currentInitialStringTC);

                if (elementThere.IsNumber())
                {
                    int newValue = elementThere.GetNumericalValue() + deltaSemiTones;

                    if (!tablature.IsACharThere(currentTargetStringTC) &&
                        !tablature.IsACharThere(currentTargetStringTC.CoordOnLeft()) &&
                        !tablature.IsACharThere(currentTargetStringTC.CoordOnLeft().CoordOnLeft()) &&
                        !tablature.IsACharThere(currentTargetStringTC.CoordOnRight())
                        )
                    {
                        tablature.ElementAt(currentTargetStringTC).ParseInteger(newValue);
                    }
                }
            }
        }

        /// <summary>
        /// Move all notes from the last string to the second-last string if possible.
        /// Will do nothing if the move failed because of a string already over this one.
        /// TODO: allow insertion and tabAdjustement if a note is already there.
        /// </summary>
        /// <param name="tablature"></param>
        public static void MoveLastStringNotesUp(Tablature tablature)
        {
            if (tablature.NStrings < 2)
                return;

            Note firstStringTuning = tablature.Tuning.notes[tablature.NStrings - 1];
            Note secondStringTuning = tablature.Tuning.notes[tablature.NStrings - 2];

            int deltaSemiTones = secondStringTuning.GetNumericalEquivalent()
                - firstStringTuning.GetNumericalEquivalent();

            for (int x = 0; x < tablature.Length; x++)
            {
                TabCoord currentInitialStringTC = new TabCoord(x, tablature.NStrings - 1);
                TabCoord currentTargetStringTC = new TabCoord(x, tablature.NStrings - 2);
                Element elementThere = tablature.ElementAt(currentInitialStringTC);

                if (elementThere.IsNumber())
                {
                    int newValue = elementThere.GetNumericalValue() + deltaSemiTones;

                    if (!tablature.IsACharThere(currentTargetStringTC) &&
                        !tablature.IsACharThere(currentTargetStringTC.CoordOnLeft()) &&
                        !tablature.IsACharThere(currentTargetStringTC.CoordOnLeft().CoordOnLeft()) &&
                        !tablature.IsACharThere(currentTargetStringTC.CoordOnRight())
                        )
                    {
                        tablature.ElementAt(currentTargetStringTC).ParseInteger(newValue);
                    }
                }
            }
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
