using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace tablature_editor
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

        public void DrawRectangle(Coord tabCoord)
        {
            if (!isDrawing)
                throw new Exception();

            Coord canvasCoord = Coord.toCanvasCoord(tabCoord);
            Point point = new Point(canvasCoord.x, canvasCoord.y);

            drawingContext.DrawRectangle(
                Brushes.LightBlue,
                null,
                new Rect(point, new Size(Configuration.Inst.unitSizeX, Configuration.Inst.unitSizeX)));
        }

        public void DrawText(Coord tabCoord, string text)
        {
            if (!isDrawing)
                throw new Exception();

            Coord canvasCoord = Coord.toCanvasCoord(tabCoord);
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
