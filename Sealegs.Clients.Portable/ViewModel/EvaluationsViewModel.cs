using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using FormsToolkit;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using Xamarin.Forms;

using Sealegs.DataObjects;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class EvaluationsViewModel: BaseViewModel
    {
        #region CTOR

        private INavigationService navService;
        public EvaluationsViewModel (INavigationService navigation)
        {
            navService = navigation;
            NextForceRefresh = DateTime.UtcNow.AddMinutes (45);
        }

        #endregion 

        bool sync = true;

        bool isWineCell;
        public bool IsWineCell
        {
            get { return isWineCell; }
            set
            {
                isWineCell = value;
                RaisePropertyChanged();
            }
        }

        public static bool ForceRefresh { get; set; }

        public ObservableRangeCollection<LockerMember> Lockers { get; } = new ObservableRangeCollection<LockerMember> ();
        public DateTime NextForceRefresh { get; set; }


        bool noLockersFound;
        public bool NoLockersFound {
            get { return noLockersFound; }
            set
            {
                noLockersFound = value;
                RaisePropertyChanged();
            }
        }

        string noLockersFoundMessage;
        public string NoLockersFoundMessage {
            get { return noLockersFoundMessage; }
            set
            {
                noLockersFoundMessage = value;
                RaisePropertyChanged();
            }
        }

        ICommand loadLockersCommand;
        public ICommand LoadLockersCommand =>
            loadLockersCommand ?? (loadLockersCommand = new Command (async () => await ExecuteLoadLockersAsync ()));

        async Task<bool> ExecuteLoadLockersAsync ()
        {
            if (IsBusy)
                return false;

            try 
            {
                NextForceRefresh = DateTime.UtcNow.AddMinutes (45);
                IsBusy = true;
                NoLockersFound = false;

#if DEBUG
                await Task.Delay(1000);

#endif

                if (!Settings.IsLoggedIn) 
                {
                    NoLockersFoundMessage = "Please sign in\nto leave feedback";
                    NoLockersFound = true;
                    return true;
                }

                var lockers = (await StoreManager.LockerStore.GetItemsAsync ()).ToList();
                var feedback = (await StoreManager.FeedbackStore.GetItemsAsync (sync)).ToList();

                sync = false;

                var finalLockers = new List<LockerMember> ();
                foreach (var locker in lockers) 
                {
                    if (!(await StoreManager.FavoriteStore.IsFavorite (User.Id, locker.Id, Favorite.Locker)))
                        continue;
                    
                    //if TBA
                  //  if (!locker.StartTime.HasValue)
                  //      continue;
#if !DEBUG

                    //if it hasn't started yet
                   // if (DateTime.UtcNow < locker.StartTime.Value)
                        continue;
#endif
                    if (feedback.Any (f => f.FeedbackEntityId == locker.Id))
                        continue;

                 //   finalLockers.Add (locker);
                }

                Lockers.ReplaceRange (finalLockers);

                if (Lockers.Count == 0) 
                {
                    NoLockersFoundMessage = "No Pending\nEvaluations Found";
                    NoLockersFound = true;
                } 
                else 
                {
                    NoLockersFound = false;
                }
            } 
            catch (Exception ex) 
            {
                Logger.Report (ex, "Method", "ExecuteLoadLockersAsync");
                MessagingService.Current.SendMessage (MessageKeys.Error, ex);
            } 
            finally 
            {
                IsBusy = false;
            }

            return true;
        }
    }
}

