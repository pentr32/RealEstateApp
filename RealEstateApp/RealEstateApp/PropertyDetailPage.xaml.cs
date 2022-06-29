using Android.Content.PM;
using RealEstateApp.Models;
using RealEstateApp.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstateApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyDetailPage : ContentPage
    {
        #region Properties
        IRepository _repository;
        public Agent Agent { get; set; }
        public Property Property { get; set; }
        private bool isSpeeching;
        private CancellationTokenSource cts;
        public List<string> Emails = new List<string>();
        #endregion Properties

        #region Constructor and other methods
        public PropertyDetailPage(PropertyListItem propertyListItem)
        {
            InitializeComponent();

            Property = propertyListItem.Property;

            _repository = TinyIoCContainer.Current.Resolve<IRepository>();
            Agent = _repository.GetAgents().FirstOrDefault(x => x.Id == Property.AgentId);
            Emails.Add(Property.Vendor.Email);

            BindingContext = this;
        }

        public async void TextToSpeech_Description()
        {
            isSpeeching = !isSpeeching;

            var settings = new SpeechOptions()
            {
                Volume = 1.0f,
                Pitch = 1.0f
            };

            if (isSpeeching)
            {
                cts = new CancellationTokenSource();

                ButtonSpeechDesc.Text = "\uf04d";
                await TextToSpeech.SpeakAsync(Property.Description, settings, cancelToken: cts.Token);
            }
            else if (!isSpeeching)
            {
                ButtonSpeechDesc.Text = "\uf04b";
                if (cts?.IsCancellationRequested ?? true)
                    return;

                cts.Cancel();
            }

            ButtonSpeechDesc.Text = "\uf04b";
        }
        #endregion Constructor and other methods

        #region Events
        private async void EditProperty_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddEditPropertyPage(Property));
        }

        private void ButtonSpeechDesc_Clicked(object sender, EventArgs e)
        {
            TextToSpeech_Description();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImageListPage(Property.ImageUrls));
        }

        private async void TapGestureRecognizer_Tapped_Email(object sender, EventArgs e)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = $"Angående {Property.Address}",
                    Body = "",
                    To = Emails
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

        private async void TapGestureRecognizer_Tapped_Phone(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet(Property.Vendor.Phone, "Cancel", null, "Call", "SMS");

            switch (action)
            {
                case "Call":
                    try
                    {
                        PhoneDialer.Open(Property.Vendor.Phone);
                    }
                    catch (ArgumentNullException anEx)
                    {
                        // Number was null or white space
                    }
                    catch (FeatureNotSupportedException ex)
                    {
                        // Phone Dialer is not supported on this device.
                    }
                    catch (Exception ex)
                    {
                        // Other error has occurred.
                    }
                    break;

                case "SMS":
                    try
                    {
                        await Sms.ComposeAsync(new SmsMessage($"Hej, {Property.Vendor.FirstName}! Angående {Property.Address}, er rigtig interreset i det, kan du ringer mig for mere info?", 
                            Property.Vendor.Phone));
                    }
                    catch (FeatureNotSupportedException ex)
                    {
                        // Sms is not supported on this device.
                    }
                    catch (Exception ex)
                    {
                        // Other error has occurred.
                    }
                    break;
            }
        }

        private async void ButtonMapDestination_Clicked(object sender, EventArgs e)
        {
            var placemarks = await Geocoding.GetPlacemarksAsync((double)Property.Latitude, (double)Property.Longitude);

            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                try
                {
                    await Map.OpenAsync(placemark);
                }
                catch (Exception ex)
                {
                    // No map application available to open or placemark can not be located
                }
            }
        }

        private async void ButtonLinked_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync(Property.NeighbourhoodUrl, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.DarkBlue,
                    PreferredControlColor = Color.Violet
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async void ButtonNavigateToDestination_Clicked(object sender, EventArgs e)
        {
            var location = new Location((double)Property.Latitude, (double)Property.Longitude);
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

            try
            {
                await Map.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                // No map application available to open or placemark can not be located
            }
        }

        private async void ButtonExternalLink_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Launcher.OpenAsync(Property.NeighbourhoodUrl);
            }
            catch (Exception ex)
            {

            }
        }

        private async void ButtonFileSignature_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(Property.ContractFilePath)
                });
            }
            catch (Exception ex)
            {

            }
        }

        #endregion Events
    }
}