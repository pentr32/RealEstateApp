using RealEstateApp.Models;
using RealEstateApp.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace RealEstateApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyListPage : ContentPage
    {
        IRepository _repository;
        public ObservableCollection<PropertyListItem> PropertiesCollection { get; set; } = new ObservableCollection<PropertyListItem>();
        public Location _myLocation;

        public PropertyListPage()
        {
            InitializeComponent();
            SortAsync();

            _repository = TinyIoCContainer.Current.Resolve<IRepository>();

            //ItemsListView.ItemsSource = PropertiesCollection;
            LoadProperties();


            BindingContext = this;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadProperties();
        }

        void OnRefresh(object sender, EventArgs e)
        {
            ItemsListView.RefreshCommand = new Command(() =>
            {
                LoadProperties();
                ItemsListView.IsRefreshing = false;
            });
        }

        void LoadProperties()
        {
            PropertiesCollection.Clear();
            var items = _repository.GetProperties();

            foreach (Property item in items)
            {
                item.Distance = Location.CalculateDistance((double)item.Latitude, (double)item.Longitude, _myLocation, DistanceUnits.Kilometers);
                PropertiesCollection.Add(new PropertyListItem(item));
            }
        }
        private async void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new PropertyDetailPage(e.Item as PropertyListItem));
        }

        private async void AddProperty_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditPropertyPage());
        }

        private async void SortAsync()
        {
            _myLocation = await Geolocation.GetLastKnownLocationAsync();
            if (_myLocation == null) _myLocation = await Geolocation.GetLocationAsync();
        }

        private void ToolBarSorting_Clicked(object sender, EventArgs e)
        {
            PropertiesCollection = new ObservableCollection<PropertyListItem>(PropertiesCollection.OrderBy(x => x.Property.Distance));
        }
    }
}