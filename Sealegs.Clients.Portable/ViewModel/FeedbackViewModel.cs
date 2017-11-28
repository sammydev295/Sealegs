using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using FormsToolkit;
using GalaSoft.MvvmLight.Views;
using Sealegs.Clients.Portable.ViewModel;
using Xamarin.Forms;

using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable
{
    // ADD COMMENT TEST OF CODELENS

    public class FeedbackViewModel : BaseViewModel
    {
        #region FeaturedEvent

        FeaturedEvent featuredEvent;
        public FeaturedEvent FeaturedEvent
        {
            get { return featuredEvent; }
            set
            {
                featuredEvent = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Locker

        LockerMember locker;
        public LockerMember Locker
        {
            get { return locker; }
            set
            {
                locker = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Wine

        Wine wine;
        public Wine Wine
        {
            get { return wine; }
            set
            {
                wine = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        String Id { get; set; }

        #region CTOR

        private INavigationService navService;
        public FeedbackViewModel(INavigationService navigation)
        {
            navService = navigation;
               FeaturedEvent = featuredEvent;
           // Title = featuredEvent.Title;
            Id = featuredEvent.Id;
        }

        //public FeedbackViewModel(INavigation navigation, LockerMember locker) : base(navigation)
        //{
        //    Locker = locker;
        //   // Title = Locker.DisplayName;
        //    Id = Locker.Id;
        //}

        //public FeedbackViewModel(INavigation navigation, Wine wine) : base(navigation)
        //{
        //    Wine = wine;
        //   // Title = wine.WineTitle;
        //    Id = Wine.Id;
        //}

        #endregion

        #region SubmitRatingCommand 

        ICommand submitRatingCommand;
        public ICommand SubmitRatingCommand =>
            submitRatingCommand ?? (submitRatingCommand = new Command<int>(async (rating) => await ExecuteSubmitRatingCommandAsync(rating))); 

        async Task ExecuteSubmitRatingCommandAsync(int rating)
        {
            if(IsBusy)
                return;

            IsBusy = true;
            try
            {
                if(rating == 0)
                {
                    MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                        {
                            Title = "No Rating Selected",
                            Message = "Please select a rating to leave feedback for this locker.",
                            Cancel = "OK"
                        });
                        return;
                }

                EvaluationsViewModel.ForceRefresh = true;
                Logger.Track(SealegsLoggerKeys.LeaveFeedback, "Title", rating.ToString());
                
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                {
                    Title = "Feedback Received",
                    Message = "Thanks for the feedback, have a great day",
                    Cancel = "OK",
                    OnCompleted = async () => 
                    {
                       // await Navigation.PopModalAsync ();
                        if (Device.OS == TargetPlatform.Android)
                            MessagingService.Current.SendMessage ("eval_finished");
                    }
                });

                await StoreManager.FeedbackStore.InsertAsync(new Feedback
                {
                    FeedbackEntityId = Id,
                    Rating = rating
                });
            }
            catch(Exception ex)
            {
                Logger.Report(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}

