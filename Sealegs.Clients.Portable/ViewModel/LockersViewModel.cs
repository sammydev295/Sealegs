using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;

using Xamarin.Forms;
using MvvmHelpers;
using FormsToolkit;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoreLinq;

using Sealegs.Clients.Portable.Locator;
using Sealegs.DataObjects;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class LockersViewModel : BaseViewModel
    {
        #region Fields 

        private readonly INavigationService navService;
        
        #endregion

        #region CTOR

        public LockersViewModel(INavigationService navigation)
        {
            navService = navigation;
            PageTitle = "All Active Lockers";
        }

        #endregion

        #region Initialize

        public async void Initialize()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                LockersFound = false;
                NoLockersFound = false;

                // Set title
                PageTitle = "All Active Lockers";

                //  IEnumerable<LockerMember> lockers = await LockerMemberStore.GetAllLockerMembers(true);
                IEnumerable<LockerMember> lockers = await LockerMemberDb.GetAllNonStaff();
                IEnumerable<LockerMember> lockerMembers = lockers as IList<LockerMember> ?? lockers.ToList();
                if (!lockerMembers.Any())
                {
                    NoLockersFoundMessage = "No Lockers Found";
                    LockersFound = false;
                    NoLockersFound = true;
                }
                else
                {
                    Lockers.ReplaceRange(lockerMembers);
                    Lockers.Where(l => String.IsNullOrEmpty(l.ProfileImage)).ToList().ForEach(l => l.ProfileImage = LockerMember.DefaultProfileImage);
                    LockersFiltered.ReplaceRange(Lockers);

                    LockersFound = true;
                    NoLockersFound = false;

                    // Locker types loaded just once here since they don't change
                    var lockerTypes = await LockerTypeDb.GetAllLockerTypes();
                    LockerTypes = new List<LockerType>(lockerTypes);

                    // WinecVarietals loaded just once here since they don't change
                    var wineVarietals = await WineVarietalDb.GetAllWineVarietal();
                    WineVarietals = new List<WineVarietal>(wineVarietals);

                    // Check Last Locker Saved Setting
                    var lockerFilter = LockerFilterSettingDb.GetFilterLockers();
                    if (lockerFilter != null)
                    {
                        if (lockerFilter.IsLastUsed)
                            ExecuteFilterLastUsed();

                        if (lockerFilter.IsInActive)
                            ExecuteFilterInActive();

                        if (lockerFilter.IsFavourite)
                            await ExecuteFilterFavoritesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadLockersAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }

        }

        #endregion 

        #region Properties

        #region Lockers

        public ObservableRangeCollection<LockerMember> Lockers { get; } = new ObservableRangeCollection<LockerMember>();

        #endregion

        #region LockersFiltered

        public ObservableRangeCollection<LockerMember> LockersFiltered { get; } = new ObservableRangeCollection<LockerMember>();
        
        #endregion

        #region SelectedLocker

        private LockerMember _selectedLocker;
        public LockerMember SelectedLocker
        {
            get => _selectedLocker;
            set
            {
                _selectedLocker = value;
                RaisePropertyChanged();
                if (_selectedLocker == null)
                    return;

                navService.NavigateTo(ViewModelLocator.LockerDetails, new Tuple<LockerMember, List<LockerType>, List<WineVarietal>>(SelectedLocker, LockerTypes, WineVarietals));
            }
        }

        #endregion

        #region Filtering

        private string _searchFilter = string.Empty;
        public string SearchFilter
        {
            get => _searchFilter;
            set
            {
                _searchFilter = value;
                RaisePropertyChanged();
                ExecuteSearchLockersAsync();
            }
        }

        private bool _lockersFound;

        public bool LockersFound
        {
            get => _lockersFound;
            set
            {
                _lockersFound = value;
                RaisePropertyChanged();
            }

        }

        private bool _noLockersFound;
        public bool NoLockersFound
        {
            get => _noLockersFound;
            set
            {
                _noLockersFound = value;
                RaisePropertyChanged();
            }
        }

        private string _noLockersFoundMessage;
        public string NoLockersFoundMessage
        {
            get => _noLockersFoundMessage;
            set
            {
                _noLockersFoundMessage = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region LockerTypes 

        public List<LockerType> LockerTypes { get; set; }

        #endregion

        #region WineVarietals 

        public List<WineVarietal> WineVarietals { get; set; }

        #endregion

        #endregion

        #region Relay Commands

        public RelayCommand ForceRefreshCommand => new RelayCommand(ExecuteForceRefreshCommandAsync);
        public RelayCommand SearchLockersCommand => new RelayCommand(async () => await ExecuteSearchLockersAsync());
        public RelayCommand FilterLastUsedCommand => new RelayCommand(ExecuteFilterLastUsed);
        public RelayCommand FilterFavoritesCommand => new RelayCommand(async () => await ExecuteFilterFavoritesAsync());
        public RelayCommand FilterInActiveCommand => new RelayCommand(ExecuteFilterInActive);
        public RelayCommand LoadLockersCommand => new RelayCommand(Initialize);
        public RelayCommand AddCommand => new RelayCommand(AddLocker_OnClick);

        #endregion

        #region Event Handlers & Helpers

        #region ExecuteForceRefreshCommandAsync

        private void ExecuteForceRefreshCommandAsync()
        {
            LockerFilterSettingDb.DeleteFilterLockers();
            Initialize();
        }

        #endregion

        private async Task ExecuteSearchLockersAsync()
        {
            IsBusy = true;
            LockersFound = true;

            // Abort the current command if the query has changed and is not empty
            if (!string.IsNullOrEmpty(SearchFilter))
            {
                var query = SearchFilter;
                await Task.Delay(250);
                if (query != SearchFilter)
                    return;
            }

            // Set title
            PageTitle = $"Lockers with Name {SearchFilter}";

            LockersFiltered.ReplaceRange(Lockers.Search(SearchFilter));
            if (!LockersFiltered.Any())
            {
                NoLockersFoundMessage = "No Lockers Found";
                LockersFound = false;
            }
            else
            {
                LockersFound = true;
            }

            IsBusy = false;
        }

        private void ExecuteFilterLastUsed()
        {
            IsBusy = true;

            PageTitle = "Last Checkout Lockers";
            LockersFiltered.ReplaceRange(Lockers.FilterLastUsed());
            if (!LockersFiltered.Any())
            {
                NoLockersFoundMessage = "No Lockers Found";
                LockersFound = false;
                NoLockersFound = true;
            }
            else
            {
                LockersFound = true;
                NoLockersFound = false;

                LockerFilterSettingDb.DeleteFilterLockers();
                LockerFilterSettingDb.InsertFilterLocker(new Utils.LockerFilterSettingBO() { IsLastUsed = true });
            }
            IsBusy = false;
        }

        private async Task ExecuteFilterFavoritesAsync()
        {
            IsBusy = true;
            LockersFound = false;
            NoLockersFound = false;

            // Set title
            PageTitle = $"Favorite Lockers";
            LockersFiltered.ReplaceRange(await Lockers.FilterFavorites());
            if (!LockersFiltered.Any())
            {
                NoLockersFoundMessage = "No Favorite Lockers Found";
                LockersFound = false;
                NoLockersFound = true;
            }
            else
            {
                LockersFound = true;
                NoLockersFound = false;

                LockerFilterSettingDb.DeleteFilterLockers();
                LockerFilterSettingDb.InsertFilterLocker(new Utils.LockerFilterSettingBO() { IsFavourite = true });
            }
            IsBusy = false;
        }

        private void ExecuteFilterInActive()
        {
            IsBusy = true;
            LockersFound = false;
            NoLockersFound = false;

            // Set title
            PageTitle = $"InActive Lockers";

            LockersFiltered.ReplaceRange(Lockers.FilterInActive());
            if (!LockersFiltered.Any())
            {
                NoLockersFoundMessage = "No InActive Lockers Found";
                LockersFound = false;
                NoLockersFound = true;
            }
            else
            {
                LockersFound = true;
                NoLockersFound = false;

                LockerFilterSettingDb.DeleteFilterLockers();
                LockerFilterSettingDb.InsertFilterLocker(new Utils.LockerFilterSettingBO() { IsInActive = true });
            }

            IsBusy = false;
        }

        private void AddLocker_OnClick()
        {
            (navService as ISealegsNavigationService).PopupNavigateTo(ViewModelLocator.AddEditLocker, LockerTypes);
        }

        #endregion
    }
}