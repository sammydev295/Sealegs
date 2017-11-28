using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Sealegs.Clients.Portable;
using FormsToolkit;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.Clients.UI.Pages.Events;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI
{
    public partial class EventsManagementPage : ContentPage
    {
     
        readonly EventManagementViewModel _viewModel =App.Locator.EventManagementViewModel; 

        public EventsManagementPage(ObservableCollection<FeaturedEvent> events)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.AllEvents = events;
            _viewModel.Intilize(events);
        }
        private void ListViewEvents_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var ev = ListViewEvents.SelectedItem as FeaturedEvent;
            if (ev == null)
                return;
            _viewModel.EditOrDelete(ev);
        }
        private void ListViewEvents_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }

        #region Commented
        //#region Save Events 

        //private async void SaveEvents_OnClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PopAsync();
        //}

        //#endregion

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    if (ViewModel.Events.Count == 0)
        //        ViewModel.LoadEventsCommand.Execute(false);
        //}
        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //}


        //#region TapGestureRecognizer_OnTapped

        //private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        //{

        //    await Navigation.PushModalAsync(new AddEditEventPage("Add Event"));
        //}
        //private async void EditOnTapped(object sender, EventArgs e)
        //{

        //    await  Navigation.PushModalAsync(new AddEditEventPage("Edit Event"));
        //}

        //private async void DeleteOnTapped(object sender, EventArgs e)
        //{
        //    var s = sender as FeaturedEvent;

        //}


        //#endregion
        #endregion


    }
}

