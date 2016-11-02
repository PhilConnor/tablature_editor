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
    class DrawSurface : FrameworkElement
    {
        protected VisualCollection visuals;
        protected DrawingContext drawingContext;
        protected DrawingVisual drawingVisual;

        private bool isDrawing = false;

        public DrawSurface()
        {
            visuals = new VisualCollection(this);
            this.Height = Config_DrawSurface.Inst().Height;
            this.Width = Config_DrawSurface.Inst().Width;
        }

        public void StartDrawing()
        {
            isDrawing = true;

            visuals.Clear();
            drawingVisual = new DrawingVisual();
            drawingContext = drawingVisual.RenderOpen();
        }

        public void EndDrawing()
        {
            isDrawing = false;

            drawingContext.Close();
            visuals.Add(drawingVisual);
        }

        public void DrawBackground()
        {
            drawingContext.DrawRectangle(
                Brushes.Black,
                null,
                new Rect(
                    new Point(0, 0), 
                    new Size(this.Width, 
                    this.Height)));
        }

        /// <summary>
        /// Draws a 1x1 GridUnit dimension rectangle at canvas coord.
        /// Used mainly to draw the cursor.
        /// </summary>
        public void DrawRectangle(DrawSurfaceCoord canvasCoord)
        {
            if (!isDrawing)
                throw new Exception();

            Point point = new Point(canvasCoord.x, canvasCoord.y);

            drawingContext.DrawRectangle(
                Brushes.LightBlue,
                null,
                new Rect(
                    point, 
                    new Size(Config_DrawSurface.Inst().GridUnitWidth, 
                    Config_DrawSurface.Inst().GridUnitWidth)));
        }

        public void DrawCharAtTabCoord(DrawSurfaceCoord canvasCoord, char chr)
        {
            if (!isDrawing)
                throw new Exception("Something is wrong with the input text");

            Point point = new Point(canvasCoord.x, canvasCoord.y);
            DrawCharAtPoint(point, chr);
        }

        private void DrawCharAtPoint(Point point, char chr)
        {
            FormattedText formattedText =
                new FormattedText(
                    chr.ToString(),
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    Config_DrawSurface.Inst().TextFont,
                    Config_DrawSurface.Inst().FontSize,
                    Brushes.White);

            drawingContext.DrawText(
                formattedText,
                point);
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }
    }
}
