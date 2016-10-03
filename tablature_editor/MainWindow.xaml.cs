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
            Configuration.Initialisation();

            // Setup
            canvasCustom.Height = Configuration.CanvasHeight;
            canvasCustom.Width = Configuration.CanvasWidth;
            window.Background = new SolidColorBrush(Configuration.BGColor);
            cursorController = new CursorController();
       
            // Init & Dependancy injection
            editorFacade = new TabEditor(new Tab(), cursorController);
            tabController = new TabController(canvasCustom, editorFacade);

        }

        private void window_TextInput(object sender, TextCompositionEventArgs e)
        {
            //text
            editorFacade.writeCharAtCursor(e.Text);
        }

        private void window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            //shift+arrow
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Left)
                editorFacade.moveCursor(CursorMovements.ExpandLeft);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Up)
                editorFacade.moveCursor(CursorMovements.ExpandUp);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Right)
                editorFacade.moveCursor(CursorMovements.ExpandRight);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Down)
                editorFacade.moveCursor(CursorMovements.ExpandDown);

            //arrow
            else if (e.Key == Key.Left)
                editorFacade.moveCursor(CursorMovements.Left);
            else if (e.Key == Key.Up)
                editorFacade.moveCursor(CursorMovements.Up);
            else if (e.Key == Key.Right)
                editorFacade.moveCursor(CursorMovements.Right);
            else if (e.Key == Key.Down)
                editorFacade.moveCursor(CursorMovements.Down);

            //backspace, delete
            else if (e.Key == Key.Back || e.Key == Key.Delete)
                editorFacade.writeCharAtCursor("-");

            else if (e.Key == Key.CapsLock)
                editorFacade.toggleWriteMode();
        }

    } // 
}
