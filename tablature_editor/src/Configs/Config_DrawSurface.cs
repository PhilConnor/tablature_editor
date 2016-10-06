using System.Windows.Media;

namespace PFE.Configs
{
    public static class Config_DrawSurface
    {

        // Canvas.
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Window_Width { get; set; }
        public static int Window_Height { get; set; }
        public static int CharWidth { get; set; }
        public static int CharHeight { get; set; }
        public static int LineThickness { get; set; }
        public static int FontSize { get; set; }
        public static int SpacingBetweenStaff { get; set; }

        // Grid resolution.
        public static int GridUnitWidth { get; set; }
        public static int GridUnitHeight { get; set; }

        // Tab pages margins.
        public static int MarginX { get; set; }
        public static int MarginY { get; set; }

        // Colors
        public static Color BGColor { get; set; }
        public static Brush TextColor { get; set; }

        public static Typeface TextFont { get; set; }

        //Constructors.
        public static void Initialisation()
        {
            // Maybe editable in the future.
            FontSize = 14;
            GridUnitWidth = FontSize;
            GridUnitHeight = GridUnitWidth;
            SpacingBetweenStaff = 1;
            Width = GridUnitWidth * 81;
            Height = GridUnitHeight * 200;
            Window_Width = GridUnitWidth * 83;
            Window_Height = GridUnitHeight * 200;
            CharWidth = 14;
            CharHeight = 14;
            LineThickness = 14;
            MarginX = 14;
            MarginY = 14;

            BGColor = Colors.LightGray;
            TextColor = Brushes.White;
            TextFont = new Typeface("Verdana");

        }
    }
}






