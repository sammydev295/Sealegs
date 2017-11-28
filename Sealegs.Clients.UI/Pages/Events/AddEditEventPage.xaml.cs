using System;
using Xamarin.Forms;
using Sealegs.DataObjects;
using Sealegs.Clients.Portable.ViewModel;

namespace Sealegs.Clients.UI.Pages.Events

{
    public partial class AddEditEventPage : ContentPage
    {
        private readonly AddEditEventPageViewModel _viewModel = App.Locator.AddEditEventPageViewModel;

        public AddEditEventPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.Event = new DataObjects.FeaturedEvent(){StartTime = DateTime.Now,EndTime = DateTime.Now};
            _viewModel.PageTitle = "Add Events";
            _viewModel.ButtonName = "Save";
        }
        public AddEditEventPage(FeaturedEvent featuredEvent)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.Event = featuredEvent;
            _viewModel.PageTitle = featuredEvent.Title;
            _viewModel.ButtonName = "Update";
        }
    }


}
