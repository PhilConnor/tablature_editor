using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace tablature_editor
{
    public class Configuration
    {
        private static Configuration instance;

        public static Configuration Inst
        {
            get
            {
                if (instance == null) { instance = new Configuration(); }
                return instance;
            }
        }

        // tablature parameters
        private string tuning;
        public string Tuning
        {
            get { return tuning; }
            set { tuning = value; }
        }

        private int nStaff;
        public int NStaff
        {
            get { return nStaff; }
            set { nStaff = value; }
        }

        public int staffLength;

        // canvas related
        public int canvasWidth;
        public int canvasHeight;
        public int charWidth;
        public int charHeight;
        public int lineTickness;
        public int fontSize;
        public int staffSpacingUnitY;

        // grid resolution
        public int unitSizeX;
        public int unitSizeY;

        // tab pages margins
        public int marginX;
        public int marginY;

        // colors
        public Color bgColor;

        private Configuration()
        {
            //editable
            NStaff = 3;
            staffLength = 80;
            Tuning = "EADGBe";

            //maybe editable in the future
            fontSize = 12;
            unitSizeX = fontSize;
            unitSizeY = unitSizeX;// * 2;
            staffSpacingUnitY = 2;
            canvasWidth = unitSizeX * (staffLength + 1);
            canvasHeight = unitSizeY * 200;
            charWidth = 12;
            charHeight = 12;
            lineTickness = 12;
            marginX = 10;
            marginY = 10;
            bgColor = Colors.LightGray;
        }

        public int TabLength
        {
            get
            {
                return NStaff * staffLength;
            }
        }

        public int NStringPerStaff
        {
            get
            {
                return Tuning.Length;
            }
        }

    } //
}






