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
    class EditorController : IObserver
    {
        /// <summary>
        /// The drawSurface instance.
        /// </summary>
        private DrawSurface _drawSurface;

        /// <summary>
        /// The Editor instance.
        /// </summary>
        private Editor _tablatureEditor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="drawSurface">the surface on wich to draw the editor</param>
        /// <param name="tablatureEditor">an editor instance</param>
        public EditorController(Editor tablatureEditor, DrawSurface drawSurface)
        {
            _tablatureEditor = tablatureEditor;
            _drawSurface = drawSurface;

            tablatureEditor.Subscribe(this);
            ReDrawTablature();
        }
        
        /// <summary>
        /// Redraws the Editor objects on the DrawSurface.
        /// </summary>
        public void ReDrawTablature()
        {
            _drawSurface.StartDrawing();

            _drawSurface.DrawBackground();

            RedrawCursor();

            RedrawElements();

            _drawSurface.EndDrawing();
        }

        /// <summary>
        /// Redraws the cursor.
        /// </summary>
        private void RedrawCursor()
        {
            foreach (TabCoord tabCoord in _tablatureEditor.GetSelectedTabCoords())
            {
                DrawSurfaceCoord canvasCoord = CoordConverter.ToDrawSurfaceCoord(tabCoord, _tablatureEditor);
                _drawSurface.DrawRectangle(canvasCoord);
            }
        }

        /// <summary>
        /// Redraws all tablature elements ( all chars )
        /// </summary>
        private void RedrawElements()
        {
            //for each tab elements
            for (int x = 0; x < _tablatureEditor.TabLength; ++x)
            {
                for (int y = 0; y < _tablatureEditor.NStrings; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);
                    DrawSurfaceCoord canvasCoord = CoordConverter.ToDrawSurfaceCoord(tabCoord, _tablatureEditor);
                    _drawSurface.DrawCharAtTabCoord(canvasCoord, _tablatureEditor.GetElementCharAt(tabCoord));                    
                }
            }
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
                _tablatureEditor.ClearCharsAtCursor();

            //enter
            else if (!Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Enter)
                _tablatureEditor.MoveCursor(CursorMovements.SkipStaffDown);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Enter)
                _tablatureEditor.MoveCursor(CursorMovements.SkipStaffUp);

            //copy
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.C)
                System.Windows.Clipboard.SetText(_tablatureEditor.SelectionToAscii());

            //paste
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.V)
                _tablatureEditor.ParseAscii(System.Windows.Clipboard.GetText());

            //capslock to toggle write mode
            else if (e.Key == Key.CapsLock)
                _tablatureEditor.ToggleWriteMode();

        }

        public void TextInput(TextCompositionEventArgs e)
        {
            if (e.Text.Length < 1)
                return;

            char charInput = e.Text.ToCharArray()[0];
           
            //text
            if (Char.IsLetterOrDigit(charInput))        
                _tablatureEditor.WriteCharAtCursor(e.Text.ToCharArray()[0]);
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
