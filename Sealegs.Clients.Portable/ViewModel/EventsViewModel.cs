using MvvmHelpers;
using FormsToolkit;

using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable.Locator;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoreLinq;
using Xamarin.Forms;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class EventsViewModel : BaseViewModel
    {
        #region GLobal Variables
        private readonly INavigationService _navService;
        #endregion

        #region CTOR
        public EventsViewModel(INavigationService navigation)
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

        private ObservableCollection<FeaturedEvent> _events = new ObservableCollection<FeaturedEvent>();
        public ObservableCollection<FeaturedEvent> Events
        {
            get => _events;
            set
            {
                _events = value;
                RaisePropertyChanged();
            }
        }
        private ObservableRangeCollection<Grouping<string, FeaturedEvent>> _eventsGrouped = new ObservableRangeCollection<Grouping<string, FeaturedEvent>>();
        public ObservableRangeCollection<Grouping<string, FeaturedEvent>> EventsGrouped
        {
            get => _eventsGrouped;
            set
            {
                _eventsGrouped = value;
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

        #region SelectedEvent

        private FeaturedEvent _selectedEvent;
        public FeaturedEvent SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                RaisePropertyChanged();
                if (_selectedEvent == null)
                    return;
                // MessagingService.Current.SendMessage(MessageKeys.NavigateToEvent, selectedEvent);
                SelectedEvent = null;
            }
        }

        #endregion

        #region All Events
        public ObservableCollection<FeaturedEvent> AllEvents { get; set; }
        #endregion

        #region Reminder 
        public bool ShowReminder { get; set; }

        bool _isReminderSet;
        public bool IsReminderSet
        {
            get => _isReminderSet;
            set
            {
                _isReminderSet = value;
                RaisePropertyChanged();
            }
        }
        #endregion 

        #region Relay Commands
        public RelayCommand ForceRefreshCommand => new RelayCommand(ForceRefresh);
        public RelayCommand LoadEventsCommand => new RelayCommand(LoadEvents);
        public RelayCommand<FeaturedEvent> EventsDetailNavigateCommand => new RelayCommand<FeaturedEvent>(EventsDetailNavigate);
        public RelayCommand PassedCommand=>new RelayCommand(Passed_OnClick);
        public RelayCommand UpcomingCommand => new RelayCommand(Upcoming_OnClick);
        public RelayCommand ManageEventCommand => new RelayCommand(ManageEvents_OnClick);

        #endregion

        #region Events
        private void Passed_OnClick()
        {
          
            UpcomingTextColor = Color.FromHex("#F5F5F5");
            UpcomingBackgroundColor = Color.FromHex("#7635EB");
            PassedTextColor = Color.FromHex("#7635EB");
            PassedBackgroundColor = Color.FromHex("#F5F5F5");
            var sorted = AllEvents.Where(o => o.StartTime < DateTime.Today).OrderBy(o=>o.StartTime);

            Events = new ObservableRangeCollection<FeaturedEvent>(sorted);
            SortEvents();
        }
        private void Upcoming_OnClick()
        {
            PassedTextColor = Color.FromHex("#F5F5F5");
            PassedBackgroundColor = Color.FromHex("#7635EB");
            UpcomingTextColor = Color.FromHex("#7635EB");
            UpcomingBackgroundColor = Color.FromHex("#F5F5F5");
            var sorted = AllEvents.Where(o => o.StartTime > DateTime.Today).OrderBy(o => o.StartTime);

            Events = new ObservableRangeCollection<FeaturedEvent>(sorted);
            SortEvents();
        }
        private async void ForceRefresh()
        {
            if (IsListBusy)
                return;
            try
            {
                IsListBusy = true;

                var events = await StoreManager.EventStore.GetItemsAsync(true);
                AllEvents = new ObservableCollection<FeaturedEvent>(events);
                Events = new ObservableCollection<FeaturedEvent>(AllEvents);
                Upcoming_OnClick();

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
        private async void LoadEvents()
        {

            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                TopIsVisible = false;
              
                var events = await FeaturedEventsDb.GetAll();

                AllEvents =new ObservableCollection<FeaturedEvent>(events);
                Events = new ObservableCollection<FeaturedEvent>(AllEvents);
                Upcoming_OnClick();

            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadEventsAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
                TopIsVisible = true;
            }
        }
        private void EventsDetailNavigate(FeaturedEvent featuredEvent)
        {
            _navService.NavigateTo(ViewModelLocator.EventDetails, featuredEvent);
        }
        void SortEvents()
        {
            EventsGrouped.ReplaceRange(Events.GroupByDate());
        }
        private void ManageEvents_OnClick()
        {
             if (User != null && User.Role.RoleName == "Admin"&& !IsBusy)
            {
                _navService.NavigateTo(ViewModelLocator.EventsManagment, AllEvents);
            }
        }
        #endregion

        #region Commented
        //public ObservableRangeCollection<FeaturedEvent> Events { get; } = new ObservableRangeCollection<FeaturedEvent>();
        //public ObservableRangeCollection<Grouping<string, FeaturedEvent>> EventsGrouped { get; } = new ObservableRangeCollection<Grouping<string, FeaturedEvent>>();

        //        ICommand forceRefreshCommand;
        //        public ICommand ForceRefreshCommand =>
        //        forceRefreshCommand ?? (forceRefreshCommand = new Command(async () => await ExecuteForceRefreshCommandAsync()));

        //        async Task ExecuteForceRefreshCommandAsync()
        //        {
        //            await ExecuteLoadEventsAsync(true);
        //        }

        //        ICommand loadEventsCommand;
        //        public ICommand LoadEventsCommand =>
        //            loadEventsCommand ?? (loadEventsCommand = new Command<bool>(async (f) => await ExecuteLoadEventsAsync()));
        //        async Task<bool> ExecuteLoadEventsAsync(bool force = false)
        //        {
        //            if (IsBusy)
        //                return false;

        //            try
        //            {
        //                IsBusy = true;

        //#if DEBUG
        //                await Task.Delay(1000);
        //#endif

        //                var eventss = await StoreManager.EventStore.GetItemsAsync(true);

        //              //  Events.ReplaceRange(await StoreManager.EventStore.GetItemsAsync(force));

        //                //   Title = "Events (" + Events.Count(e => e.StartTime.HasValue && e.StartTime.Value > DateTime.UtcNow) + ")";

        //                SortEvents();

        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.Report(ex, "Method", "ExecuteLoadEventsAsync");
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

