using System;
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
    class TablatureEditorController : IObserver
    {
        private DrawSurface drawSurface;
        private Models.TablatureEditor tablatureEditor;

        public TablatureEditorController(DrawSurface drawSurface, Models.TablatureEditor tablatureEditor)
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
            foreach (TabCoord tabCoord in tablatureEditor._cursor.Logic.GetSelectionTabCoords())
            {
                drawSurface.DrawRectangle(tabCoord);
            }
        }

        private void RedrawElements()
        {
            for (int x = 0; x < tablatureEditor._tablature.positions.Count; ++x)
            {
                for (int y = 0; y < tablatureEditor._tablature.positions.ElementAt(0).elements.Count; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);

                    if (!IsAnElementAtAlreadyThere(tabCoord))
                    {
                        drawSurface.DrawTextAtTabCoord(tabCoord, tablatureEditor._tablature.getTextAt(tabCoord));
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
            string txt = tablatureEditor._tablature.getTextAt(tc);
            return Util.isNumberOver9(txt);
        }
        
        public void Notify()
        {
            ReDrawTablature();
        }
    }
}
