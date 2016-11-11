using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using PFE.Models;
using PFE.Configs;
using PFE.Utils;

namespace PFE
{
    /// <summary>
    /// The surface on wich the tablature is drawn.
    /// Provites methods to draw the background, rectangles and text.
    /// </summary>
    class DrawsurfaceV2
    {

        public DrawsurfaceV2()
        {
        }

        public DrawingImage TestDrawImage()

        {

            GeometryGroup myellipses = new GeometryGroup();
            myellipses.Children.Add(
                new EllipseGeometry(new Point(50, 50), 30, 30)
                );

            myellipses.Children.Add(
                new EllipseGeometry(new Point(50, 50), 10, 10)
                );

            GeometryDrawing MyGeometryDrawing = new GeometryDrawing();

            MyGeometryDrawing.Geometry = myellipses;

            MyGeometryDrawing.Brush =
                new LinearGradientBrush(
                    Colors.PaleVioletRed,
                    Color.FromRgb(204, 204, 255),
                    new Point(0, 0),
                    new Point(1, 1));

            MyGeometryDrawing.Pen = new Pen(Brushes.Indigo, 10);




            DrawingImage MyFirstGeometryImage = new DrawingImage(MyGeometryDrawing);
            MyFirstGeometryImage.Freeze();
            return MyFirstGeometryImage;
        }


        public void DrawBackground()
        {




            //drawingContext.DrawRectangle(
            //    Brushes.Black,
            //    null,
            //    new Rect(
            //        new Point(0, 0), 
            //        new Size(this.Width, 
            //        this.Height)));
        }

        /// <summary>
        /// Draws a 1x1 GridUnit dimension rectangle at canvas coord.
        /// Used mainly to draw the cursor.
        /// </summary>
        public void DrawRectangle(DrawSurfaceCoord canvasCoord)
        {
            //if (!isDrawing)
            //    throw new Exception();

            //Point point = new Point(canvasCoord.x, canvasCoord.y);

            //drawingContext.DrawRectangle(
            //    Brushes.LightBlue,
            //    null,
            //    new Rect(
            //        point, 
            //        new Size(Config_DrawSurface.Inst().GridUnitWidth, 
            //        Config_DrawSurface.Inst().GridUnitWidth)));
        }

        public void DrawCharAtTabCoord(DrawSurfaceCoord canvasCoord, char chr)
        {
            //if (!isDrawing)
            //    throw new Exception("Something is wrong with the input text");

            //Point point = new Point(canvasCoord.x, canvasCoord.y);
            //DrawCharAtPoint(point, chr);
        }

        private void DrawCharAtPoint(Point point, char chr)
        {
            //    FormattedText formattedText =
            //        new FormattedText(
            //            chr.ToString(),
            //            CultureInfo.GetCultureInfo("en-us"),
            //            FlowDirection.LeftToRight,
            //            Config_DrawSurface.Inst().TextFont,
            //            Config_DrawSurface.Inst().FontSize,
            //            Brushes.White);

            //    drawingContext.DrawText(
            //        formattedText,
            //        point);
        }
    }
}
