using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstateApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class CompassItem
    {
        public double CurrentHeading { get; set; }
        public double RotationAngle { get; set; }
        public string CurrentAspect { get; set; }
        public string CurrentAspectAbr { get; set; }
    }
}
