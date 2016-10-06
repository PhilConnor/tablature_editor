using PFE.Interfaces;
using System.Windows.Media;
using System;
using System.Collections.Generic;

namespace PFE.Configs
{
    // Contains de configurations for the current tablature.
    public class Config_Tab : IObservable
    {
        // Tuning and number of string.
        public string Tuning { get; set; } // Tablature parameters.
        public int NStrings
        {
            get
            {
                return Tuning.Length;
            }
        }

        // Number of staffs.
        public int NStaff { get; set; }
        public int StaffLength { get; set; }
        
        public int TabLength
        {
            get
            {
                return NStaff * StaffLength;
            }
        }

        private static Config_Tab config;

        public static Config_Tab Inst()
        {
            if (config == null)
            {
                config = new Config_Tab();
                config.Initialisation();
                return config;
            }
            else
            {
                return config;
            }
        }
        //Constructors.
        public void Initialisation()
        {
            // Editable.
            NStaff = 3;
            StaffLength = 80;
            Tuning = "EADGBe";
        }

        private List<IObserver> observers = new List<IObserver>();
        public void NotifyObserver()
        {
            observers.ForEach(o => o.Notify());
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }
    }
}






