using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Sealegs.Clients.Portable;
using FormsToolkit;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI
{
    public partial class EventsPage : ContentPage
    {
     
        private readonly EventsViewModel _viewModel = App.Locator.EventsViewModel; 

        public EventsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadEventsCommand.Execute(null);
        }

        #region Manage Events 
        private void ListViewEvents_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var ev = ListViewEvents.SelectedItem as FeaturedEvent;
            if (ev == null)
                return;


            _viewModel.EventsDetailNavigateCommand.Execute(ev);
            App.Logger.TrackPage(AppPage.Event.ToString(), ev.Title);
            ListViewEvents.SelectedItem = null;
        }
      

        #endregion


        private void ListViewEvents_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }
    }
}

