using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using Sealegs.Clients.Portable.Locator;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class NewsManagmentViewModel : BaseViewModel
    {
        #region Global
        private readonly INavigationService _navService; 
        #endregion
        #region CTOR
        public NewsManagmentViewModel(INavigationService navService)
        {
            _navService = navService;
        }
        #endregion

        #region Observable Properties

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

        #region News
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

        #endregion

        #region Relay Command
        public RelayCommand AddCommand => new RelayCommand(Add);

        #endregion

        #region Events
        private void Add()
        {
           _navService.NavigateTo(ViewModelLocator.AddEditNews);
        }
        public async void EditOrDelete(DataObjects.News news)
        {

            var result =await UserDialogs.Instance.ActionSheetAsync("Select", "Cancel", null, null, "Edit", "Delete");
            if (result == "Edit")
            {
                _navService.NavigateTo(ViewModelLocator.AddEditNews, news);
                return;
            }
            if (result == "Delete")
            {
                var isDeleted = await NewsDb.DeleteNews(news.Id);
                if (isDeleted)
                {
                    _navService.GoBack();
                }
            }
            
        }
        #endregion
    }
}
