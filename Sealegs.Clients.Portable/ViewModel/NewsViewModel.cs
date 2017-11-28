using System;
using Sealegs.DataObjects;
using Sealegs.Clients.Portable.Locator;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FormsToolkit;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using MvvmHelpers;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class NewsViewModel : BaseViewModel
    {
        #region GLobal Variables
        private readonly INavigationService _navService;
        #endregion

        #region CTOR
        public NewsViewModel(INavigationService navigation)
        {
            _navService = navigation;
        }
        #endregion

        #region Observable Properties

        public string EditIcon
        {
            get
            {

                if (User != null && User.Role.RoleName == "Admin")
                    return "btnLocker.png";
                return String.Empty;
            }
        }

        private ObservableRangeCollection<News> _news = new ObservableRangeCollection<News>();
        public ObservableRangeCollection<News> News
        {
            get => _news;
            set
            {
                _news = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region SelectedNews
        private News _selectedNews;
        public News SelectedNews
        {
            get => _selectedNews;
            set
            {
                _selectedNews = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Relay Commands
        public RelayCommand ForceRefreshCommand => new RelayCommand(ForceRefresh);
        public RelayCommand LoadNewsCommand => new RelayCommand(LoadNews);
        public RelayCommand<News> NewsDetailNavigateCommand => new RelayCommand<News>(NewsDetailNavigate);
        public RelayCommand ManageCommand => new RelayCommand(Manage_OnClick);

        #endregion

        #region Events
        private async void ForceRefresh()
        {
            try
            {
                IsListBusy = true;
                var news = await NewsDb.GetAll();
                News.ReplaceRange(news);

            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadEventsAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsListBusy = false;
            }
        }
        
        private void NewsDetailNavigate(News news)
        {
            _navService.NavigateTo(ViewModelLocator.NewsDetails, news);
        }
        private void Manage_OnClick()
        {
            if (User != null && User.Role.RoleName == "Admin" && !IsBusy)
                _navService.NavigateTo(ViewModelLocator.NewsManagment, News);
        }
        #endregion

        #region Methods
        public async void LoadNews()
        {
            if (IsBusy)
                return;
            try
            {
                   IsBusy = true;
                var news = await NewsDb.GetAll();
                News = new ObservableRangeCollection<News>(news);

            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadEventsAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region Commented Code

        //    IEnumerable<News> news = await StoreManager.NewsStore.GetItemsAsync(true);
        //   var orderedEnumerable = news.OrderBy(o => o.CreatedAt).Reverse();
        //public ObservableRangeCollection<News> News { get; } = new ObservableRangeCollection<News>();
        //public ObservableRangeCollection<Grouping<string, News>> NewsGrouped { get; } = new ObservableRangeCollection<Grouping<string, News>>(); 
        //void SortNews(IEnumerable<News> theNews)
        //{
        //    //var newsRanked = from news in theNews
        //    //                 orderby news.Name, news.Rank
        //    //                 orderby news.NewsLevel.Rank
        //    //                 select news;

        //    //News.ReplaceRange(newsRanked);

        //    //var groups = from news in News
        //    //             group news by news.NewsLevel.Name
        //    //    into newsGroup
        //    //             select new Grouping<string, News>(newsGroup.Key, newsGroup);

        //    //NewsGrouped.ReplaceRange(groups);
        //}
        //private News _selectedNews;
        // public News SelectedNews
        // {
        //     get { return _selectedNews; }
        //     set
        //     {
        //         _selectedNews = value;
        //         RaisePropertyChanged();
        //         if (_selectedNews == null)
        //             return;

        //         MessagingService.Current.SendMessage(MessageKeys.NavigateToNews, _selectedNews);

        //         SelectedNews = null;
        //     }
        // }

        //        ICommand forceRefreshCommand;
        //        public ICommand ForceRefreshCommand =>
        //        forceRefreshCommand ?? (forceRefreshCommand = new Command(async () => await ExecuteForceRefreshCommandAsync()));

        //        async Task ExecuteForceRefreshCommandAsync()
        //        {
        //            await ExecuteLoadNewsAsync(true);
        //        }

        //        ICommand loadNewsCommand;
        //        public ICommand LoadNewsCommand =>
        //            loadNewsCommand ?? (loadNewsCommand = new Command(async (f) => await ExecuteLoadNewsAsync()));

        //        async Task<bool> ExecuteLoadNewsAsync(bool force = false)
        //        {
        //            if (IsBusy)
        //                return false;

        //            try
        //            {
        //                IsBusy = true;

        //#if DEBUG
        //                await Task.Delay(1000);
        //#endif
        //                var news = await StoreManager.NewsStore.GetItemsAsync(force);

        //                SortNews(news);

        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.Report(ex, "Method", "ExecuteLoadNewsAsync");
        //                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
        //            }
        //            finally
        //            {
        //                IsBusy = false;
        //            }

        //            return true;
        //        }
        #endregion
    }
}

