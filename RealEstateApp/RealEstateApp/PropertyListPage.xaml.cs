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

namespace RealEstateApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyListPage : ContentPage
    {
        IRepository _repository;
        public ObservableCollection<PropertyListItem> PropertiesCollection { get; } = new ObservableCollection<PropertyListItem>();
        public PropertyListPage()
        {
            InitializeComponent();

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
            var items = _repository.GetProperties();

            foreach (Property item in items)
            {
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
    }
}