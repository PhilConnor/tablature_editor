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

        public void DrawRectangle(TabCoord tabCoord)
        {
            if (!isDrawing)
                throw new Exception();

            CanvasCoord canvasCoord = CoordConverter.ToCanvasCoord(tabCoord);
            Point point = new Point(canvasCoord.x, canvasCoord.y);

            drawingContext.DrawRectangle(
                Brushes.LightBlue,
                null,
                new Rect(point, new Size(Config_DrawSurface.GridUnitWidth, Config_DrawSurface.GridUnitWidth)));
        }

        public void DrawTextAtTabCoord(TabCoord tabCoord, string text)
        {
            if (!isDrawing)
                throw new Exception("Something is wrong with the input text");

            for (int i = 0; i < text.Length; i++)
            {
                TabCoord adjustedTabCoord = new TabCoord(tabCoord.x + i, tabCoord.y);
                CanvasCoord canvasCoord = CoordConverter.ToCanvasCoord(adjustedTabCoord);
                Point point = new Point(canvasCoord.x, canvasCoord.y);
                DrawCharAtPoint(point, text[i]);
            }
        }

        private void DrawCharAtPoint(Point point, char c)
        {
            FormattedText formattedText =
                new FormattedText(
                    c.ToString(),
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    Config_DrawSurface.TextFont,
                    Config_DrawSurface.FontSize,
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
