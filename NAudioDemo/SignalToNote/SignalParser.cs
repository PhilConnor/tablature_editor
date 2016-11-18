using NAudio.Wave;
using Pitch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAudioDemo.SignalToNote
{
    class SignalParser
    {
        private PitchTracker _pitchTracker;
        private WaveIn _waveIn;
        private float[] m_audioBuffer;
        private float m_sampleRate;
        private int m_timeInterval;
        PitchTracker.PitchDetectedHandler onNote;

        public SignalParser(PitchTracker.PitchDetectedHandler onNote)
        {
            Init(onNote);
        }

        /// <summary>
        /// Initialise variables
        /// </summary>
        public void Init(PitchTracker.PitchDetectedHandler onNote)
        {
            _waveIn = new WaveIn();
            _pitchTracker = new PitchTracker();

            // 44.1kHz - 44 100 samples per seconds
            m_sampleRate = 44100.0f;

            // 100ms between each sample processings
            m_timeInterval = 100;

            // Pitchtracker init
            _pitchTracker.SampleRate = m_sampleRate;

            // Audio buffer that will contain our signal floats
            m_audioBuffer = new float[(int)Math.Round(m_sampleRate * m_timeInterval / 1000.0)];

            // Receive signal from default audio input devce (index is always 0 for default device)
            _waveIn.DeviceNumber = 0;

            _waveIn.DataAvailable += waveIn_DataAvailable; // TODO
            _waveIn.BufferMilliseconds = m_timeInterval; // every 100ms triggers dataAvailable

            // Set wave format to sample rate as mono (1 channel)
            _waveIn.WaveFormat = new WaveFormat((int)m_sampleRate, 1);

            _pitchTracker.PitchDetected += OnPitchDetected;
            this.onNote = onNote;
            //_pitchTracker.PitchDetected += onNote;

        }

        /// <summary>
        /// Print audio devices for debuging purpose.
        /// </summary>
        public void PrintSoundCards()
        {
            // Write soundcards to console
            int waveInDevices = WaveIn.DeviceCount;

            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);

                Console.WriteLine("Device {0}: {1}, {2} channels",
                    waveInDevice, deviceInfo.ProductName, deviceInfo.Channels);
            }
        }

        public void StartNoteRecognition()
        {
            _waveIn.StartRecording();
        }

        public void StopNoteRecognition()
        {
            _waveIn.StopRecording();
        }

        public void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // process current buffer
            _pitchTracker.ProcessBuffer(BytePairsToFloats(e.Buffer));
        }

        int n = 0;
        string prevNoteName = null;
        string currNoteName = null;

        string realNoteDetected = null;
        string prevRealNoteDetected = null;


        public void OnPitchDetected(PitchTracker sender, PitchTracker.PitchRecord pitchRecord)
        {
            //Debug.WriteLine("note:" + PitchDsp.GetNoteName(_pitchTracker.CurrentPitchRecord.MidiNote, true, true));
            currNoteName = PitchDsp.GetNoteName(sender.CurrentPitchRecord.MidiNote, true, true);

            if (currNoteName == prevNoteName)
            {
                n++;
            }
            else 
            {
                n = 0;
            }

            if (n > 3)
            {
                realNoteDetected = currNoteName;

                if (realNoteDetected != prevRealNoteDetected && realNoteDetected != null)
                    onNote(sender, pitchRecord);

                prevRealNoteDetected = realNoteDetected;
            }

            prevNoteName = currNoteName;
        }

        public float[] BytePairsToFloats(byte[] bytes)
        {
            float[] floats = new float[bytes.Length / 2];

            int i = 0;
            for (int index = 0; index < bytes.Length; index += 2)
            {
                short sample = (short)((bytes[index + 1] << 8) |
                                        bytes[index + 0]);
                float sample32 = sample / 32768f;

                floats[i] = sample32;
                i++;
            }
            return floats;
        }
    }
}
