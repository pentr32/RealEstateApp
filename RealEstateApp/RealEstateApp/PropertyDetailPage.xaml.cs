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
        IRepository _repository;
        public Agent Agent { get; set; }
        public Property Property { get; set; }
        private bool isSpeeching;
        private CancellationTokenSource cts;

        public PropertyDetailPage(PropertyListItem propertyListItem)
        {
            InitializeComponent();

            Property = propertyListItem.Property;

            _repository = TinyIoCContainer.Current.Resolve<IRepository>();
            Agent = _repository.GetAgents().FirstOrDefault(x => x.Id == Property.AgentId);

            BindingContext = this;
        }

        private async void EditProperty_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddEditPropertyPage(Property));
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
            else if(!isSpeeching)
            {
                ButtonSpeechDesc.Text = "\uf04b";
                if (cts?.IsCancellationRequested ?? true)
                    return;

                cts.Cancel();
            }

            ButtonSpeechDesc.Text = "\uf04b";
        }

        private void ButtonSpeechDesc_Clicked(object sender, EventArgs e)
        {
            TextToSpeech_Description();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImageListPage(Property.ImageUrls));
        }
    }
}