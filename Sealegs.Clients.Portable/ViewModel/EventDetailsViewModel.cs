using FormsToolkit;
using MvvmHelpers;

using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;

using System;
using System.Threading.Tasks;

using Sealegs.DataObjects;

using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class EventDetailsViewModel : BaseViewModel
    {
        #region Global Variables
        private INavigationService navService;
        #endregion

        #region CTOR
        public EventDetailsViewModel(INavigationService navigation)
        {
            navService = navigation;
            //Event = e;
            //News = new ObservableRangeCollection<News>();
            //if (e.News != null)
            //    News.Add(e.News);
        }
        #endregion

        #region Observable Properties

        #region Event 
        private FeaturedEvent _event;
        public FeaturedEvent Event
        {
            get { return _event; }
            set
            {
                _event = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region News
        private ObservableRangeCollection<News> _news = new ObservableRangeCollection<News>();
        public ObservableRangeCollection<News> News
        {
            get { return _news; }
            set
            {
                _news = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsRemindeerSet
        private bool _isReminderSet;
        public bool IsReminderSet
        {
            get { return _isReminderSet; }
            set
            {
                _isReminderSet = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Selected News
        private News _selectedNews;
        public News SelectedNews
        {
            get { return _selectedNews; }
            set
            {
                _selectedNews = value;
                RaisePropertyChanged();
                if (_selectedNews == null)
                    return;
               // MessagingService.Current.SendMessage(MessageKeys.NavigateToNews, _selectedNews);
                SelectedNews = null;
            }
        }
        #endregion

        #region Title
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Start Time

        private DateTime? _startTime;
        public DateTime? StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
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

        #region LocationName

        private string _locationName;
        public string LocationName
        {
            get { return _locationName; }
            set
            {
                _locationName = value;
                RaisePropertyChanged();
            }
        }

        #endregion 

        #endregion

        #region Relay Commands
        public RelayCommand LoadEventDetailsCommand => new RelayCommand(async () => await ExecuteLoadEventDetailsCommandAsync());
        public RelayCommand ReminderCommand => new RelayCommand(async () => await ExecuteReminderCommandAsync());
        #endregion

        #region Events
        async Task ExecuteLoadEventDetailsCommandAsync()
        {

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                IsReminderSet = await ReminderService.HasReminderAsync("event_" + Event.Id);
            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadEventDetailsCommandAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task ExecuteReminderCommandAsync()
        {
            if (!IsReminderSet)
            {
                if (Event.EndTime != null)
                {
                    if (Event.StartTime != null)
                    {
                        var result = await ReminderService.AddReminderAsync("event_" + Event.Id,
                            new Plugin.Calendars.Abstractions.CalendarEvent
                            {
                                Description = Event.Description,
                                Location = Event.LocationName,
                                AllDay = Event.IsAllDay,
                                Name = Event.Title,
                                Start = Event.StartTime.Value,
                                End = Event.EndTime.Value
                            });
                        if (!result)
                            return;
                    }
                }
                Logger.Track(SealegsLoggerKeys.ReminderAdded, "Title", Event.Title);
                IsReminderSet = true;
            }
            else
            {
                var result = await ReminderService.RemoveReminderAsync("event_" + Event.Id);
                if (!result)
                    return;
                Logger.Track(SealegsLoggerKeys.ReminderRemoved, "Title", Event.Title);
                IsReminderSet = false;
            }

        }
        public void Initlize(FeaturedEvent featuredEvent)
        {
            Event = featuredEvent;
            Description = Event.Description;
            Title = Event.Title;
            LocationName = Event.LocationName;
            StartTime = Event.StartTime;
        }
        #endregion
    }
}


