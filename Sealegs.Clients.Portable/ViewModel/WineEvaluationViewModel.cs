using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using MvvmHelpers;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class WineEvaluationViewModel:BaseViewModel
    {
        private INavigationService _navService;

        public WineEvaluationViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        #region Observable Properties

        #region No Wines Found

        private bool _noWinesFound;
        public bool NoWinesFound
        {
            get => _noWinesFound;
            set
            {
                _noWinesFound = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region FeedbackList

        private ObservableRangeCollection<FeedbackExteneded> _feedbackList;
        public ObservableRangeCollection<FeedbackExteneded> FeedbackList
        {
            get => _feedbackList;
            set
            {
                _feedbackList = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #endregion

        #region MyRegion
        public RelayCommand RefreshCommand=>new RelayCommand(Refreshing);
        #endregion

        #region Events

        private  void Refreshing()
        {
            IsListBusy = true;
            Intilize();
            IsListBusy = false;
        }
        #endregion

        #region Methods

        public async void Intilize()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                ListVisibilty = false;
                NoWinesFound = false;

                var list = await FeedbackDb.GetAll();
                if (!list.Any())
                {
                    NoWinesFound = true;
                    ListVisibilty = false;
                }
                else
                {
                    FeedbackList = new ObservableRangeCollection<FeedbackExteneded>(list);
                   
                    ListVisibilty = true;
                    NoWinesFound = false;
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                IsBusy = false;
            }

        }

        #endregion
    }
}
