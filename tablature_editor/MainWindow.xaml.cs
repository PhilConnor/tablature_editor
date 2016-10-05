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
        TablatureEditorController tabController;


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

            TablatureEditor tabEditor = new Models.TablatureEditor(tablature, cursor);

            tabController = new TablatureEditorController(drawSurface, tabEditor);
        }

        public void Setup()
        {
            drawSurface.Height = Config_DrawSurface.Height;
            drawSurface.Width = Config_DrawSurface.Width;
            window.Background = new SolidColorBrush(Config_DrawSurface.BGColor);
        }

        private void window_TextInput(object sender, TextCompositionEventArgs e)
        {
            tabController.window_TextInput(sender, e);
        }

        private void window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            tabController.window_PreviewKeyDown(sender, e);
        }

    } // 
}
