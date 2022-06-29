using RealEstateApp.Models;
using RealEstateApp.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstateApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditPropertyPage : ContentPage
    {
        private IRepository Repository;
        public ObservableCollection<Agent> Agents { get; }
        public CompassItem CompassAspect { get; set; } = new CompassItem();

        private Property _property;
        public Property Property
        {
            get => _property;
            set
            {
                _property = value;
                if (_property.AgentId != null)
                {
                    SelectedAgent = Agents.FirstOrDefault(x => x.Id == _property?.AgentId);
                }

            }
        }

        private Agent _selectedAgent;

        public Agent SelectedAgent
        {
            get => _selectedAgent;
            set
            {
                if (Property != null)
                {
                    _selectedAgent = value;
                    Property.AgentId = _selectedAgent?.Id;
                }
            }
        }

        public string StatusMessage { get; set; }

        public Color StatusColor { get; set; } = Color.White;

        public AddEditPropertyPage(Property property = null)
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += ConnectionChecker;
            CheckNetworkConnectivity();

            Battery.BatteryInfoChanged += Battery_BatteryInfoChanged;
            Battery.EnergySaverStatusChanged += OnEnergySaverStatusChanged;

            Repository = TinyIoCContainer.Current.Resolve<IRepository>();
            Agents = new ObservableCollection<Agent>(Repository.GetAgents());

            if (property == null)
            {
                Title = "Add Property";
                Property = new Property();
            }
            else
            {
                Title = "Edit Property";
                Property = property;
            }

            BindingContext = this;
        }

        private async void SaveProperty_Clicked(object sender, System.EventArgs e)
        {
            if (IsValid() == false)
            {
                StatusMessage = "Please fill in all required fields";
                StatusColor = Color.Red;

                try
                {
                    var duration = TimeSpan.FromSeconds(5);
                    Vibration.Vibrate(duration);
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                }
            }
            else
            {
                Repository.SaveProperty(Property);
                await Navigation.PopToRootAsync();
            }
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Property.Address)
                || Property.Beds == null
                || Property.Price == null
                || Property.AgentId == null)
                return false;

            return true;
        }

        private async void CancelSave_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void LocationButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();
                if(location != null)
                {
                    Property.Latitude = location.Latitude;
                    Property.Longitude = location.Longitude;

                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        var geocodeAddress =
                            $"{placemark.CountryCode}\n" +
                            $"{placemark.CountryName}\n" +
                            $"{placemark.FeatureName}\n" +
                            $"{placemark.Locality}\n" +
                            $"{placemark.PostalCode}\n" +
                            $"{placemark.Thoroughfare}\n" +
                            $"{placemark.SubThoroughfare}\n";

                        Property.Address = geocodeAddress;
                    }
                }
            }

            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        private async void HouseAddressButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(EntryAddress.Text))
                {
                    await DisplayAlert("Alert", "Tekstfelt er tom", "OK");
                }
                else
                {
                    var locations = await Geocoding.GetLocationsAsync(EntryAddress.Text);

                    var location = locations?.FirstOrDefault();
                    if (location != null)
                    {
                        Property.Latitude = location.Latitude;
                        Property.Longitude = location.Longitude;
                    }
                }
            }

            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        private async void CheckNetworkConnectivity()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) 
            {
                await DisplayAlert("Alert", "Du har ikke internet. Gå ind i dit indstillinger og slå det til", "OK");
                LocationButton.IsEnabled = false;
                HouseAddressButton.IsEnabled = false;
            }

            else if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                LocationButton.IsEnabled = true;
                HouseAddressButton.IsEnabled = true;
            }
        }

        private void ConnectionChecker(object sender, ConnectivityChangedEventArgs e)
        {
            CheckNetworkConnectivity();
        }

        private void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
        {

            if(e.ChargeLevel <= 0.20)
            {
                switch (Battery.State)
                {
                    case BatteryState.NotCharging:
                        StatusMessage = "Battery level is under 20%";
                        StatusColor = Color.Red;
                        break;

                    case BatteryState.Charging:
                        StatusMessage = "Battery level is under 20%";
                        StatusColor = Color.Yellow;
                        break;
                }
            }
            else
            {
                StatusMessage = "";
            }    
        }

        private void OnEnergySaverStatusChanged(object sender, EnergySaverStatusChangedEventArgs e)
        {
            var status = e.EnergySaverStatus;

            switch (status)
            {
                case EnergySaverStatus.On:
                    StatusMessage = "Batteri niveau er lav!";
                    StatusColor = Color.Green;
                    break;

                case EnergySaverStatus.Off:
                    StatusMessage = "";
                    break;
            }
        }

        private async void AspectProp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompassPage(CompassAspect));
        }
    }
}