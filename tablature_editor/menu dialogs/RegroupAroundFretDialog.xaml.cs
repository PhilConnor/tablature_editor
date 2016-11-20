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
    /// Interaction logic for RegroupAroundFret.xaml
    /// </summary>
    public partial class RegroupAroundFretDialog : Window
    {
        //Properties.
        public bool ApplyToSelection { get; set; }
        public int NumberOfFret { get; set; }

        //Constructors.
        public RegroupAroundFretDialog()
        {
            InitializeComponent();
        }

        //Private methods.
        //Button Ok.
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            //Put values into properties.
            ApplyToSelection = (bool)radio_Selection.IsChecked;
            NumberOfFret = int.Parse(tb_NumberOfFret.Text);

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
