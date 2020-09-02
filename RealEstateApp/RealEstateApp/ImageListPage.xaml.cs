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
    public partial class ImageListPage : CarouselPage
    {
        #region Properties
        public List<string> ImageUrls { get; set; }
        SensorSpeed speed = SensorSpeed.Game;
        #endregion

        #region LifeTime
        protected override void OnAppearing()
        {
            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
            Accelerometer.Start(speed);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            Accelerometer.Stop();
            Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
            Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
            base.OnDisappearing();
        }
        #endregion LifeTime

        #region Constructor and other methods
        public ImageListPage(List<string> _imageUrls)
        {
            InitializeComponent();

            ImageUrls = _imageUrls;
            LoadImages();
        }

        void LoadImages()
        {
            foreach (string image in ImageUrls)
            {
                Thickness padding;
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                    case Device.Android:
                        padding = new Thickness(0, 40, 0, 0);
                        break;
                    default:
                        padding = new Thickness();
                        break;
                }

                var imageContentPage = new ContentPage
                {
                    Padding = padding,
                    Content = new StackLayout
                    {
                        Children = {
                            new Image
                            {
                                Source = image,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.CenterAndExpand
                            }
                        }
                    }
                };

                Children.Add(imageContentPage);
            }
        }
        #endregion Constructor and other methods

        #region Events
        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {

        }
        #endregion
    }
}