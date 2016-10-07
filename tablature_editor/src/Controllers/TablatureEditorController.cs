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
using PFE.Configs;

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
            foreach (TabCoord tabCoord in _tablatureEditor.GetSelectedTabCoords())
            {
                CanvasCoord canvasCoord = CoordConverter.ToCanvasCoord(tabCoord, _tablatureEditor);
                _drawSurface.DrawRectangle(canvasCoord);
            }
        }

        private void RedrawElements()
        {
            for (int x = 0; x < _tablatureEditor.TabLength; ++x)
            {
                for (int y = 0; y < _tablatureEditor.NStrings; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);

                    if (!IsAnElementAtAlreadyThere(tabCoord))
                    {
                        for (int i = 0; i < _tablatureEditor.GetTextAt(tabCoord).Length; i++)
                        {
                            TabCoord adjustedTabCoord = new TabCoord(tabCoord.x + i, tabCoord.y);
                            CanvasCoord canvasCoord = CoordConverter.ToCanvasCoord(adjustedTabCoord, _tablatureEditor);
                            _drawSurface.DrawTextAtTabCoord(canvasCoord, _tablatureEditor.GetTextAt(tabCoord)[i]);
                        }
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
            string txt = _tablatureEditor.GetTextAt(tc);
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
            Point point = e.GetPosition(sender as DrawSurface);

            TabCoord tabCoord = CoordConverter.ToTabCoord(point, _tablatureEditor);

            if (tabCoord == null)
                return;

            _tablatureEditor.SelectUpTo(tabCoord);
        }

        public void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(sender as DrawSurface);

            TabCoord tabCoord = CoordConverter.ToTabCoord(point, _tablatureEditor);

            if (tabCoord == null)
                return;

            _tablatureEditor.Select(tabCoord);
        }

        #endregion
        public void Notify()
        {
            ReDrawTablature();
        }
    }
}
