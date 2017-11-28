using MvvmHelpers;
using GalaSoft.MvvmLight.Views;

using Sealegs.DataObjects;
using Xamarin.Forms;

using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class NewsDetailsViewModel : BaseViewModel
    {
        #region Global Variables
        private INavigationService _navService;
        #endregion

        #region CTOR
        public NewsDetailsViewModel(INavigationService navigation)
        {
            _navService = navigation;
        }
        #endregion

        #region Observable Properties

        #region Follow Items
        private ObservableRangeCollection<MenuItem> _followItems = new ObservableRangeCollection<MenuItem>();
        public ObservableRangeCollection<MenuItem> FollowItems
        {
            get { return _followItems; }
            set
            {
                _followItems = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Menu Item
        MenuItem _selectedFollowItem;
        public MenuItem SelectedFollowItem
        {
            get { return _selectedFollowItem; }
            set
            {
                _selectedFollowItem = value;
                RaisePropertyChanged();
                if (_selectedFollowItem == null)
                    return;

                LaunchBrowserCommand.Execute(_selectedFollowItem.Parameter);

                SelectedFollowItem = null;
            }
        }
        #endregion

        #region News
        private News _news;
        public News News
        {
            get { return _news; }
            set
            {
                _news = value;
                RaisePropertyChanged();
            }
        }
        #endregion 

        #region Name
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        #endregion 

        #region Description
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region NewsImageUri

        private string _newsImageUri;
        public string NewsImageUri
        {
            get { return _newsImageUri; }
            set
            {
                _newsImageUri = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #endregion

        #region Initlize
        public void Initilize(News news)
        {
            Name = news.Name;
            NewsImageUri = news.ImageUrl;
            Description = news.Description;
            FollowItems = AddListItems(news);
        }
        private ObservableRangeCollection<MenuItem> AddListItems(News news)
        {
            var list = new ObservableRangeCollection<MenuItem>
            {
                new MenuItem
                {
                    Name = "Web",
                    Subtitle = news.WebsiteUrl,
                    Parameter = news.WebsiteUrl,
                    Icon = "icon_website.png"
                },
                new MenuItem
                {
                    Name = Device.OS == TargetPlatform.iOS ? "Twitter" : news.TwitterUrl,
                    Subtitle = $"@{news.TwitterUrl}",
                    Parameter = "http://twitter.com/" + news.TwitterUrl,
                    Icon = "icon_twitter.png"
                }
            };
            return list;
        }
        #endregion
    }
}

