﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablatureEditor.Models;
using TablatureEditor.Interfaces;
using TablatureEditor.Utils;

namespace TablatureEditor.Controllers
{
    /// <summary>
    /// Manage redraw of graphics elements on notification.
    /// </summary>
    class TabController : IObserver
    {
        private DrawSurface drawSurface;
        private TabEditor tablatureEditor;

        public TabController(DrawSurface drawSurface, TabEditor tablatureEditor)
        {
            this.drawSurface = drawSurface;
            this.tablatureEditor = tablatureEditor;

            tablatureEditor.Subscribe(this);
            ReDrawTablature();
        }

        public void ReDrawTablature()
        {
            drawSurface.StartDrawing();

            drawSurface.DrawBackground();

            RedrawCursor();

            RedrawElements();

            drawSurface.EndDrawing();
        }

        private void RedrawCursor()
        {
            foreach (TabCoord tabCoord in tablatureEditor.cursorController.GetSelectionTabCoords())
            {
                drawSurface.DrawRectangle(tabCoord);
            }
        }

        private void RedrawElements()
        {
            for (int x = 0; x < tablatureEditor.tablature.positions.Count; ++x)
            {
                for (int y = 0; y < tablatureEditor.tablature.positions.ElementAt(0).elements.Count; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);

                    if (!IsAnElementAtAlreadyThere(tabCoord))
                    {
                        drawSurface.DrawTextAtTabCoord(tabCoord, tablatureEditor.tablature.getTextAt(tabCoord));
                    }
                }
            }
        }

        /// Indicates if the element before the one pointed by 
        /// tabCoord is cointaining two char
        private bool IsAnElementAtAlreadyThere(TabCoord tabCoord)
        {
            if (tabCoord.x == 0)
                return false;

            TabCoord tc = new TabCoord(tabCoord.x - 1, tabCoord.y);
            string txt = tablatureEditor.tablature.getTextAt(tc);
            return Util.isNumberOver9(txt);
        }
        
        public void Notify()
        {
            ReDrawTablature();
        }
    }
}
