using System.Windows;
using System.Windows.Controls;
using PFE.Models;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace PFE.MenuDialogs
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ChangeTuningDialog : Window
    {
        //Properties.
        public string Tuning { get; set; }
        public IList<Note> selectedNotes = new List<Note>();

        //Constructors.
        public ChangeTuningDialog()
        {
            InitializeComponent();
            cb_StandardTuning.ItemsSource = Note.GetListNotes();

            //TODO: Get the number of strings in the editor object.
            for (int i = 0; i < 6; ++i)
            {
                stringsPanel.Children.Add(CreateAndReturnStringChanger(i));
            }
        }

        //Private methods.
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            //TODO : Program the preset of tuning.
            //Tuning = cb_StandardTuning.SelectedValue.ToString();

            //For each string in the tablature...
            for (int i = 0; i < 6; ++i)
            {
                //Retreive the comboBox and textBox object of a string.
                ComboBox comboBoxNote = (ComboBox)LogicalTreeHelper.FindLogicalNode(stringsPanel, "cb_Note" + i);
                TextBox tb_Octave = (TextBox)LogicalTreeHelper.FindLogicalNode(stringsPanel, "tb_Octave" + i);
                Note stringNote = (Note)comboBoxNote.SelectedItem;

                //Construct the Note object.
                int noteNumericalEquivalent = int.Parse(tb_Octave.Text) * 12 + (int)stringNote.Value;
                stringNote.SetNumericalEquivalent(noteNumericalEquivalent);

                //Add the Note to the list.
                selectedNotes.Add(stringNote);
            }

            //Close the window et return the values.
            DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Create a component that can modify a string of the tablature.
        /// </summary>
        /// <param name="i">The number to put into the name of the objects.</param>
        /// <returns>The WrapPanel, ComboBox and TextBox needed to modify a string.</returns>
        private FrameworkElement CreateAndReturnStringChanger(int i)
        {
            //Create the needed FrameworkElements.
            WrapPanel container = new WrapPanel();
            ComboBox combobox = new ComboBox();
            TextBox textbox = new TextBox();

            //Initialise the ComboBox.
            combobox.Name = "cb_Note" + i;
            combobox.HorizontalAlignment = HorizontalAlignment.Left;
            combobox.VerticalAlignment = VerticalAlignment.Top;
            combobox.Width = 48;
            combobox.SelectedIndex = 0;
            combobox.ItemsSource = Note.GetListNotes();
            
            //Initialise the Textbox.
            textbox.Name = "tb_Octave" + i;
            textbox.HorizontalAlignment = HorizontalAlignment.Left;
            textbox.VerticalAlignment = VerticalAlignment.Top;
            textbox.Height = 23;
            textbox.Width = 28;
            textbox.Text = "2";
            textbox.TextWrapping = TextWrapping.Wrap;
            textbox.MaxLength = 2;
            textbox.RenderTransformOrigin = new Point(-1.614, 0.225);
            textbox.PreviewTextInput += new TextCompositionEventHandler(NumberValidationTextBox);

            //Add the elements to WrapPanel.
            container.Children.Add(combobox);
            container.Children.Add(textbox);

            return container;
        }

        //Filter on textbox.
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Allows number between 0 and 99.
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
