using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

using Sealegs.Clients.Portable;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI
{
    public partial class NewsDetailsPage : ContentPage
    {
        private readonly NewsDetailsViewModel _viewModel = App.Locator.NewsDetailsViewModel;
      
        public NewsDetailsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }
        public NewsDetailsPage(News news)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.Initilize(news);
        }

        //public News News
        //{
        //    get { return ViewModel.News; }
        //    set { BindingContext = new NewsDetailsViewModel(Navigation, value); }
        //}

        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();
        //    //vm = null;
        //    var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.FollowItems.Count + 1;
        //    ListViewFollow.HeightRequest = (ViewModel.FollowItems.Count * ListViewFollow.RowHeight) - adjust;
        //}
        private void ListViewFollow_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
         
        }
    }
}

