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
        public Note Element { get; set; }

        //Constructors.
        public AddStringDialog()
        {
            InitializeComponent();

            Element = new Note();
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
            Element.SetNumericalEquivalent(noteNumericalEquivalent);
            DialogResult = true;
            this.Close();
        }

        //Button Cancel.
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
