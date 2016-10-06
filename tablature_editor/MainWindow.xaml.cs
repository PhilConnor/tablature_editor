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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PFE.Controllers;
using PFE.Models;
using PFE.Configs;
using PFE.Utils;

namespace PFE
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

            Config_Tab.Inst().Initialisation();
            Config_DrawSurface.Inst().Initialisation();

            // Setup
            Setup();

            // Init & Dependancy injection
            CursorLogic cursorLogic = new CursorLogic();
            Cursor cursor = new Cursor(cursorLogic);
            Tablature tablature = new Tablature();
            TablatureEditor tablatureEditor = new TablatureEditor(tablature, cursor);

            tabController = new TablatureEditorController(drawSurface, tablatureEditor);
        }

        public void Setup()
        {
            drawSurface.Height = Config_DrawSurface.Inst().Height;
            drawSurface.Width = Config_DrawSurface.Inst().Width;
            window.Width = Config_DrawSurface.Inst().Window_Width;
            window.Height = Config_DrawSurface.Inst().Window_Height;
            window.Background = new SolidColorBrush(Config_DrawSurface.Inst().BGColor);
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        }

        private void window_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            tabController.TextInput(e);
        }

        private void window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            tabController.KeyDown(e);
        }

        private void drawSurface_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                tabController.MouseDrag(sender,e);
            }
        }

        private void drawSurface_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            tabController.MouseDown(sender,e);
        }
    } // 
}
