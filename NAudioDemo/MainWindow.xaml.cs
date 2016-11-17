using NAudio.Wave;
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
        private PitchTracker m_pitchTracker;
        private float[] m_audioBuffer;
        private float m_sampleRate;
        private int m_timeInterval;

        public MainWindow()
        {
            m_sampleRate = 44100.0f; //44.1kHz
            m_timeInterval = 100;  // 100ms

            //Pitchtracker init
            m_pitchTracker = new PitchTracker();
            m_pitchTracker.SampleRate = m_sampleRate;
            m_audioBuffer = new float[(int)Math.Round(m_sampleRate * m_timeInterval / 1000.0)];



            //WPF Init
            InitializeComponent();

            //Write soundcards to console
            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                Console.WriteLine("Device {0}: {1}, {2} channels",
                    waveInDevice, deviceInfo.ProductName, deviceInfo.Channels);
            }

            //Starting the shit
            WaveIn waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.BufferMilliseconds = m_timeInterval; // every 100ms triggers dataAvailable
            int sampleRate = (int)m_sampleRate;
            int channels = 1; // mono
            waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
            waveIn.StartRecording();
        }

        int tht = 0;

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            //convert samples to float
            int i = 0;
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((e.Buffer[index + 1] << 8) |
                                        e.Buffer[index + 0]);
                float sample32 = sample / 32768f;

                m_audioBuffer[i] = sample32;
                i++;
            }
            
            // process current buffer
            m_pitchTracker.ProcessBuffer(m_audioBuffer);

            //write current pitch note detected to console
            tht++;
            if (tht == 10)
            {
                tht = 0;
                Debug.WriteLine("note:" + PitchDsp.GetNoteName(m_pitchTracker.CurrentPitchRecord.MidiNote, true, true));
            }
        }
    }
}
