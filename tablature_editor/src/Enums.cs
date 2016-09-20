using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tablature_editor.src
{
    public class Enums
    {
        public enum WriteModes { Unity, Tenth, Twenyth, Thirtieth };
        public enum SkipModes { Zero, One };
        public enum CursorMovements { Left, Up, Right, Down, ExpandLeft, ExpandUp, ExpandRight, ExpandDown };
    }
}
