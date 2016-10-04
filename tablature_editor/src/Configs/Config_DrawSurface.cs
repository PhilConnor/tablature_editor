using System.Windows.Media;

namespace TablatureEditor.Configs
{
    public static class Config_DrawSurface
    {

        // Canvas.
        public static int Width { get; set; }
        public static int Height { get; set; }
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
            FontSize = 12;
            GridUnitWidth = FontSize;
            GridUnitHeight = GridUnitWidth;
            SpacingBetweenStaff = 2;
            Width = GridUnitWidth * 81;
            Height = GridUnitHeight * 200;
            CharWidth = 12;
            CharHeight = 12;
            LineThickness = 12;
            MarginX = 10;
            MarginY = 10;

            BGColor = Colors.LightGray;
            TextColor = Brushes.White;
            TextFont = new Typeface("Verdana");

        }
    }
}






