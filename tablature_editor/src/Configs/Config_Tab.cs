using System.Windows.Media;

namespace PFE.Configs
{
    // Contains de configurations for the current tablature.
    public static class Config_Tab
    {
        // Tuning and number of string.
        public static string Tuning { get; set; } // Tablature parameters.
        public static int NStrings
        {
            get
            {
                return Tuning.Length;
            }
        }

        // Number of staffs.
        public static int NStaff { get; set; }
        public static int StaffLength { get; set; }
        
        public static int TabLength
        {
            get
            {
                return NStaff * StaffLength;
            }
        }
        
        //Constructors.
        public static void Initialisation()
        {
            // Editable.
            NStaff = 3;
            StaffLength = 80;
            Tuning = "EADGBe";
        }
    }
}






