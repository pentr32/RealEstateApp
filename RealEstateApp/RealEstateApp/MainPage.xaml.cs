using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealEstateApp
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            menuPage.NavigationListView.ItemSelected += NavigationListView_ItemSelected;
        }

        private void NavigationListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Models.MenuItem item)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.Target));
                menuPage.NavigationListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
