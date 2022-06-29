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
    public partial class ImageListPage : ContentPage
    {
        #region Properties
        public List<string> ImageUrls { get; set; }
        SensorSpeed speed = SensorSpeed.Game;
        #endregion

        #region LifeTime
        protected override void OnAppearing()
        {
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Start(speed);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            Accelerometer.Stop();
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            base.OnDisappearing();
        }
        #endregion LifeTime

        #region Constructor and other methods
        public ImageListPage(List<string> _imageUrls)
        {
            InitializeComponent();

            ImageUrls = _imageUrls;
            BindingContext = this;
        }
        #endregion Constructor and other methods

        #region Events
        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var reading = e.Reading;

            if(reading.Acceleration.X > 0)
            {
                if (CarouselViewer.Position == ImageUrls.Count - 1) CarouselViewer.Position = 0;

                else
                {
                    CarouselViewer.Position = CarouselViewer.Position = 1;
                }
            }


            else if(reading.Acceleration.X < 0)
            {
                if (CarouselViewer.Position == 0) CarouselViewer.Position = ImageUrls.Count - 1;

                else
                {
                    CarouselViewer.Position = CarouselViewer.Position - 1;
                }
            }
        }
        #endregion
    }
}