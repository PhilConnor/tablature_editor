using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PFE.MenuDialogs
{
    /// <summary>
    /// Interaction logic for TransposeDialog.xaml
    /// </summary>
    public partial class TransposeDialog : Window
    {
        //Properties.
        public bool ApplyToSelection { get; set; }
        public bool Increment { get; set; }
        public int NumberOfSemiTone { get; set; }

        //Constructors.
        public TransposeDialog()
        {
            InitializeComponent();
        }

        //Private methods.
        //Button Ok.
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            //Put values into properties.
            ApplyToSelection = (bool)radio_Selection.IsChecked;
            Increment = (bool)radio_Increment.IsChecked;
            NumberOfSemiTone = int.Parse(tb_NbSemiTone.Text);

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
