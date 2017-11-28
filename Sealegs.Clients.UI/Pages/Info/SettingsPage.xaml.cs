using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Sealegs.Clients.Portable;
using FormsToolkit;

namespace Sealegs.Clients.UI
{
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel ViewModel = App.Locator.SettingsViewModel;
        public SettingsPage ()
        {
            InitializeComponent ();


            BindingContext = ViewModel;
            var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.AboutItems.Count + 1;
            ListViewAbout.HeightRequest = (ViewModel.AboutItems.Count * ListViewAbout.RowHeight) - adjust;
            ListViewAbout.ItemTapped += (sender, e) => ListViewAbout.SelectedItem = null;
            adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.TechnologyItems.Count + 1;
            ListViewTechnology.HeightRequest = (ViewModel.TechnologyItems.Count * ListViewTechnology.RowHeight) - adjust;
            ListViewTechnology.ItemTapped += (sender, e) => ListViewTechnology.SelectedItem = null;
        }

        bool dialogShown;
        int count;
        async void OnTapGestureRecognizerTapped (object sender, EventArgs args)
        {
            count++;
            if (dialogShown || count < 8)
                return;

            dialogShown = true;

            await DisplayAlert ("Credits",
                               "The Xamarin Evolve mobile apps were handcrafted by Xamarins spread out all over the world.\n\n" +
                                "Development:\n" +
                                "James Montemagno\n" +
                                "Pierce Boggan\n" +
                               "\n" +
                                "Design:\n" +
                                "Antonio García Aprea\n" +
                               "\n" +
                                "Testing:\n" +
                                "Ethan Dennis\n" +
                               "\n" +
                                "Many thanks to:\n" +
                                "Fabio Cavalcante\n" +
                                "Matisse Hack\n" +
                                "Sweetkriti Satpathy\n" +
                                "Andrew Branch\n" +
                               "\n" +
                               "...and of course you! <3", "OK");
            
        }
    }
}

