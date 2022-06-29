using RealEstateApp.Services.Repository;
using System;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealEstateApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var container = TinyIoCContainer.Current;
            container.Register<IRepository, MockRepository>();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
