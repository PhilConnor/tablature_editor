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
using tablature_editor.src;
using tablature_editor.src.Controler;
using static tablature_editor.src.Enums;

namespace tablature_editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TablatureEditor _editorFacade;
        TabController _tabcontroller;

        public MainWindow()
        {
            InitializeComponent();

            // Setup
            canvasCustom.Height = Configuration.Inst.canvasHeight;
            canvasCustom.Width = Configuration.Inst.canvasWidth;
            window.Background = new SolidColorBrush(Configuration.Inst.bgColor);            
       
            // Init & Dependancy injection
            _editorFacade = new TablatureEditor(new Tab(), new Cursor());
            _tabcontroller = new TabController(canvasCustom, _editorFacade);

        }

        private void window_TextInput(object sender, TextCompositionEventArgs e)
        {
            //text
            _editorFacade.writeCharAtCursor(e.Text);
        }

        private void window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            //shift+arrow
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Left)
                _editorFacade.moveCursor(CursorMovements.ExpandLeft);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Up)
                _editorFacade.moveCursor(CursorMovements.ExpandUp);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Right)
                _editorFacade.moveCursor(CursorMovements.ExpandRight);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Down)
                _editorFacade.moveCursor(CursorMovements.ExpandDown);

            //arrow
            else if (e.Key == Key.Left)
                _editorFacade.moveCursor(CursorMovements.Left);
            else if (e.Key == Key.Up)
                _editorFacade.moveCursor(CursorMovements.Up);
            else if (e.Key == Key.Right)
                _editorFacade.moveCursor(CursorMovements.Right);
            else if (e.Key == Key.Down)
                _editorFacade.moveCursor(CursorMovements.Down);

            //backspace, delete
            else if (e.Key == Key.Back || e.Key == Key.Delete)
                _editorFacade.writeCharAtCursor("-");

            else if (e.Key == Key.CapsLock)
                _editorFacade.toggleWriteMode();
        }

    } // 
}
