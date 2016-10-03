using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablatureEditor.Models;
using TablatureEditor.Interfaces;

namespace TablatureEditor.Controllers
{
    /// <summary>
    /// Manage redraw of graphics elements on notification.
    /// </summary>
    class TabController : IObserver
    {
        private DrawSurface canvasCustom;
        private TabEditor tablatureEditor;        
        
        public TabController(DrawSurface canvasCustom, TabEditor tablatureEditor)
        {
            this.canvasCustom = canvasCustom;
            this.tablatureEditor = tablatureEditor;

            tablatureEditor.Subscribe(this);
            ReDrawTablature();
        }

        public void ReDrawTablature()
        {
            canvasCustom.StartDrawing();

            canvasCustom.DrawBackground();

            foreach(TabCoord tabCoord in tablatureEditor.cursorController.GetTouchingTabCoords())
            {
                canvasCustom.DrawRectangle(tabCoord);
            }

            for (int x = 0; x < tablatureEditor.tablature.positions.Count; ++x)
            {
                for (int y = 0; y < tablatureEditor.tablature.positions.ElementAt(0).elements.Count; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);
                    canvasCustom.DrawText(tabCoord, tablatureEditor.tablature.getTextAt(tabCoord));
                }
            }

            canvasCustom.EndDrawing();
        }

        public void Notify()
        {
            ReDrawTablature();
        }
    }
}
