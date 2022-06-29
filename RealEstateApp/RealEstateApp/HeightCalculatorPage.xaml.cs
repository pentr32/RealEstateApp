using RealEstateApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstateApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeightCalculatorPage : ContentPage
    {
        #region Properties
        SensorSpeed speed = SensorSpeed.UI;
        public double CurrentPressure { get; set; }
        public double CurrentAltitude { get; set; }
        public string MeasurementLabel { get; set; }
        public ObservableCollection<BarometerMeasurement> Measurements { get; set; } = new ObservableCollection<BarometerMeasurement>();
        #endregion Properties

        public HeightCalculatorPage()
        {
            InitializeComponent();
            ToggleBarometer();
            BindingContext = this;
        }

        #region LifeTime
        protected override void OnAppearing()
        {
            Barometer.ReadingChanged += Barometer_ReadingChanged;
        }

        protected override void OnDisappearing()
        {
            Barometer.ReadingChanged -= Barometer_ReadingChanged;
        }
        #endregion LifeTime

        private void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
        {
            var data = e.Reading;
            CurrentPressure = data.PressureInHectopascals;
            CurrentAltitude = 44307.694 * (1 - Math.Pow(data.PressureInHectopascals / 1013, 0.190284));
        }

        public void ToggleBarometer()
        {
            try
            {
                if (Barometer.IsMonitoring)
                    Barometer.Stop();
                else
                    Barometer.Start(speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        #region Events
        private void SaveMeasurementButton_Clicked(object sender, EventArgs e)
        {
            BarometerMeasurement foundMeasurement = Measurements.FirstOrDefault(x => x.Label == MeasurementLabel);

            if(foundMeasurement != null)
            {
                foundMeasurement.HeightChange = CurrentAltitude - foundMeasurement.Altitude;
            }

            else
            {
                Measurements.Add(new BarometerMeasurement
                {
                    Pressure = CurrentPressure,
                    Altitude = CurrentAltitude,
                    Label = MeasurementLabel
                });
            }
           

            BindingContext = this;
        }
        #endregion Events
    }
}