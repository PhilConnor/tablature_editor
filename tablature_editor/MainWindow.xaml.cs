using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TablatureEditor.Controllers;
using TablatureEditor.Models;
using TablatureEditor.Configs;
using TablatureEditor.Utils;
using System.Windows.Input;

namespace TablatureEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Models.TablatureEditor tabEditor;


        public MainWindow()
        {
            InitializeComponent();

            Config_Tab.Initialisation();
            Config_DrawSurface.Initialisation();

            // Setup
            Setup();


            // Init & Dependancy injection
            Models.CursorLogic cursorLogic = new Models.CursorLogic();
            Models.Cursor cursor = new Models.Cursor(cursorLogic);

            Tablature tablature = new Tablature();

            tabEditor = new Models.TablatureEditor(tablature, cursor);

            TablatureEditorController tabController = new TablatureEditorController(drawSurface, tabEditor);
        }

        public void Setup()
        {
            drawSurface.Height = Config_DrawSurface.Height;
            drawSurface.Width = Config_DrawSurface.Width;
            window.Background = new SolidColorBrush(Config_DrawSurface.BGColor);
        }

        private void window_TextInput(object sender, TextCompositionEventArgs e)
        {
            //text
            tabEditor.WriteCharAtCursor(e.Text);
        }

        private void window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            //shift+arrow
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Left)
                tabEditor.MoveCursor(CursorMovements.ExpandLeft);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Up)
                tabEditor.MoveCursor(CursorMovements.ExpandUp);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Right)
                tabEditor.MoveCursor(CursorMovements.ExpandRight);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Down)
                tabEditor.MoveCursor(CursorMovements.ExpandDown);

            //arrow
            else if (e.Key == Key.Left)
                tabEditor.MoveCursor(CursorMovements.Left);
            else if (e.Key == Key.Up)
                tabEditor.MoveCursor(CursorMovements.Up);
            else if (e.Key == Key.Right)
                tabEditor.MoveCursor(CursorMovements.Right);
            else if (e.Key == Key.Down)
                tabEditor.MoveCursor(CursorMovements.Down);

            //backspace, delete
            else if (e.Key == Key.Back || e.Key == Key.Delete)
                tabEditor.WriteCharAtCursor("-");

            else if (e.Key == Key.CapsLock)
                tabEditor.ToggleWriteMode();
        }

    } // 
}
