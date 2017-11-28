using FormsToolkit;
using Acr.UserDialogs;

using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable.Locator;

using System;
using System.Linq;
using System.Collections.ObjectModel;

using MvvmHelpers;
using Xamarin.Forms;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class EventManagementViewModel:BaseViewModel
    {
        #region GLobal Variables
        private readonly INavigationService _navService;
        #endregion

        #region CTOR
        public EventManagementViewModel(INavigationService navigation)
        {
            _navService = navigation;
        }
        #endregion

        #region Observable Properties

      

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
        public RelayCommand PassedCommand => new RelayCommand(Passed_OnClick);
        public RelayCommand UpcomingCommand => new RelayCommand(Upcoming_OnClick);
        public RelayCommand AddCommand => new RelayCommand(Add);
        #endregion

        #region Events

        private void Add()
        {
            _navService.NavigateTo(ViewModelLocator.AddEditEventPage);
        }
        private void Passed_OnClick()
        {

            UpcomingTextColor = Color.FromHex("#F5F5F5");
            UpcomingBackgroundColor = Color.FromHex("#7635EB");
            PassedTextColor = Color.FromHex("#7635EB");
            PassedBackgroundColor = Color.FromHex("#F5F5F5");
            var sorted = AllEvents.Where(o => o.StartTime < DateTime.Today).OrderBy(o => o.StartTime);

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
        public  void Intilize(ObservableCollection<FeaturedEvent> events)
        {

            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                TopIsVisible = false;

                AllEvents = events;
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

        public async void EditOrDelete(FeaturedEvent events)
        {

            var result = await UserDialogs.Instance.ActionSheetAsync("Select", "Cancel", null, null, "Edit", "Delete");
            if (result == "Edit")
            {
                _navService.NavigateTo(ViewModelLocator.AddEditEventPage, events);
                return;
            }
            if (result == "Delete")
            {
                await FeaturedEventsDb.DeleteFeaturedEvent(events.Id);
            }
        }
       
        void SortEvents()
        {
            EventsGrouped.ReplaceRange(Events.GroupByDate());
        }
        #endregion
    }
}
