using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using FormsToolkit;
using MoreLinq;
using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;

using Sealegs.Clients.Portable.NavigationService;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class RatingBarPopUpViewModel : BaseViewModel
    {
        #region Fields

        private INavigationService _navService;

        #endregion

        #region CTOR

        public RatingBarPopUpViewModel(INavigationService navigation)
        {
            _navService = navigation;
        }

        #endregion

        #region Initialize

        public void Initialize()
        {
            RatingCountList = new ObservableRangeCollection<RatingBarBO>();
            Enumerable.Range(1, 5).ToList().ForEach(i => RatingCountList.Add(new RatingBarBO() { Index = i, StarImage = "star_outline.png" }));
        }

        #endregion 

        #region Observable Properties

        public Wine SelectedWine { get; set; }

        private ObservableRangeCollection<RatingBarBO> _ratingCountList;
        public ObservableRangeCollection<RatingBarBO> RatingCountList
        {
            get => _ratingCountList;
            set
            {
                _ratingCountList = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Relay Commands

        public RelayCommand CancelCommand => new RelayCommand(Cancel_OnClick);

        #endregion

        #region Event Handlers & Helpers

        #region Cancel_OnClick

        private void Cancel_OnClick()
        {
            (_navService as ISealegsNavigationService).PopupGoBack();
        }

        #endregion

        #region ItemSelected

        public void ItemSelected(RatingBarBO item)
        {
            RatingCountList = new ObservableRangeCollection<RatingBarBO>();
            Enumerable.Range(1, item.Index).ToList().ForEach(i => RatingCountList.Add(new RatingBarBO() { Index = i, StarImage = "star_selected.png"}));
            Enumerable.Range(RatingCountList.Count + 1, 5 - RatingCountList.Count).ToList().ForEach(i => RatingCountList.Add(new RatingBarBO() { Index = i, StarImage = "star_outline.png" }));

            SendFeedback(item.Index);
        }

        #endregion

        #region SendFeedback

        private async void SendFeedback(int rating)
        {
            bool result = false;
            if (IsBusy) return;

            Toast.SendToast($"Saving feedback for wine {SelectedWine.WineTitle} ...");
            IsBusy = true;
           
            try
            {
                var list = await FeedbackDb.GetAllByWine(SelectedWine.Id.ToString());
                bool containWine = (list != null) && list.Select(s => s.WineId).Contains(SelectedWine.Id);
                if (containWine)
                {
                    Feedback feedback = list.FirstOrDefault(s => s.WineId == SelectedWine.Id);
                    feedback.Rating = rating;
                    result = await FeedbackDb.Update(feedback);
                }
                else
                {
                    result = await FeedbackDb.Insert(
                    new Feedback
                    {
                        WineId = SelectedWine.Id,
                        Rating = rating,
                        UserId = SelectedWine.LockerID,
                        FeedbackEntityId = ""
                    });
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "SendFeedback");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                if (!result)
                    await UserDialogs.Instance.AlertAsync($"Sorry Rating update failed for wine {SelectedWine.WineTitle}");
                else
                    await UserDialogs.Instance.AlertAsync($"Rating updated for wine {SelectedWine.WineTitle}");

                Cancel_OnClick();
                IsBusy = false;
            }
        }

        #endregion

        #endregion
    }

    public class RatingBarBO
    {
        public string StarImage { get; set; }
        public int Index { get; set; }
    }
}
