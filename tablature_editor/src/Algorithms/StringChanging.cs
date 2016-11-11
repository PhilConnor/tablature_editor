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
        /// </summary>
        /// <param name="tablature"></param>
        public static void MoveFirstStringNotesDown(Tablature tablature)
        {
            //if (tablature.NStrings < 2)
            //    return;

            //foreach(Position p in tablature.positions)
            //{
            //    Element FirstElem = p.elements[0];
            //    Element SecondElem = p.elements[1];
                
            //}
        }

        /// <summary>
        /// Move all notes from the last string to the second-last string if possible.
        /// Will do nothing if the move failed because of a string already over this one.
        /// </summary>
        /// <param name="tablature"></param>
        public static void MoveLastStringNotesUp(Tablature tablature)
        {
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
