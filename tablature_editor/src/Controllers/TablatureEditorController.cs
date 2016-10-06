using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFE.Models;
using PFE.Interfaces;
using PFE.Utils;
using System.Windows.Input;
using System.Windows;
using tablature_editor.Utils;

namespace PFE.Controllers
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
            foreach (TabCoord tabCoord in _tablatureEditor.Cursor.Logic.GetSelectionTabCoords())
            {
                _drawSurface.DrawRectangle(tabCoord);
            }
        }

        private void RedrawElements()
        {
            for (int x = 0; x < _tablatureEditor.Tablature.positions.Count; ++x)
            {
                for (int y = 0; y < _tablatureEditor.Tablature.positions.ElementAt(0).elements.Count; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);

                    if (!IsAnElementAtAlreadyThere(tabCoord))
                    {
                        _drawSurface.DrawTextAtTabCoord(tabCoord, _tablatureEditor.Tablature.getTextAt(tabCoord));
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
            string txt = _tablatureEditor.Tablature.getTextAt(tc);
            return Util.isNumberOver9(txt);
        }

        #region User inputs
        public void KeyDown(KeyEventArgs e)
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

        public void TextInput(TextCompositionEventArgs e)
        {
            //text
            _tablatureEditor.WriteCharAtCursor(e.Text);
        }

        public void MouseDrag(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point p = e.GetPosition(sender as DrawSurface);
            CanvasCoord c = CanvasCoord.PointToCanvasCoord(p);
            TabCoord t = CoordConverter.ToTabCoord(c);
            if (t == null)
                return;

            _tablatureEditor.Cursor._c2 = t;
            Debug.WriteLine("drag " + c.ToString() + " " + t.ToString());
        }

        public void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(sender as DrawSurface);
            CanvasCoord c = CanvasCoord.PointToCanvasCoord(p);
            TabCoord t = CoordConverter.ToTabCoord(c);

            if (t == null)
                return;

            _tablatureEditor.Cursor.setPositions(t);
            Debug.WriteLine("down " + c.ToString() + " " + t.ToString());
        }

        #endregion
        public void Notify()
        {
            ReDrawTablature();
        }
    }
}
