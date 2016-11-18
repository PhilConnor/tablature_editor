using NAudio.Wave;
using NAudioDemo.SignalToNote;
using Pitch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NAudioDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //WPF Init
            InitializeComponent();
            
            SignalParser stn = new SignalParser(OnPitchDetected);
            stn.StartNoteRecognition();

        }

        public void OnPitchDetected(PitchTracker sender, PitchTracker.PitchRecord pitchRecord)
        {
            //Debug.WriteLine("note:" + PitchDsp.GetNoteName(_pitchTracker.CurrentPitchRecord.MidiNote, true, true));
            string noteName = tbox.Text + " - " + PitchDsp.GetNoteName(sender.CurrentPitchRecord.MidiNote, true, true);

            if (noteName != null)
                tbox.Text = noteName;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
