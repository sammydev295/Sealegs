using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using MoreLinq;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using FormsToolkit;
using Acr.UserDialogs;
using MvvmHelpers;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Plugin.Share;

using Sealegs.Clients.Portable.Interfaces;
using Sealegs.Clients.Portable.Locator;
using Sealegs.Clients.Portable.NavigationService;
using Sealegs.DataObjects;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class WineCarouselViewModel : BaseViewModel
    {
        #region Fields

        public INavigationService _navService;

        #endregion

        #region CTOR

        public WineCarouselViewModel(INavigationService navigation)
        {
            _navService = navigation;
        }

        #endregion

        #region Properties

        #region WinesCarousel

        private ObservableCollection<Wine> _winesCarousel = new ObservableCollection<Wine>();
        public ObservableCollection<Wine> WinesCarousel
        {
            get => _winesCarousel;
            set
            {
                _winesCarousel = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Wines

        public ObservableRangeCollection<Wine> Wines { get; set; } = new ObservableRangeCollection<Wine>();

        #endregion

        #region SelectedCarouselItem

        private Wine _selectedCarouselItem;
        public Wine SelectedCarouselItem
        {
            get => _selectedCarouselItem;
            set
            {
                _selectedCarouselItem = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region WineVarietals 

        public IList<WineVarietal> WineVarietals { get; set; }

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

        #region Locker Member

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
        public RelayCommand FilterNotCheckedOutCommand => new RelayCommand(FilterNotCheckedOut);
        public RelayCommand FilterCheckedOutCommand => new RelayCommand(FilterCheckedOut);
        public RelayCommand EditWineCommand => new RelayCommand(EditWine);
        public RelayCommand DeleteWineCommand => new RelayCommand(DeleteWine);

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

                wines.Where(w => String.IsNullOrEmpty(w.ImagePath)).ToList().ForEach(w => w.ImagePath = Wine.DefaultBottleImage);
                var random = new Random();
                wines.Where(w => w.Quantity == 0).ToList().ForEach(w =>
                {
                    Task.Delay(100); // for randow number generator to work properly since they are based on system clock
                    w.Quantity = random.Next(1, 30);
                    w.BottleSize = w.BottleSize?.ToLower() ?? "750 ml";
                    w.Vintage = w.Vintage ?? "unknown";
                    w.WineVarietalId = w.WineVarietalId ?? Guid.Empty.ToString();
                });
                Wines = new ObservableRangeCollection<Wine>(wines);
                WinesCarousel = Wines;

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

        #region FilterNotCheckedOut

        private void FilterNotCheckedOut()
        {
            PageTitle = $"Availiable for checkout";

            var list = Wines.Where(s => s.CheckedOutDate != null);
            WinesCarousel = new ObservableRangeCollection<Wine>(list);
        }

        #endregion

        #region FilterCheckedOut

        private void FilterCheckedOut()
        {
            PageTitle = $"Checked out Wine";

            var list = Wines.Where(s => s.CheckedOutDate != null);
            WinesCarousel = new ObservableRangeCollection<Wine>(list);
        }

        #endregion

        #region EditWine

        public void EditWine()
        {
            Wine wine = SelectedCarouselItem;
            (_navService as ISealegsNavigationService).PopupNavigateTo(ViewModelLocator.AddWine, wine, WineVarietals);
        }

        #endregion

        #region DeleteWine

        public async void DeleteWine()
        {
            try
            {
                IsBusy = true;
                var wine = SelectedCarouselItem;
                var confirm = await UserDialogs.Instance.ConfirmAsync($"Are you sure you want to delete wine {wine.WineTitle} {wine.Vintage}", "DELETE WINE?", "Yes", "No");
                if (!confirm)
                    return;

                Toast.SendToast($"Deleting wine {wine.WineTitle}...");
                var deleted = await WinesDb.DeleteWine(wine.Id);
                if (deleted)
                    UserDialogs.Instance.ShowSuccess($"Deleted wine {wine.WineTitle} {wine.Vintage}", 3000);
                else
                    await UserDialogs.Instance.AlertAsync($"Unable to delete wine {wine.WineTitle} {wine.Vintage}", "Please contact support!", "OK");

                if (deleted)
                    _navService.GoBack();
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "DeleteWine");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region ItemSelected

        public async void ItemSelected(Wine wine)
        {
            var result = await UserDialogs.Instance.ActionSheetAsync("Select", "Cancel", null, null, "Edit", "Delete");
        }

        #endregion

        #endregion
    }
}
