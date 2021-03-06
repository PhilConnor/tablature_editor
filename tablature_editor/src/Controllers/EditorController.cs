﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFE.Models;
using PFE.Interfaces;
using PFE.Utils;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using PFE.Configs;
using PFE.UndoRedo;
using PFE.Algorithms;

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
        private ScrollViewer _scrollViewer;

        /// <summary>
        /// The Editor instance.
        /// </summary>
        private Editor _editor;

        private MementoCareTaker mementoCareTaker;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="drawSurface">the surface on wich to draw the editor</param>
        /// <param name="editor">an editor instance</param>
        public EditorController(Editor editor, DrawSurface drawSurface, ScrollViewer scrollViewer)
        {
            _editor = editor;
            _drawSurface = drawSurface;
            _scrollViewer = scrollViewer;

            mementoCareTaker = new MementoCareTaker(_editor.GetMemento());
            editor.Subscribe(this);
            ReDrawTablature();
        }

        #region UndoRedo

        public void Undo()
        {
            Memento memento = mementoCareTaker.Undo();
            _editor.UpdateToMemento(memento);
        }

        public void Redo()
        {
            Memento memento = mementoCareTaker.Redo();
            _editor.UpdateToMemento(memento);
        }

        public void UpdateMementoCareTaker()
        {
            mementoCareTaker.AddMemento(_editor.GetMemento());
        }

        #endregion

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
            foreach (TabCoord tabCoord in _editor.GetSelectedTabCoords())
            {
                DrawSurfaceCoord canvasCoord = CoordConverter.ToDrawSurfaceCoord(tabCoord, _editor);
                _drawSurface.DrawRectangle(canvasCoord);
            }
        }

        /// <summary>
        /// Redraws all tablature elements ( all chars )
        /// </summary>
        private void RedrawElements()
        {
            //for each tab elements
            for (int x = 0; x < _editor.TabLength; ++x)
            {
                for (int y = 0; y < _editor.NStrings; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);
                    DrawSurfaceCoord canvasCoord = CoordConverter.ToDrawSurfaceCoord(tabCoord, _editor);
                    _drawSurface.DrawCharAtTabCoord(canvasCoord, _editor.GetElementCharAt(tabCoord));
                }
            }
        }

        #region User inputs
        public void KeyDown(KeyEventArgs e)
        {
            //shift + arrow
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Left)
            {
                _editor.MoveCursor(CursorMovements.ExpandLeft);
                UpdateMementoCareTaker();
            }
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Up)
            {
                _editor.MoveCursor(CursorMovements.ExpandUp);
                UpdateMementoCareTaker();
            }
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Right)
            {
                _editor.MoveCursor(CursorMovements.ExpandRight);
                UpdateMementoCareTaker();
            }
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Down)
            {
                _editor.MoveCursor(CursorMovements.ExpandDown);
                UpdateMementoCareTaker();
            }

            //arrow
            else if (e.Key == Key.Left)
            {
                _editor.MoveCursor(CursorMovements.Left);
                UpdateMementoCareTaker();
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                _editor.MoveCursor(CursorMovements.Up);
                UpdateMementoCareTaker();
                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                _editor.MoveCursor(CursorMovements.Right);
                UpdateMementoCareTaker();
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                _editor.MoveCursor(CursorMovements.Down);
                UpdateMementoCareTaker();
                e.Handled = true;
            }

            //backspace, delete
            else if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                _editor.ClearCharsAtCursor();
                UpdateMementoCareTaker();
            }

            //space removal
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Space)
            {
                _editor.RemoveSpaceAtCursor();
                UpdateMementoCareTaker();
            }

            //space insertion
            else if (e.Key == Key.Space)
            {
                _editor.InsertSpaceAtCursor();
                UpdateMementoCareTaker();
            }
            
            //enter
            else if (!Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Enter)
            {
                _editor.MoveCursor(CursorMovements.SkipStaffDown);
                UpdateMementoCareTaker();
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Enter)
            {
                _editor.MoveCursor(CursorMovements.SkipStaffUp);
                UpdateMementoCareTaker();
            }

            //copy
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.C)
                System.Windows.Clipboard.SetText(_editor.SelectionToAscii());

            //paste
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.V)
            {
                _editor.ParseAscii(System.Windows.Clipboard.GetText());
                UpdateMementoCareTaker();
            }

            //undo
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Z)
            {
                Debug.WriteLine("Undo " + DateTime.Now.ToString());
                Undo();
            }

            //redo
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.R)
                Redo();

            //increment
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Add)
            {
                _editor.TransposeSelection(1);
                UpdateMementoCareTaker();
            }

            //decrement
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Subtract)
            {
                _editor.TransposeSelection(-1);
                UpdateMementoCareTaker();
            }

            //capslock to toggle write mode
            else if (e.Key == Key.CapsLock)
                _editor.ToggleWriteMode();

        }

        public void TextInput(TextCompositionEventArgs e)
        {
            if (e.Text.Length < 1)
                return;

            char charInput = e.Text.ToCharArray()[0];

            //text
            if (Char.IsLetterOrDigit(charInput))
            {
                _editor.WriteCharAtCursor(e.Text.ToCharArray()[0]);
                UpdateMementoCareTaker();
            }
        }

        public void MouseDrag(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point point = e.GetPosition(sender as DrawSurface);

            TabCoord tabCoord = CoordConverter.ToTabCoord(point, _editor);

            if (tabCoord == null)
                return;

            _editor.SelectUpTo(tabCoord);
        }

        public void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(sender as DrawSurface);

            TabCoord tabCoord = CoordConverter.ToTabCoord(point, _editor);

            if (tabCoord == null)
                return;

            _editor.Select(tabCoord);
        }

        public void MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UpdateMementoCareTaker();
            Debug.WriteLine("Mouse up " + DateTime.Now.ToString());
        }
        #endregion

        public void NotifyRedraw()
        {
            ReDrawTablature();
        }

        /// <summary>
        /// Increase the height of the DrawSurface when we add a staff.
        /// </summary>
        public void NotifyNewStaffAdded()
        {
            _drawSurface.Height = Math.Max(710, 710 + ((_editor.NStaff - 7) * 98));
            _scrollViewer.ScrollToEnd();
        }

        /// <summary>
        /// Scrolls up or down to follow the cursor's movement.
        /// </summary>
        public void NotifyScrollToCursor()
        {
            double ScreenTop = _scrollViewer.VerticalOffset;
            int cursorPos = CoordConverter.ToDrawSurfaceCoord(_editor.CursorCoord, _editor).y;
            double ScreenBottom = _scrollViewer.VerticalOffset + _scrollViewer.ViewportHeight;

            if (cursorPos < ScreenTop)
            {
                _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset - 98);
            }
            else if (cursorPos > ScreenBottom)
            {
                _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + 98);
            }
        }
    }
}
