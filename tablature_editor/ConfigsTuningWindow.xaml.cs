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

namespace PFE
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ConfigsTuningWindow : Window
    {
        //Enum-like class with all possible musical notes.
        public sealed class Notes
        {
            //Attributs.
            private readonly String name;
            private readonly int value;

            //List of all the notes there is.
            public static readonly Notes A = new Notes(0, "A");
            public static readonly Notes ASharp = new Notes(1, "A#");
            public static readonly Notes B = new Notes(2, "B");
            public static readonly Notes C = new Notes(3, "C");
            public static readonly Notes CSharp = new Notes(4, "C#");
            public static readonly Notes D = new Notes(5, "D");
            public static readonly Notes DSharp = new Notes(6, "D#");
            public static readonly Notes E = new Notes(7, "E");
            public static readonly Notes F = new Notes(8, "F");
            public static readonly Notes FSharp = new Notes(9, "F#");
            public static readonly Notes G = new Notes(10, "G");
            public static readonly Notes GSharp = new Notes(11, "G#");

            //Constructors.
            private Notes(int value, string name)
            {
                this.name = name;
                this.value = value;
            }

            public static IEnumerable<Notes> GetListNotes()
            {
                List<Notes> notes = new List<Notes>();
                notes.Add(A);
                notes.Add(ASharp);
                notes.Add(B);
                notes.Add(C);
                notes.Add(CSharp);
                notes.Add(D);
                notes.Add(DSharp);
                notes.Add(E);
                notes.Add(F);
                notes.Add(FSharp);
                notes.Add(G);
                notes.Add(GSharp);

                return notes.AsEnumerable<Notes>();
            }

            //Override of the ToString method.
            public override string ToString()
            {
                return name;
            }
        }

        //Properties.
        public string TuningTest { get; set; }

        //Constructors.
        public ConfigsTuningWindow()
        {
            InitializeComponent();
            cb_note.ItemsSource = Notes.GetListNotes();
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
