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
using PFE.MenuDialogs;

namespace PFE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EditorController editorController;

        public MainWindow()
        {
            InitializeComponent();

            Config_DrawSurface.Inst().Initialisation();

            // Setup
            Setup();

            // Init & Dependancy injection
            Cursor cursor = new Cursor();
            Models.Tablature tablature = new Models.Tablature();
            Editor editor = new Editor(tablature, cursor);
            editorController = new EditorController(editor, drawSurface, _scrollViewer);
        }

        /// <summary>
        /// Setup the window and drawSurface.
        /// </summary>
        public void Setup()
        {
            window.Width = Config_DrawSurface.Inst().Window_Width;
            window.Height = Config_DrawSurface.Inst().Window_Height;
            window.Background = new SolidColorBrush(Config_DrawSurface.Inst().BGColor);
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void window_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            editorController.TextInput(e);
        }

        private void window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            editorController.KeyDown(e);
        }

        private void drawSurface_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)            
                editorController.MouseDrag(sender, e);            
        }

        private void drawSurface_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            editorController.MouseDown(sender, e);
        }

        private void drawSurface_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            editorController.MouseUp(sender, e);
        }

        //Show a dialog so the user can select a file to open.
        private void MenuPrincipal_File_Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.DefaultExt = ".apptab";                  //Default file extension.
            openFileDialog.Filter = "Tablature (.apptab)|*.apptab"; //Filter by files extension.

            //Show open file dialog box.
            if (openFileDialog.ShowDialog() == true)
            {
                //Save tablature.
                string filename = openFileDialog.FileName;
                MessageBox.Show("Path of selected file : " + filename);
            }
        }

        //Show a dialog so the user can select a file to save his tablature to.
        private void MenuPrincipal_File_Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.FileName = "newtablature";               //Default file name.
            saveFileDialog.DefaultExt = ".apptab";                  //Default file extension.
            saveFileDialog.Filter = "Tablature (.apptab)|*.apptab"; //Filter by files extension.

            //Show save file dialog box.
            if (saveFileDialog.ShowDialog() == true)
            {
                //Save tablature.
                string filename = saveFileDialog.FileName;
                MessageBox.Show("Path of selected file : " + filename);
            }
        }

        private void MenuPrincipal_File_Quitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuPrincipal_Strings_AddString_Click(object sender, RoutedEventArgs e)
        {
            AddStringDialog addStringDialog = new AddStringDialog();
            if (addStringDialog.ShowDialog() == true)
            {
                MessageBox.Show("You picked : " + addStringDialog.Element.ToStringWithOctave() + " and add bellow = " + addStringDialog.AddBellow);
            }
        }

        private void MenuPrincipal_Strings_RemoveString_Click(object sender, RoutedEventArgs e)
        {
            RemoveStringDialog removeStringDialog = new RemoveStringDialog();
            if (removeStringDialog.ShowDialog() == true)
            {
                MessageBox.Show("You picked : " + "Bellow = " + removeStringDialog.RemoveBellow + " and Destructive : " + removeStringDialog.Destructive);
            }
        }

        private void MenuPrincipal_Configs_Tablature_Click(object sender, RoutedEventArgs e)
        {
            ChangeTuningDialog tuningWindow = new ChangeTuningDialog();
            if (tuningWindow.ShowDialog() == true)
            {
                MessageBox.Show("You picked : " + tuningWindow.TuningTest);
            }
        }
    }
}
