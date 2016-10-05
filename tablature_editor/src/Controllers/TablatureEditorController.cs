using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablatureEditor.Models;
using TablatureEditor.Interfaces;
using TablatureEditor.Utils;
using System.Windows.Input;

namespace TablatureEditor.Controllers
{
    /// <summary>
    /// Manage redraw of graphics elements on notification and user inputs.
    /// </summary>
    class TablatureEditorController : IObserver
    {
        private DrawSurface _drawSurface;
        private Models.TablatureEditor _tablatureEditor;

        public TablatureEditorController(DrawSurface drawSurface, Models.TablatureEditor tablatureEditor)
        {
            _drawSurface = drawSurface;
            _tablatureEditor = tablatureEditor;

            tablatureEditor.Subscribe(this);
            ReDrawTablature();
        }

        public void ReDrawTablature()
        {
            _drawSurface.StartDrawing();

            _drawSurface.DrawBackground();

            RedrawCursor();

            RedrawElements();

            _drawSurface.EndDrawing();
        }

        private void RedrawCursor()
        {
            foreach (TabCoord tabCoord in _tablatureEditor._cursor.Logic.GetSelectionTabCoords())
            {
                _drawSurface.DrawRectangle(tabCoord);
            }
        }

        private void RedrawElements()
        {
            for (int x = 0; x < _tablatureEditor._tablature.positions.Count; ++x)
            {
                for (int y = 0; y < _tablatureEditor._tablature.positions.ElementAt(0).elements.Count; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);

                    if (!IsAnElementAtAlreadyThere(tabCoord))
                    {
                        _drawSurface.DrawTextAtTabCoord(tabCoord, _tablatureEditor._tablature.getTextAt(tabCoord));
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
            string txt = _tablatureEditor._tablature.getTextAt(tc);
            return Util.isNumberOver9(txt);
        }

        #region User inputs
        public void window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //shift + arrow
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Left)
                _tablatureEditor.MoveCursor(CursorMovements.ExpandLeft);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Up)
                _tablatureEditor.MoveCursor(CursorMovements.ExpandUp);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Right)
                _tablatureEditor.MoveCursor(CursorMovements.ExpandRight);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Down)
                _tablatureEditor.MoveCursor(CursorMovements.ExpandDown);

            //arrow
            else if (e.Key == Key.Left)
                _tablatureEditor.MoveCursor(CursorMovements.Left);
            else if (e.Key == Key.Up)
                _tablatureEditor.MoveCursor(CursorMovements.Up);
            else if (e.Key == Key.Right)
                _tablatureEditor.MoveCursor(CursorMovements.Right);
            else if (e.Key == Key.Down)
                _tablatureEditor.MoveCursor(CursorMovements.Down);

            //backspace, delete
            else if (e.Key == Key.Back || e.Key == Key.Delete)
                _tablatureEditor.WriteCharAtCursor("-");

            else if (e.Key == Key.CapsLock)
                _tablatureEditor.ToggleWriteMode();
        }

        public void window_TextInput(object sender, TextCompositionEventArgs e)
        {
            //text
            _tablatureEditor.WriteCharAtCursor(e.Text);
        }

        #endregion
        public void Notify()
        {
            ReDrawTablature();
        }
    }
}
