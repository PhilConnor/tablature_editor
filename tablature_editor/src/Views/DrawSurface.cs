using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using TablatureEditor.Models;
using TablatureEditor.Configs;

namespace TablatureEditor
{
    class DrawSurface : FrameworkElement
    {
        protected VisualCollection visuals;

        protected DrawingContext drawingContext;
        protected DrawingVisual drawingVisual;

        private bool isDrawing = false;

        public DrawSurface()
        {
            visuals = new VisualCollection(this);
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
                new Rect(new Point(0, 0), new Size(this.Width, this.Height)));
        }

        public void DrawRectangle(TabCoord tabCoord)
        {
            if (!isDrawing)
                throw new Exception();

            CanvasCoord canvasCoord = tabCoord.ToCanvasCoord();
            Point point = new Point(canvasCoord.x, canvasCoord.y);

            drawingContext.DrawRectangle(
                Brushes.LightBlue,
                null,
                new Rect(point, new Size(Configuration.UnitSizeX, Configuration.UnitSizeX)));
        }

        public void DrawText(TabCoord tabCoord, string text)
        {
            if (!isDrawing)
                throw new Exception();

            Coord canvasCoord = tabCoord.ToCanvasCoord();
            Point point = new Point(canvasCoord.x, canvasCoord.y);

            drawingContext.DrawText(
                new FormattedText(text,
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface("Verdana"),
                    12,
                    Brushes.White),
                point);
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }
    }
}
