using System;
using System.Collections.Generic;

using Xamarin.Forms;
using FormsToolkit;

using Sealegs.Clients.Portable;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI
{
    public partial class EventDetailsPage : ContentPage
    {
        private readonly EventDetailsViewModel _viewModel = App.Locator.EventDetailsViewModel;

        public EventDetailsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;

            _viewModel.LoadEventDetailsCommand.Execute(null);

        }

        public EventDetailsPage(FeaturedEvent featuredEvent)
        {
            InitializeComponent();
            BindingContext = _viewModel;

            _viewModel.Initlize(featuredEvent);
        }
    }
}

