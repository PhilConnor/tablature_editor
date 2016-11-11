using System.Windows;
using PFE.Models;

namespace PFE.MenuDialogs
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ChangeTuningDialog : Window
    {
        //Properties.
        public string TuningTest { get; set; }

        //Constructors.
        public ChangeTuningDialog()
        {
            InitializeComponent();
            cb_Note.ItemsSource = Note.GetListNotes();
        }

        //Private methods.
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            TuningTest = cb_Note.SelectedValue.ToString();
            DialogResult = true;
            this.Close();
        }
    }
}
