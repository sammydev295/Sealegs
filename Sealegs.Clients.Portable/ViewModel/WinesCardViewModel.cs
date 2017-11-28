using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using FormsToolkit;
using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoreLinq;
using MvvmHelpers;
using Plugin.Share;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable.Locator;
using Sealegs.Clients.Portable.NavigationService;
using Sealegs.Clients.UI;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class WinesCardVieModel : BaseViewModel
    {
        #region Fields

        private INavigationService _navService;

        #endregion

        #region CTOR

        public WinesCardVieModel(INavigationService navigation)
        {
            _navService = navigation;
        }

        #endregion

        #region Initialize (2 overloads)

        public async void Initialize()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                NoWinesFound = false;

                Locker = await LockerMemberDb.GetByMemberId(User.UserID.ToString());
                LoadWinesCommand.Execute(null);
            }
            catch (Exception ex)
            {
                NoWinesFound = true;
                Logger.Report(ex, "Method", "Initialize");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void Initialize(LockerMember locker)
        {
            Locker = locker;
            LoadWinesCommand.Execute(null);
        }

        #endregion

        #region Properties

        #region WinesCards

        private List<CardStackViewItem> _wineCards = new List<CardStackViewItem>();
        public List<CardStackViewItem> WineCards
        {
            get => _wineCards;
            set
            {
                _wineCards = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Wines

        public ObservableRangeCollection<Wine> Wines { get; set; } = new ObservableRangeCollection<Wine>();

        #endregion

        #region SelectedCardItem

        private CardStackViewItem _selectedCardItem;
        public CardStackViewItem SelectedCardItem
        {
            get => _selectedCardItem;
            set
            {
                _selectedCardItem = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region NoWinesFound

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

        private string _noWinesFoundMessage;
        public string NoWinesFoundMessage
        {
            get => _noWinesFoundMessage;
            set
            {
                _noWinesFoundMessage = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region LockerMember

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

        #endregion

        #region Relay Commands

        public RelayCommand LoadWinesCommand => new RelayCommand(LoadWines);
        public RelayCommand ShareCommand => new RelayCommand(ShareWine);
        public RelayCommand RateWineCommand => new RelayCommand(RateWine);
        public RelayCommand WineDetailsCommand => new RelayCommand(ViewWine);

        #endregion

        #region Event Handlers & Helpers

        #region LoadWines

        private async void LoadWines()
        {
            if (IsBusy)
                return;
            try
            {
                PageTitle = "Wines";
                IsBusy = true;

                var wines = await WinesDb.GetAllWinesById(Locker.Id);
                if (!wines.Any())
                {
                    NoWinesFound = true;
                    return;
                }
                
                wines.Where(w => String.IsNullOrEmpty(w.ImagePath)).ToList().ForEach(w => w.ImagePath = "wine_placeholder.png");
                wines.Where(w => String.IsNullOrEmpty(w.CheckedOutMemberSignature)).ToList().ForEach(w => w.CheckedOutMemberSignature = "no_cover.jpg");
                wines.Where(w => String.IsNullOrEmpty(w.CheckedOutEmployeeSignature)).ToList().ForEach(w => w.CheckedOutEmployeeSignature = "no_cover.jpg");
                var random = new Random();
                wines.Where(w => w.Quantity == 0).ToList().ForEach(w =>
                {
                    Task.Delay(20); // for randow number generator to work properly since they are based on system clock
                    w.Quantity = random.Next(1, 30);
                    w.BottleSize = w.BottleSize?.ToLower() ?? "750 ml";
                    w.Vintage = w.Vintage ?? "unknown";
                    w.WineVarietalId = w.WineVarietalId ?? Guid.Empty.ToString();
                });

                Wines = new ObservableRangeCollection<Wine>(wines);
                WineCards = Wines.Select(w =>
                    new CardStackViewItem()
                    {
                        Id = w.Id,
                        Name = $"{w?.WineTitle} - {w?.Vintage}",
                        Description = String.IsNullOrEmpty(w?.Notes) ? "Description goes here" : w.Notes,
                        Quantity = w?.Quantity.ToString(),
                        ImagePath = w?.ImagePath
                    }).ToList();

                SelectedCardItem = WineCards.FirstOrDefault();
                NoWinesFound = false;
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "LoadWines");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion 

        #region ShareWine

        private async void ShareWine()
        {
            try
            {
                var wine = Wines.First(w => w.Id == SelectedCardItem.Id);
                Logger.Track(SealegsLoggerKeys.Share, "Title", wine.WineTitle);

                await CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage()
                {
                    Text = $"Can't wait for some more {wine.WineTitle} {wine.Vintage} {wine.WineVarietal} at Sealegs!",
                    Title = "Share"
                });
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "ShareWine");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
        }

        #endregion 

        #region RateWine

        private void RateWine()
        {
            try
            {
                var wine = Wines.First(w => w.Id == SelectedCardItem.Id);
                (_navService as ISealegsNavigationService).PopupNavigateTo(ViewModelLocator.RatingBarPopUp, wine);
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "RateWine");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
        }

        #endregion

        #region ViewWine

        private void ViewWine()
        {
            try
            {
                var wine = Wines.First(w => w.Id == SelectedCardItem.Id);
                (_navService as ISealegsNavigationService).PopupNavigateTo(ViewModelLocator.WineDetail, wine);
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "RateWine");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
        }

        #endregion

        #endregion
    }
}
