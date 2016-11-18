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
        private Tuning selectedTuning;
        private Tablature tablature;

        //Constructors.
        public ChangeTuningDialog(Tablature tablature)
        {
            InitializeComponent();
            //cb_StandardTuning.ItemsSource = Note.GetListNotes(); //Legacy - CB with standard tunings.
            this.tablature = tablature;
            selectedTuning = tablature.Tuning;

            //TODO: Get the number of strings in the editor object.
            for (int i = 0; i < tablature.NStrings; ++i)
            {
                stringsPanel.Children.Add(CreateAndReturnStringChanger(i));
            }
        }

        //Private methods.
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            //For each string in the tablature...
            for (int i = 0; i < tablature.NStrings; ++i)
            {
                //Retreive the comboBox and textBox object of a string.
                ComboBox comboBoxNote = (ComboBox)LogicalTreeHelper.FindLogicalNode(stringsPanel, "cb_Note" + i);
                TextBox tb_Octave = (TextBox)LogicalTreeHelper.FindLogicalNode(stringsPanel, "tb_Octave" + i);
                Note stringNote = (Note)comboBoxNote.SelectedItem;

                //Construct the Note object.
                int noteNumericalEquivalent = int.Parse(tb_Octave.Text) * 12 + (int)stringNote.Value;

                //Add the Note to the Tuning object.
                selectedTuning.notes[i].SetNumericalEquivalent(noteNumericalEquivalent);
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
            Note currentNote = tablature.Tuning.notes[i];

            //Create the needed FrameworkElements.
            WrapPanel container = new WrapPanel();
            ComboBox combobox = new ComboBox();
            TextBox textbox = new TextBox();

            //Initialise the ComboBox.
            combobox.Name = "cb_Note" + i;
            combobox.HorizontalAlignment = HorizontalAlignment.Left;
            combobox.VerticalAlignment = VerticalAlignment.Top;
            combobox.Width = 48;
            combobox.SelectedIndex = (int)currentNote.Value;
            combobox.ItemsSource = Note.GetListNotes();
            
            //Initialise the Textbox.
            textbox.Name = "tb_Octave" + i;
            textbox.HorizontalAlignment = HorizontalAlignment.Left;
            textbox.VerticalAlignment = VerticalAlignment.Top;
            textbox.Height = 23;
            textbox.Width = 28;
            textbox.Text = currentNote.Octave.ToString();
            textbox.TextWrapping = TextWrapping.Wrap;
            textbox.MaxLength = 2;
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
