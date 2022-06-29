using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstateApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class BarometerMeasurement
    {
        public double Pressure { get; set; }
        public double Altitude { get; set; }
        public string Label { get; set; }
        public double? HeightChange { get; set; }

        public string Display => $"{Label}: {Altitude:N2}m";
    }
}
