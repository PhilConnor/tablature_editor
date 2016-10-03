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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TablatureEditor.Controllers;
using TablatureEditor.Models;
using TablatureEditor.Configs;
using TablatureEditor.Utils;

namespace TablatureEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TabEditor editorFacade;
        TabController tabController;
        CursorController cursorController;

        public MainWindow()
        {
            InitializeComponent();
            Config_Tab.Initialisation();
            Config_DrawSurface.Initialisation();

            // Setup
            canvasCustom.Height = Config_DrawSurface.Height;
            canvasCustom.Width = Config_DrawSurface.Width;
            window.Background = new SolidColorBrush(Config_DrawSurface.BGColor);
            cursorController = new CursorController();
       
            // Init & Dependancy injection
            editorFacade = new TabEditor(new Tab(), cursorController);
            tabController = new TabController(canvasCustom, editorFacade);

        }

        private void window_TextInput(object sender, TextCompositionEventArgs e)
        {
            //text
            editorFacade.WriteCharAtCursor(e.Text);
        }

        private void window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            //shift+arrow
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Left)
                editorFacade.MoveCursor(CursorMovements.ExpandLeft);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Up)
                editorFacade.MoveCursor(CursorMovements.ExpandUp);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Right)
                editorFacade.MoveCursor(CursorMovements.ExpandRight);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Down)
                editorFacade.MoveCursor(CursorMovements.ExpandDown);

            //arrow
            else if (e.Key == Key.Left)
                editorFacade.MoveCursor(CursorMovements.Left);
            else if (e.Key == Key.Up)
                editorFacade.MoveCursor(CursorMovements.Up);
            else if (e.Key == Key.Right)
                editorFacade.MoveCursor(CursorMovements.Right);
            else if (e.Key == Key.Down)
                editorFacade.MoveCursor(CursorMovements.Down);

            //backspace, delete
            else if (e.Key == Key.Back || e.Key == Key.Delete)
                editorFacade.WriteCharAtCursor("-");

            else if (e.Key == Key.CapsLock)
                editorFacade.ToggleWriteMode();
        }

    } // 
}
