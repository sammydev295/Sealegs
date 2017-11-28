using System;
using System.Collections.ObjectModel;
using MvvmHelpers;
using Sealegs.Clients.Portable.ViewModel;

using Xamarin.Forms;

namespace Sealegs.Clients.UI.Pages.News
{
    public partial class NewsManagmentPage : ContentPage
    {
        readonly NewsManagmentViewModel _viewModel =App.Locator.NewsManagmentViewModel;

        public NewsManagmentPage(ObservableRangeCollection<DataObjects.News> news)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.News = news; 
         
        }
        private void ListViewNews_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem==null)return;
            var news = e.SelectedItem as DataObjects.News;
            _viewModel.EditOrDelete(news);
        }
        //void ListViewTapped(object sender, ItemTappedEventArgs e)
        //{
        //    var list = sender as ListView;
        //    if (list == null)
        //        return;
        //    list.SelectedItem = null;
        //}

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    ListViewNews.ItemTapped += ListViewTapped;
        //    //if (_viewModel.News.Count == 0)
        //    //    _viewModel.LoadNewsCommand.Execute(false);
        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    ListViewNews.ItemTapped -= ListViewTapped;
        //}

        //    private async void SaveEvents_OnClicked(object sender, EventArgs e)
        //    {
        //      //  await Navigation.PopAsync();
        //    }

        //    private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        //    {

        //      //  await Navigation.PushModalAsync(new AddEditNewspage("Add News"));
        //    }
        //    private async void EditOnTapped(object sender, EventArgs e)
        //    {
        //       // await Navigation.PushModalAsync(new AddEditNewspage("Edit News"));
        //    }

        //    private  void DeleteOnTapped(object sender, EventArgs e)
        //    {
        //    }

        //    #region Commented

        //    //if (OS == TargetPlatform.Android)
        //    //ListViewNews.Effects.Add(Effect.Resolve("Xamarin.ListViewSelectionOnTopEffect"));

        //    //ListViewNews.ItemSelected +=  (sender, e) =>
        //    //{
        //    //    var selectNews = ListViewNews.SelectedItem as DataObjects.News;
        //    //    if (selectNews == null)
        //    //        return;
        //    //    App.Logger.TrackPage(AppPage.News.ToString(), selectNews.Name);
        //    //    ListViewNews.SelectedItem = null;

        //    //};

        ////var newsDetails = new NewsDetailsPage();
        ////  newsDetails.News = news;
        ////  await NavigationService.PushAsync(Navigation, newsDetails);
        //#endregion


        private void ListViewNews_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null; 
        }
    }
}
