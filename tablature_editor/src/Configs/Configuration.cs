using System.Windows.Media;

namespace TablatureEditor.Configs
{
    // Contains de configurations for the current tablature.
    public static class Configuration
    {
        // Tuning and number of string.
        public static string Tuning { get; set; } // Tablature parameters.
        public static int NumberOfStrings
        {
            get
            {
                return Tuning.Length;
            }
        }

        // Number of staffs.
        public static int NStaff { get; set; }
        public static int StaffLength { get; set; }

        public static int TabLength
        {
            get
            {
                return NStaff * StaffLength;
            }
        }

        // Canvas.
        public static int CanvasWidth { get; set; }
        public static int CanvasHeight { get; set; }
        public static int CanvasCharWidth { get; set; }
        public static int CanvasCharHeight { get; set; }
        public static int CanvasLineTickness { get; set; }
        public static int CanvasFontSize { get; set; }
        public static int CanvasStaffSpacingUnitY { get; set; }

        // Grid resolution.
        public static int UnitSizeX { get; set; }
        public static int UnitSizeY { get; set; }

        // Tab pages margins.
        public static int MarginX { get; set; }
        public static int MarginY { get; set; }

        // Colors
        public static Color BGColor { get; set; }
        
        //Constructors.
        public static void Initialisation()
        {
            // Editable.
            NStaff = 3;
            StaffLength = 80;
            Tuning = "EADGBe";

            // Maybe editable in the future.
            CanvasFontSize = 12;
            UnitSizeX = CanvasFontSize;
            UnitSizeY = UnitSizeX;
            CanvasStaffSpacingUnitY = 2;
            CanvasWidth = UnitSizeX * (StaffLength + 1);
            CanvasHeight = UnitSizeY * 200;
            CanvasCharWidth = 12;
            CanvasCharHeight = 12;
            CanvasLineTickness = 12;
            MarginX = 10;
            MarginY = 10;
            BGColor = Colors.LightGray;
        }
    }
}






