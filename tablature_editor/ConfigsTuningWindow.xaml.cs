using System.Windows;
using PFE.Models;

namespace PFE
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ConfigsTuningWindow : Window
    {
        //Properties.
        public string TuningTest { get; set; }

        //Constructors.
        public ConfigsTuningWindow()
        {
            InitializeComponent();
            cb_note.ItemsSource = Note.GetListNotes();
        }

        //Private methods.
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            TuningTest = cb_note.SelectedValue.ToString();
            DialogResult = true;
            this.Close();
        }
    }
}
