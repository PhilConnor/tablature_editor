using PFE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Models
{
    // Temporary implementation for demo purpose.
    public static class Chords
    {
        public static List<int?> GetChordFretsC()
        {
            List<int?> frets = new List<int?>();

            frets.Add(2);
            frets.Add(3);
            frets.Add(2);
            frets.Add(0);
            frets.Add(null);
            frets.Add(null);

            return frets;
        }

        public static List<int?> GetChordFretsG()
        {
            List<int?> frets = new List<int?>();

            frets.Add(3);
            frets.Add(3);
            frets.Add(0);
            frets.Add(0);
            frets.Add(2);
            frets.Add(3);

            return frets;
        }

        public static List<int?> GetChordFretsE()
        {
            List<int?> frets = new List<int?>();

            frets.Add(0);
            frets.Add(0);
            frets.Add(0);
            frets.Add(2);
            frets.Add(2);
            frets.Add(0);

            return frets;
        }

    }
}
