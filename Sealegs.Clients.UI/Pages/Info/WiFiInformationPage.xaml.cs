using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Sealegs.Clients.Portable;

namespace Sealegs.Clients.UI
{
    public partial class WiFiInformationPage : ContentPage
    {
        ConferenceInfoViewModel vm;
        public WiFiInformationPage()
        {
            InitializeComponent();
            BindingContext = vm = new ConferenceInfoViewModel();
        }

        protected override async void OnAppearing ()
        {
            base.OnAppearing ();

            await vm.UpdateConfigs ();
        }
    }
}

