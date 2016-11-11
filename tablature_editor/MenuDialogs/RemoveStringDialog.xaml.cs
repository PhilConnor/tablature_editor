using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for RemoveStringDialog.xaml
    /// </summary>
    public partial class RemoveStringDialog : Window
    {
        //Properties.
        public bool RemoveBellow { get; set; }
        public bool Destructive { get; set; }

        //Constructors.
        public RemoveStringDialog()
        {
            InitializeComponent();
        }

        //Private methods.
        //Button Ok.
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            //Put radio button selection in AddBellow.
            RemoveBellow = (bool)radio_Bellow.IsChecked;
            Destructive = (bool)radio_Destructive.IsChecked;

            //Close the dialog and return the values.
            DialogResult = true;
            this.Close();
        }

        //Button Cancel.
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
