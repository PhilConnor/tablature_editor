using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tablature_editor;
using tablature_editor.src.Interfaces;

namespace tablature_editor.src.Controler
{
    /// <summary>
    /// Manage redraw of graphics elements on notification.
    /// </summary>
    class TabController : IObserver
    {
        private DrawSurface _canvasCustom;
        private TablatureEditor _tablatureEditor;        
        
        public TabController(DrawSurface canvasCustom, TablatureEditor tablatureEditor)
        {
            _canvasCustom = canvasCustom;
            _tablatureEditor = tablatureEditor;

            tablatureEditor.Subscribe(this);
            ReDrawTablature();
        }

        public void ReDrawTablature()
        {
            _canvasCustom.StartDrawing();

            _canvasCustom.DrawBackground();

            foreach(Coord tabCoord in _tablatureEditor._cursor.getTouchingTabCoords())
            {
                _canvasCustom.DrawRectangle(tabCoord);
            }

            for (int x = 0; x < _tablatureEditor._tablature._positions.Count; x++)
            {
                for (int y = 0; y < _tablatureEditor._tablature._positions.ElementAt(0)._elements.Count; y++)
                {
                    Coord tabCoord = new Coord(x, y);
                    _canvasCustom.DrawText(tabCoord, _tablatureEditor._tablature.getTextAt(tabCoord));
                }
            }

            _canvasCustom.EndDrawing();
        }

        public void Notify()
        {
            ReDrawTablature();
        }
    }
}
