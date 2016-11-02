using PFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.UndoRedo
{
    public class Memento
    {
        private Cursor cursor;
        private Tablature tablature;

        public Memento(Cursor cursor, Tablature tablature)
        {
            this.cursor = cursor.Clone();
            this.tablature = tablature.Clone();
        }

        public Cursor Cursor
        {
            get
            {
                return cursor.Clone();
            }
        }

        public Tablature Tablature
        {
            get
            {
                return tablature.Clone();
            }
        }

        public bool Equals(Memento memento)
        {
            return cursor.Equals(memento.cursor) && tablature.Equals(memento.tablature);
        }
    }
}
