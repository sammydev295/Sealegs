using System;

using Xamarin.Forms;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;
using Sealegs.Clients.UI.Pages.News;

namespace Sealegs.Clients.UI
{
    public partial class NewsPage : ContentPage
    {
        private readonly NewsViewModel _viewModel = App.Locator.NewsViewModel;

        public NewsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
          
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadNews();
        }

        private void ListViewNews_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem==null)return;
            var news = ListViewNews.SelectedItem as News;
            if (news == null)
                return;
            _viewModel.NewsDetailNavigateCommand.Execute(news);
          
        }

        private void ListViewNews_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }
    }
}

