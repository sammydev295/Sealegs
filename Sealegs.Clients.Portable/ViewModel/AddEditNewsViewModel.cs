using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Sealegs.DataObjects;
using Xamarin.Forms;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class AddEditNewsViewModel : BaseViewModel
    {
        private INavigationService _navService;
        public AddEditNewsViewModel(INavigationService navigation)
        {
            _navService = navigation;
        }

        #region News

        private News _news;
        public News News
        {
            get => _news;
            set
            {
                _news = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public RelayCommand AddCommand => new RelayCommand(Add);
        private void Add()
        {
            if (ButtonName == "Update")
            {
                NewsDb.UpdateNews(News);
            }
            else
            {
                NewsDb.InsertNews(News);
            }
            _navService.GoBack(); _navService.GoBack();
        }

        //private ObservableCollection<Links> _linksList;
        //public ObservableCollection<Links> LinksList
        //{
        //    get => _linksList;
        //    set
        //    {
        //        _linksList = value;
        //        RaisePropertyChanged();
        //    }
        //}
        //public int _rowHeight;
        //public int RowHeight
        //{
        //    get => _rowHeight;
        //    set { _rowHeight = value; RaisePropertyChanged(); }
        //}
        //public string _linkName;
        //public string LinkName
        //{
        //    get => _linkName;
        //    set { _linkName = value; RaisePropertyChanged(); }
        //}

        //public string _linkUrl;
        //public string LinkUrl
        //{
        //    get => _linkUrl;
        //    set { _linkUrl = value; RaisePropertyChanged(); }
        //}


        //private void OnDelete(Links e)
        //{
        //    LinksList.Remove(e);
        //    RowHeight -= 40;
        //}
        //private void OnAddLinkClick()
        //{
        //    if (!String.IsNullOrEmpty(LinkName) || !String.IsNullOrEmpty(LinkUrl))
        //    {
        //        LinksList.Add(new Links { Name = LinkName, Url = LinkUrl });
        //        RowHeight += 40;
        //    }

        //}
        //private void OnCancelCick()
        //{
        //    //   Navigation.PopModalAsync();
        //}
    }

    //public class Links
    //{
    //    public string Name { get; set; }
    //    public string Url { get; set; }
    //}
}
