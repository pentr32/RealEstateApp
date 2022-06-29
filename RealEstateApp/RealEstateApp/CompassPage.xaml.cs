using RealEstateApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstateApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompassPage : ContentPage
    {
        public CompassItem CompassAspect { get; set; }
        SensorSpeed speed = SensorSpeed.UI;
        List<CompassHelper> CompassHelpers { get; set; }

        public CompassPage(CompassItem compassItem)
        {
            InitializeComponent();
            LoadHelpers();
            ToggleCompass();
            CompassAspect = compassItem;

            Compass.ReadingChanged += Compass_ReadingChanged;
        }

        void LoadHelpers()
        {
            CompassHelpers = new List<CompassHelper>
            {
                new CompassHelper
                {
                    CurrentAspectValue = 360.00,
                    CurrentAspectName = "North",
                    CurrentAspectNameAbr = "N",
                    CurrentAspectColor = Color.Red
                },

                new CompassHelper
                {
                    CurrentAspectValue = 45.00,
                    CurrentAspectName = "North East",
                    CurrentAspectNameAbr = "NE",
                    CurrentAspectColor = Color.Gray
                },

                new CompassHelper
                {
                    CurrentAspectValue = 90.00,
                    CurrentAspectName = "East",
                    CurrentAspectNameAbr = "E",
                    CurrentAspectColor = Color.Gray
                },
                
                new CompassHelper
                {
                    CurrentAspectValue = 135.00,
                    CurrentAspectName = "South East",
                    CurrentAspectNameAbr = "SE",
                    CurrentAspectColor = Color.Gray
                },

                new CompassHelper
                {
                    CurrentAspectValue = 180.00,
                    CurrentAspectName = "South",
                    CurrentAspectNameAbr = "S",
                    CurrentAspectColor = Color.Gray
                },
                
                new CompassHelper
                {
                    CurrentAspectValue = 225.00,
                    CurrentAspectName = "South West",
                    CurrentAspectNameAbr = "SW",
                    CurrentAspectColor = Color.Gray
                },

                new CompassHelper
                {
                    CurrentAspectValue = 270.00,
                    CurrentAspectName = "West",
                    CurrentAspectNameAbr = "W",
                    CurrentAspectColor = Color.Gray
                },
                
                new CompassHelper
                {
                    CurrentAspectValue = 315.00,
                    CurrentAspectName = "North West",
                    CurrentAspectNameAbr = "NW",
                    CurrentAspectColor = Color.Gray
                },
            };
        }

        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            var data = e.Reading;
            CompassAspect.CurrentHeading = data.HeadingMagneticNorth;
            CompassAspect.RotationAngle = data.HeadingMagneticNorth;

            CompassHelper closestCorner = CompassHelpers.Aggregate((x, y) => Math.Abs(x.CurrentAspectValue - data.HeadingMagneticNorth) < Math.Abs(y.CurrentAspectValue - data.HeadingMagneticNorth) ? x : y);
            CompassAspect.CurrentAspectAbr = closestCorner.CurrentAspectNameAbr;
            CompassAspect.CurrentAspect = closestCorner.CurrentAspectName;
            aspectSpan.TextColor = closestCorner.CurrentAspectColor;

            BindingContext = this;
        }

        public void ToggleCompass()
        {
            try
            {
                if (Compass.IsMonitoring)
                    Compass.Stop();
                else
                    Compass.Start(speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Some other exception has occurred
            }
        }
    }
}