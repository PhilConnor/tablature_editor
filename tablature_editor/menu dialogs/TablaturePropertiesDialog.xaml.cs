using PFE.Models;
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
    /// Interaction logic for TablaturePropertiesDialog.xaml
    /// </summary>
    public partial class TablaturePropertiesDialog : Window
    {
        //Properties.
        public string AuthorName { get; set; }
        public string SongName { get; set; }
        public string Instrument { get; set; }

        private SongInfo songInfo;

        //Constructors.
        public TablaturePropertiesDialog(Tablature tablature)
        {
            InitializeComponent();

            songInfo = tablature.SongInfo;
            LoadTablatureProperties();
        }

        //Private methods.
        private void LoadTablatureProperties()
        {
            //Put values into Textbox.
            tb_ArtistName.Text = songInfo.Artist;
            tb_SongName.Text = songInfo.Title;
            tb_Instrument.Text = songInfo.Instrument;
        }

        //Button Ok.
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            //Put values into SongInfo.
            songInfo.Artist = tb_ArtistName.Text;
            songInfo.Title = tb_SongName.Text;
            songInfo.Instrument = tb_Instrument.Text;

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
    }
}
