using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FormsToolkit;
using Xamarin.Forms;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoreLinq;
using MvvmHelpers;

using Sealegs.Clients.Portable.Locator;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class WinesViewModel : BaseViewModel
    {
        #region Fields

        private INavigationService _navigationService;
        
        #endregion

        #region CTOR

        public WinesViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion

        #region Initialize

        public async void Initialize()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                NoWinesFound = false;
                WineIsVisible = false;
                TopIsVisible = false;

                var winesInLocker = await WinesDb.GetAllWinesById(User.UserID.ToString());
                if (!winesInLocker.Any())
                {
                    NoWinesFound = true;
                    WineIsVisible = false;
                }
                else
                {
                    winesInLocker.Where(s => String.IsNullOrEmpty(s.ImagePath)).ForEach(s => s.ImagePath = "wine_placeholder.png");
                    CompleteWinesList = new ObservableRangeCollection<Wine>(winesInLocker);
                    InitlizeRemoteChillRequests();

                    Upcoming_OnClick();

                    WineIsVisible = true;
                    NoWinesFound = false;
                }

            }
            catch (Exception ex)
            {
                WineIsVisible = false;
                NoWinesFound = true;
                Logger.Report(ex, "Method", "Initialize");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
                TopIsVisible = true;
            }

        }

        #endregion

        #region Observable Properties

        #region Wine Lists

        public ObservableRangeCollection<RemoteChillRequest> RemoteChillRequests { get; set; }
        public ObservableRangeCollection<Wine> CompleteWinesList { get; set; }

        private ObservableRangeCollection<Wine> _wines;
        public ObservableRangeCollection<Wine> Wines
        {
            get => _wines;
            set
            {
                _wines = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region SelectedWine

        private Wine _selectedWine;
        public Wine SelectedWine
        {
            get => _selectedWine;
            set
            {
                _selectedWine = value;
                RaisePropertyChanged();
            }
        }

        #endregion

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

        #region Locker

        private LockerMember _locker;
        public LockerMember Locker
        {
            get => _locker;
            set
            {
                _locker = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Bound Properties

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                RaisePropertyChanged();
            }
        }

        private bool _wineIsVisible;
        public bool WineIsVisible
        {
            get => _wineIsVisible;
            set
            {
                _wineIsVisible = value;
                RaisePropertyChanged();
            }
        }

        private Color _passedTextColor;
        public Color PassedTextColor
        {
            get => _passedTextColor;
            set
            {
                _passedTextColor = value;
                RaisePropertyChanged();
            }
        }
        private Color _passedBackgroundColor;
        public Color PassedBackgroundColor
        {
            get => _passedBackgroundColor;
            set
            {
                _passedBackgroundColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _upcomingBackgroundColor;
        public Color UpcomingBackgroundColor
        {
            get => _upcomingBackgroundColor;
            set
            {
                _upcomingBackgroundColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _upcomingTextColor;
        public Color UpcomingTextColor
        {
            get => _upcomingTextColor;
            set
            {
                _upcomingTextColor = value;
                RaisePropertyChanged();
            }
        }

        private bool _topIsVisible;
        public bool TopIsVisible
        {
            get => _topIsVisible;
            set
            {
                _topIsVisible = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #endregion

        #region NextForceRefresh 

        public DateTime NextForceRefresh { get; set; }
        
        #endregion

        #region Relay Commands

        public RelayCommand ForceRefreshCommand => new RelayCommand(ForceRefresh);
        public RelayCommand LoadWinesCommand => new RelayCommand(Initialize);
        public RelayCommand PassedCommand => new RelayCommand(Passed_OnClick);
        public RelayCommand UpcomingCommand => new RelayCommand(Upcoming_OnClick);

        #endregion

        #region Events

        private void Passed_OnClick()
        {

            UpcomingTextColor = Color.FromHex("#F5F5F5");
            UpcomingBackgroundColor = Color.FromHex("#7635EB");
            PassedTextColor = Color.FromHex("#7635EB");
            PassedBackgroundColor = Color.FromHex("#F5F5F5");
            var list = CompleteWinesList.Where(s => s.IsChecked.Value);
            Wines = new ObservableRangeCollection<Wine>(list);
        }

        private void Upcoming_OnClick()
        {
            PassedTextColor = Color.FromHex("#F5F5F5");
            PassedBackgroundColor = Color.FromHex("#7635EB");
            UpcomingTextColor = Color.FromHex("#7635EB");
            UpcomingBackgroundColor = Color.FromHex("#F5F5F5");
            var list = CompleteWinesList.Where(s => !s.IsChecked.Value);
            Wines = new ObservableRangeCollection<Wine>(list);


        }
        private void ForceRefresh()
        {
            try
            {
                IsRefreshing = true;
                Initialize();
            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadWinesAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public void ItemSelected(Wine selectedWine)
        {
            _navigationService.NavigateTo(ViewModelLocator.WineDetail, selectedWine);

        }

        #endregion

        #region Methods

        private async void InitlizeRemoteChillRequests()
        {
            var list = await RemoteChilledRequestDb.GetAllByLockerId(User.UserID.ToString());
            RemoteChillRequests = new ObservableRangeCollection<RemoteChillRequest>(list);
            FilterChilledRequestWines();
        }

        private void FilterChilledRequestWines()
        {
            try
            {
                foreach (var item in RemoteChillRequests)
                {
                    foreach (var wine in CompleteWinesList)
                    {
                        if (wine.MemberBottleID == item.MemberBottleID)
                        {
                            if (CompleteWinesList.Contains(wine))
                            {
                                int selectedIndex = CompleteWinesList.Select((w, index) => new { w, index }).First(w => w.w.Id == wine.Id).index;
                                Wine selectedWine = CompleteWinesList[selectedIndex];
                                selectedWine.IsChilledRequestSent = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
               
            }
          
            //  var wine = CompleteWinesList.First(s => s.LockerID == item.LockerMemberID);
        }

        #endregion
    }
}
