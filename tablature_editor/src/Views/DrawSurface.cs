using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using PFE.Models;
using PFE.Configs;
using tablature_editor.Utils;

namespace PFE
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

        public void DrawRectangle(CanvasCoord canvasCoord)
        {
            if (!isDrawing)
                throw new Exception();

            Point point = new Point(canvasCoord.x, canvasCoord.y);

            drawingContext.DrawRectangle(
                Brushes.LightBlue,
                null,
                new Rect(point, new Size(Config_DrawSurface.Inst().GridUnitWidth, Config_DrawSurface.Inst().GridUnitWidth)));
        }

        public void DrawTextAtTabCoord(CanvasCoord canvasCoord, char text)
        {
            if (!isDrawing)
                throw new Exception("Something is wrong with the input text");

            Point point = new Point(canvasCoord.x, canvasCoord.y);
            DrawCharAtPoint(point, text);
        }

        private void DrawCharAtPoint(Point point, char c)
        {
            FormattedText formattedText =
                new FormattedText(
                    c.ToString(),
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
