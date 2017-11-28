using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;

using Sealegs.Clients.Portable.Locator;
using Sealegs.Clients.Portable.Model.Extensions;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class RemoteChillRequestViewModel : BaseViewModel
    {
        #region CTOR

        private INavigationService _navService;

        public RemoteChillRequestViewModel(INavigationService navService)
        {
            this._navService = navService;
        }

        #endregion

        #region Intialize

        public async void Intialize()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var list = await RemoteChilledRequestDb.GetAll();
                if (!list.Any())
                {
                    NoChillRequestsFound = true;
                    ListVisibilty = false;
                }
                else
                {
                    ChillRequestList = new ObservableRangeCollection<RemoteChillRequestExteneded>(list);
                    var sortedList = ChillRequestList.GroupByName();

                    ChillRequestListGrouped.ReplaceRange(sortedList);
                    NoChillRequestsFound = false;
                    ListVisibilty = true;
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

        public async void ItemSelected()
        {

        }

        #endregion

        #region Observable Properties

        #region Remote Chill Request

        private RemoteChillRequest _remoteChillRequest;
        public RemoteChillRequest RemoteChillRequest
        {
            get => _remoteChillRequest;
            set
            {
                _remoteChillRequest = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Chill Request List

        private ObservableRangeCollection<RemoteChillRequestExteneded> _chillRequestList;
        public ObservableRangeCollection<RemoteChillRequestExteneded> ChillRequestList
        {
            get => _chillRequestList;
            set
            {
                _chillRequestList = value;
                RaisePropertyChanged();
            }
        }


        private ObservableRangeCollection<Grouping<string, RemoteChillRequestExteneded>> _chillRequestListGrouped = new ObservableRangeCollection<Grouping<string, RemoteChillRequestExteneded>>();
        public ObservableRangeCollection<Grouping<string, RemoteChillRequestExteneded>> ChillRequestListGrouped
        {
            get => _chillRequestListGrouped;
            set
            {
                _chillRequestListGrouped = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region NoChillRequestsFound

        private bool _noChillRequestsFound;
        public bool NoChillRequestsFound
        {
            get => _noChillRequestsFound;
            set
            {
                _noChillRequestsFound = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Relay Commands

        public RelayCommand<RemoteChillRequestExteneded> ApproveCommand => new RelayCommand<RemoteChillRequestExteneded>(Approve_OnClick);

        #endregion

        #region Events Handlers & Helpers

        private async void Approve_OnClick(RemoteChillRequestExteneded chillRequestExteneded)
        {
            if (IsBusy) return;
            bool result = false;
            try
            {
                IsBusy = true;

                var chillRequest = chillRequestExteneded.ChillRequest;
                chillRequest.IsCompleted = true;
                chillRequest.CompletedByID = User.UserID.ToString();
                result = await RemoteChilledRequestDb.UpdateRemoteChillRequest(chillRequest);
                if (result)
                {
                    _navService.NavigateTo(ViewModelLocator.Signature, chillRequestExteneded.Wine);
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
