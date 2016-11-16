using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;
using PFE.Models;
using System.Windows.Input;
using System.Windows.Controls;

namespace PFE.MenuDialogs
{
    /// <summary>
    /// Interaction logic for AddStringDialog.xaml
    /// </summary>
    public partial class AddStringDialog : Window
    {
        //Properties.
        public bool AddBellow { get; set; }
        public Note NewStringNote { get; set; }

        //Constructors.
        public AddStringDialog()
        {
            InitializeComponent();

            NewStringNote = new Note();
            cb_Note.ItemsSource = Note.GetListNotes();
        }

        //Private methods.
        //Button Ok.
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            Note comboBoxNote = (Note)cb_Note.SelectedItem;

            //Put radio button selection in AddBellow.
            AddBellow = (bool)radio_Bellow.IsChecked;

            //Find the selected Note.
            int noteNumericalEquivalent = int.Parse(tb_Octave.Text) * 12 + (int)comboBoxNote.Value;
            NewStringNote.SetNumericalEquivalent(noteNumericalEquivalent);

            //Close the dialog and return the values.
            DialogResult = true;
            this.Close();
        }

        //Button Cancel.
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            //Close the dialog without returning the values.
            DialogResult = false;
            this.Close();
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
