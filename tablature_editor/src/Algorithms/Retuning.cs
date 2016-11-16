using PFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Algorithms
{
    // in progress
    public static class Retuning
    {
        public static int? AttemptRetuneFret(int fret, Note initialStringTuning, Note newStringTuning)
        {
            int deltaNoteSemiTones = initialStringTuning.CalculateDistance(newStringTuning);
            int newFretNumber = fret + deltaNoteSemiTones;

            if (!IsValidFret(newFretNumber))
                return null;
            else
                return newFretNumber;
        }

        public static bool IsValidFret(int fretNumber)
        {
            return (fretNumber >= 0 && fretNumber <= 99);
        }
    }
}
