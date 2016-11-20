using System;
using System.Collections.Generic;
using PFE.Controllers;
using PFE.Interfaces;
using PFE.Utils;
using PFE.UndoRedo;
using System.Windows.Media;
using PFE.Algorithms;
using NAudioDemo.SignalToNote;
using Pitch;

namespace PFE.Models
{
    /// <summary>
    /// Acts like a facade, provides unified and simplified access 
    /// points to tablature and cursor models.
    /// </summary>
    public class Editor : IObservable
    {
        #region properties
        private Tablature _tablature;
        private Cursor _cursor;
        private WriteModes WriteMode;
        private SkipModes SkipMode;
        private SignalParser sp;
        /// <summary>
        /// Number of strings.
        /// </summary>
        public int NStrings
        {
            get { return Tablature.NStrings; }
        }

        /// <summary>
        /// Number of staffs.
        /// </summary>
        public int NStaff
        {
            get { return Tablature.NStaff; }
        }

        /// <summary>
        /// Lenght of a staff.
        /// </summary>
        public int StaffLength
        {
            get { return Tablature.StaffLength; }
        }

        /// <summary>
        /// Total lenght of the tablature.
        /// </summary>
        public int TabLength
        {
            get { return Tablature.Length; }
        }

        public TabCoord CursorCoord
        {
            get { return _cursor.DragableCoord; }
        }

        public Tablature Tablature
        {
            get
            {
                return _tablature;
            }
        }

        public Cursor Cursor
        {
            get
            {
                return _cursor;
            }
        }
        #endregion

        #region Public
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tablature">The instance of the tablature.</param>
        /// <param name="cursor">The instance of the cursor.</param>
        public Editor(Tablature tablature, Cursor cursor)
        {
            WriteMode = WriteModes.Unity;
            SkipMode = SkipModes.One;

            _tablature = tablature;
            _cursor = cursor;

            sp = new SignalParser(OnPitchDetectedHandler);


            NotifyObserverRedraw();
        }

        /// <summary>
        /// Transpose the selection by nSemiTones.
        /// </summary>
        /// <param name="nSemiTones"></param>
        public void TransposeSelection(int nSemiTones)
        {
            Algorithms.Transposition.TransposeSelection(this, nSemiTones);
            NotifyObserverRedraw();
        }

        /// <summary>
        /// Instruct the editor to place the char where the cursor is.
        /// If the cursor is bigger than 1x1, it wills the whole area with the cursor.
        /// </summary>
        /// <param name="chr"></param>
        public void WriteCharAtCursor(char chr)
        {
            TabCoord cursorTopLeftCoord = _cursor.TopLeftTabCoord();

            // Fills the cursor selection with appropriate chr.
            for (int x = cursorTopLeftCoord.x; x <= cursorTopLeftCoord.x + _cursor.Width - 1; ++x)
            {
                for (int y = cursorTopLeftCoord.y; y <= cursorTopLeftCoord.y + _cursor.Height - 1; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);
                    TabCoord tabCoordOnRight = tabCoord.CoordOnRight();
                    Element elementOnright = Tablature.ElementAt(tabCoordOnRight);

                    // if we are in 10th or 20th mode we write a 1 or 2 before the char.
                    if (Util.IsNumber(chr) && IsWriteModeActivated() && elementOnright != null)
                    {
                        Tablature.AttemptSetModifierCharAt(tabCoord, GetWriteModeCharacter().Value);
                        Tablature.AttemptSetModifierCharAt(tabCoordOnRight, chr);
                    }
                    else
                    {
                        Tablature.AttemptSetModifierCharAt(tabCoord, chr);
                    }
                }
            }

            //move the cursor to the next position.
            ApplyCursorMovementBaseOnInput(chr);

            NotifyObserverRedraw();
        }

        /// <summary>
        /// Instruct the editor to clear all chars on cursor
        /// </summary>
        public void ClearCharsAtCursor()
        {
            TabCoord cursorTopLeftCoord = _cursor.TopLeftTabCoord();

            // Fills the cursor selection with appropriate chr.
            for (int x = cursorTopLeftCoord.x; x <= cursorTopLeftCoord.x + _cursor.Width - 1; ++x)
            {
                for (int y = cursorTopLeftCoord.y; y <= cursorTopLeftCoord.y + _cursor.Height - 1; ++y)
                {
                    TabCoord tabCoord = new TabCoord(x, y);
                    Element element = Tablature.ElementAt(tabCoord);
                    element.ClearText();
                }
            }

            NotifyObserverRedraw();
        }

        /// <summary>
        /// Instruct the editor to insert space at cursor.
        /// </summary>
        public void InsertSpaceAtCursor()
        {
            TabCoord topLeft = _cursor.TopLeftTabCoord();
            Tablature.InsertSpaceAt(topLeft);

            NotifyObserverRedraw();
        }

        public void RemoveSpaceAtCursor()
        {
            Tablature.RemoveSpaceAt(_cursor.TopLeftTabCoord().x);

            NotifyObserverRedraw();
        }

        /// <summary>
        /// True if write mode is activated.
        /// </summary>
        /// <returns></returns>
        private bool IsWriteModeActivated()
        {
            return WriteMode == WriteModes.Tenth || WriteMode == WriteModes.Twenyth;
        }

        /// <summary>
        /// Returns the character to add depending of the write mode.
        /// </summary>
        /// <returns></returns>
        private char? GetWriteModeCharacter()
        {
            if (WriteMode == WriteModes.Tenth)
                return '1';
            else if (WriteMode == WriteModes.Twenyth)
                return '2';

            return null;
        }

        /// <summary>
        /// Applies the movementType to the cursor without notifiying observers of the editor.
        /// </summary>
        /// <param name="mouvementType"></param>
        private void MoveCursorWithoutNotifyingObservers(CursorMovements mouvementType)
        {
            switch (mouvementType)
            {
                case CursorMovements.Left:
                    CursorMoveLeft();
                    break;

                case CursorMovements.Up:
                    CursorMoveUp();
                    break;

                case CursorMovements.Right:
                    CursorMoveRight();
                    break;

                case CursorMovements.Down:
                    CursorMoveDown();
                    break;

                case CursorMovements.ExpandLeft:
                    CursorExpandLeft();
                    break;

                case CursorMovements.ExpandUp:
                    CursorExpandUp();
                    break;

                case CursorMovements.ExpandRight:
                    CursorExpandRight();
                    break;

                case CursorMovements.ExpandDown:
                    CursorExpandDown();
                    break;

                case CursorMovements.SkipStaffDown:
                    CursorMoveStaffDown();
                    break;

                case CursorMovements.SkipStaffUp:
                    CursorMoveStaffUp();
                    break;
            }

            NotifyObserverScrollToCursor();
        }

        /// <summary>
        /// Applies the movementType to the cursor and notifies observers of the editor.
        /// </summary>
        /// <param name="mouvementType"></param>
        public void MoveCursor(CursorMovements mouvement)
        {
            MoveCursorWithoutNotifyingObservers(mouvement);

            NotifyObserverRedraw();
        }

        /// <summary>
        /// Enlarge the cursor by widthIncrease without going out of 
        /// bound of the tablature.
        /// </summary>
        /// <param name="widthIncrease"></param>
        public void EnlargeCursorWidth(int widthIncrease)
        {
            if (widthIncrease < 0)
                return;

            if (Cursor.LastXValue() + widthIncrease >= TabLength - 1)
                return;

            Cursor.enlargeWidth(widthIncrease);
        }

        /// <summary>
        /// see Tablature.AddString(...) for details.
        /// </summary>
        /// <param name="note"></param>
        /// <param name="addBellow"></param>
        internal void AddString(Note note, bool addBellow)
        {
            Tablature.AddString(addBellow, note);
            NotifyObserverRedraw();
        }

        internal void ChangeTuning(Tuning newTuning, bool scaleNotes)
        {
            Tablature.ChangeTuning(newTuning, scaleNotes);
            NotifyObserverRedraw();
        }

        /// <summary>
        /// See tablature.RemoveString(...) for details.
        /// </summary>
        /// <param name="atEnd"></param>
        /// <param name="destructive"></param>
        internal void RemoveString(bool atEnd, bool destructive)
        {
            Tablature.RemoveString(atEnd, destructive);
            Cursor.Reset();
            NotifyObserverRedraw();
        }

        /// <summary>
        /// Makes the cursor select the area at tabCoord
        /// </summary>
        public void Select(TabCoord tabCoord)
        {
            _cursor.SetTabCoords(tabCoord);

            NotifyObserverRedraw();
        }

        /// <summary>
        /// Drags the secondary cursor tabCoord to select an area greater than 1x1.
        /// </summary>
        public void SelectUpTo(TabCoord dragableCoord)
        {
            _cursor.DragableCoord = dragableCoord;

            NotifyObserverRedraw();
        }

        /// <summary>
        /// Toggle betweens available writing modes. (units, 10th or 20th)
        /// </summary>
        public void ToggleWriteMode()
        {
            switch (WriteMode)
            {
                case WriteModes.Unity: WriteMode = WriteModes.Tenth; break;
                case WriteModes.Tenth: WriteMode = WriteModes.Twenyth; break;
                case WriteModes.Twenyth: WriteMode = WriteModes.Unity; break;
            }

            NotifyObserverRedraw();
        }

        /// <summary>
        /// Gets the tabCoords currently selected bu the cursor.
        /// </summary>
        public List<TabCoord> GetSelectedTabCoords()
        {
            return _cursor.GetSelectedTabCoords();
        }
        #endregion

        #region Private

        private void CursorMoveStaffUp()
        {
            if (!IsCursorTouchingFirstStaff())
                SkipCursorUp();
        }

        private void CursorMoveStaffDown()
        {
            if (IsCursorTouchingLastStaff())
            {
                Tablature.AddNewStaff();
                NotifyObserverNewStaffAdded();
            }

            SkipCursorDown();
        }

        private void CursorMoveUp()
        {
            if (IsCursorTouchingFirstString())
            {
                SkipCursorUp();
            }
            else
            {
                _cursor.BaseCoord.y--;
                _cursor.DragableCoord.y--;
            }
        }

        private void CursorMoveLeft()
        {
            if (!IsCursorTouchingFirstPosition())
            {
                _cursor.BaseCoord.x--;
                _cursor.DragableCoord.x--;
            }
        }

        private void CursorMoveDown()
        {
            if (IsCursorTouchingLastString() && IsCursorTouchingLastStaff())
            {
                Tablature.AddNewStaff();
                NotifyObserverNewStaffAdded();
                SkipCursorDown();
            }
            else if (IsCursorTouchingLastString())
            {
                SkipCursorDown();
            }
            else
            {
                _cursor.BaseCoord.y++;
                _cursor.DragableCoord.y++;
            }
        }

        private void CursorMoveRight()
        {
            if (IsCursorTouchingLastPosition())
            {
                Tablature.AddNewStaff();
                NotifyObserverNewStaffAdded();
            }

            _cursor.BaseCoord.x++;
            _cursor.DragableCoord.x++;
        }

        private void CursorExpandUp()
        {
            _cursor.DragableCoord.y = Math.Max(--_cursor.DragableCoord.y, 0);
        }

        private void CursorExpandLeft()
        {
            _cursor.DragableCoord.x = Math.Max(--_cursor.DragableCoord.x, 0);
        }

        private void CursorExpandDown()
        {
            _cursor.DragableCoord.y = Math.Min(++_cursor.DragableCoord.y, Tablature.NStrings);
        }

        private void CursorExpandRight()
        {
            _cursor.DragableCoord.x = Math.Min(++_cursor.DragableCoord.x, Tablature.Length - 1);
        }

        private void SkipCursorUp()
        {
            int staffLenght = Tablature.StaffLength;

            _cursor.BaseCoord.x
                = _cursor.BaseCoord.x - staffLenght >= 0
                ? _cursor.BaseCoord.x - staffLenght
                : _cursor.BaseCoord.x;

            _cursor.DragableCoord.x
                = _cursor.DragableCoord.x - staffLenght >= 0
                ? _cursor.DragableCoord.x - staffLenght
                : _cursor.DragableCoord.x;
        }

        private void SkipCursorDown()
        {
            int staffLenght = Tablature.StaffLength;

            _cursor.BaseCoord.x
                = _cursor.BaseCoord.x + staffLenght < Tablature.Length
                ? _cursor.BaseCoord.x + staffLenght
                : _cursor.BaseCoord.x;

            _cursor.DragableCoord.x
                = _cursor.DragableCoord.x + staffLenght < Tablature.Length
                ? _cursor.DragableCoord.x + staffLenght
                : _cursor.DragableCoord.x;
        }

        private void ApplyCursorMovementBaseOnInput(char ch)
        {
            bool isWritingTwoNumber = WriteMode != WriteModes.Unity && Util.IsNumber(ch);

            // move cursor to the next char position
            MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            // move cursor again is input was more than one char at the same time (ex: 10)
            if (isWritingTwoNumber)
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            // move cursor again if we are in skipModes.One
            if (SkipMode == SkipModes.One)
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);
        }

        private List<int> GetStaffsTouchingNumbers()
        {
            List<int> staffNumbers = new List<int>();
            int firstX = _cursor.FirstXValue();
            int lastX = _cursor.LastXValue();

            for (var i = firstX; i <= lastX; i++)
            {
                int currentStaffNumber = i / Tablature.StaffLength;
                staffNumbers.Add(currentStaffNumber);
            }
            return staffNumbers;
        }

        private bool IsCursorTouchingLastStaff()
        {
            return GetStaffsTouchingNumbers().IndexOf(Tablature.NStaff - 1) != -1;
        }

        private bool IsCursorTouchingFirstStaff()
        {
            return GetStaffsTouchingNumbers().IndexOf(0) != -1;
        }

        private bool IsCursorTouchingLastString()
        {
            return Math.Max(_cursor.BaseCoord.y, _cursor.DragableCoord.y) == Tablature.NStrings - 1;
        }

        private bool IsCursorTouchingFirstString()
        {
            return Math.Min(_cursor.BaseCoord.y, _cursor.DragableCoord.y) == 0;
        }

        private bool IsCursorTouchingLastPosition()
        {
            return Math.Max(_cursor.BaseCoord.x, _cursor.DragableCoord.x) == Tablature.Length - 1;
        }

        private bool IsCursorTouchingFirstPosition()
        {
            return Math.Min(_cursor.BaseCoord.x, _cursor.DragableCoord.x) <= 0;
        }
        #endregion

        #region Ascii
        public void ParseAscii(string ascii)
        {
            AsciiManipulation.PasteAsciiAtCursor(this, ascii);

            NotifyObserverRedraw();
        }

        public string SelectionToAscii()
        {
            return AsciiManipulation.SelectionToAscii(this);
        }

        public string ToAscii()
        {
            string ascii = "";
            ascii += Tablature.SongInfo.ToString();
            ascii += "Tuning\t\t: " + Tablature.Tuning.ToString();
            ascii += "\r\n\r\n";
            ascii += AsciiManipulation.AsciiFromTablature(Tablature);
            return ascii;
        }

        public void FromAscii(string ascii)
        {
            Cursor.Reset();
            _tablature = AsciiManipulation.TablatureFromAscii(ascii);

            NotifyObserverRedraw();
        }
        #endregion

        #region Observer
        private List<IObserver> observers = new List<IObserver>();

        public void NotifyObserverRedraw()
        {
            observers.ForEach(o => o.NotifyRedraw());
        }

        public void NotifyObserverNewStaffAdded()
        {
            observers.ForEach(o => o.NotifyNewStaffAdded());
        }

        public void NotifyObserverScrollToCursor()
        {
            observers.ForEach(o => o.NotifyScrollToCursor());
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }
        #endregion

        #region Memento
        public Memento GetMemento()
        {
            return new Memento(_cursor, Tablature);
        }

        public void UpdateToMemento(Memento memento)
        {
            _tablature = memento.Tablature;
            _cursor = memento.Cursor;

            NotifyObserverRedraw();
        }
        #endregion

        private enum WriteModes { Unity, Tenth, Twenyth };
        private enum SkipModes { Zero, One };

        #region Recording
        public void StartRecording()
        {
            sp.StartNoteRecognition();
        }

        public void OnPitchDetectedHandler(PitchTracker sender, PitchTracker.PitchRecord pitchRecord)
        {
            string noteName = PitchDsp.GetNoteName(sender.CurrentPitchRecord.MidiNote, true, true);
            if (noteName == null)
                return;

            noteName = noteName.Replace(" ", "");
            Note note = Note.ParseStringWithOctave(noteName);
            List<int> stringIndexes = NoteConversion.GetStringsIndexContainingNotes(note, Tablature.Tuning);
            if (!(stringIndexes.Count > 0))
                return;

            int stringIndex = stringIndexes[0];
            int? fret = NoteConversion.NoteToFret(note, Tablature.Tuning.notes[stringIndex]);
            if (!fret.HasValue)
                return;

            int x = Cursor.TopLeftTabCoord().x;
            Cursor.BaseCoord = (new TabCoord(x, 0));
            Cursor.DragableCoord = (new TabCoord(x, NStrings - 1));


            AttemptSetNoteAt(new TabCoord(x, stringIndex), fret.Value);
            MoveCursorWithoutNotifyingObservers(CursorMovements.Right);
            MoveCursorWithoutNotifyingObservers(CursorMovements.Right);

            NotifyObserverRedraw();
        }

        public void AttemptSetNoteAt(TabCoord tabCoord, int fret)
        {
            int tenth = fret / 10;
            int unity = fret % 10;

            if (fret > 9)
            {
                Tablature.AttemptSetNoteCharAt(tabCoord, tenth.ToString()[0]);
                MoveCursorWithoutNotifyingObservers(CursorMovements.Right);
                Tablature.AttemptSetNoteCharAt(new TabCoord(tabCoord.x + 1, tabCoord.y), unity.ToString()[0]);
            }
            else
                Tablature.AttemptSetNoteCharAt(new TabCoord(tabCoord.x, tabCoord.y), unity.ToString()[0]);

        }

        public void StopRecording()
        {
            sp.StopNoteRecognition();
        }
        #endregion

    }
}
