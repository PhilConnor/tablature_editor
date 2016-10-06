using PFE.Interfaces;
using System.Windows.Media;
using System;
using System.Collections.Generic;

namespace PFE.Configs
{
    public class Config_DrawSurface : IObservable
    {
        // Canvas.
        public int Width { get; set; }
        public int Height { get; set; }
        public int Window_Width { get; set; }
        public int Window_Height { get; set; }
        public int CharWidth { get; set; }
        public int CharHeight { get; set; }
        public int LineThickness { get; set; }
        public int FontSize { get; set; }
        public int SpacingBetweenStaff { get; set; }

        // Grid resolution.
        public int GridUnitWidth { get; set; }
        public int GridUnitHeight { get; set; }

        // Tab pages margins.
        public int MarginX { get; set; }
        public int MarginY { get; set; }

        // Colors
        public Color BGColor { get; set; }
        public Brush TextColor { get; set; }

        public Typeface TextFont { get; set; }

        private static Config_DrawSurface config;

        public static Config_DrawSurface Inst()
        {
            if (config == null)
            {
                config = new Config_DrawSurface();
                config.Initialisation();
                return config;
            }
            else
            {
                return config;
            }
        }

        //Constructors.
        public void Initialisation()
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

        private List<IObserver> observers = new List<IObserver>();
        public void NotifyObserver()
        {
            observers.ForEach(o => o.Notify());
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }
    }
}






