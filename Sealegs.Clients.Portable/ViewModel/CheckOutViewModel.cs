using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using FormsToolkit;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoreLinq;
using MvvmHelpers;
using Plugin.Share;
using Sealegs.Clients.Portable.Locator;
using Xamarin.Forms;

using Sealegs.DataObjects;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class CheckOutViewModel : BaseViewModel
    {
        #region CTOR

        private readonly INavigationService _navService;
        public CheckOutViewModel(INavigationService navigation)
        {
            _navService = navigation;
            IsListVisible = true;
        }

        #endregion

        #region Intialize

        public async void Intialize()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                var wines = await WinesDb.GetAllWinesById(LockerMember.Id);
                if (!wines.Any())
                {
                    NoWinesFound = true;
                }
                else
                {
                    Wines.ReplaceRange(wines.Where(w => w.CheckedOutDate == null && (w.Quantity > 0)));
                    NoWinesFound = false;
                }
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

        #region Properties

        public LockerMember LockerMember { get; set; }

        #region Observable Collections

        /// <summary>
        /// All wines loaded from backend
        /// </summary>
        public ObservableRangeCollection<Wine> Wines { get; } = new ObservableRangeCollection<Wine>();

        /// <summary>
        /// Wines currently in Cart
        /// </summary>
        public ObservableRangeCollection<Wine> WineCartItems = new ObservableRangeCollection<Wine>();

        #endregion

        #region SelectedWine

        Wine _selectedWine;
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

        #region NoWinesFound

        bool noWinesFound;
        public bool NoWinesFound
        {
            get { return noWinesFound; }
            set
            {
                noWinesFound = value;
                RaisePropertyChanged();
            }
        }

        string noWinesFoundMessage;
        public string NoWinesFoundMessage
        {
            get { return noWinesFoundMessage; }
            set
            {
                noWinesFoundMessage = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region List / Tile

        bool _isListVisible = true;
        public bool IsListVisible
        {
            get => _isListVisible;
            set
            {
                _isListVisible = value;
                RaisePropertyChanged();
            }
        }

        bool _isTileVisible = false;
        public bool IsTileVisible
        {
            get => _isTileVisible;
            set
            {
                _isTileVisible = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Relay Commands
        public RelayCommand ListCommand => new RelayCommand(List_OnClick);
        public RelayCommand TileCommand => new RelayCommand(Tile_OnClick);

        public RelayCommand CheckOutCommand=>new RelayCommand(CheckOut_OnClick);
        #endregion

        #region Event Handlers & Helpers

        private void CheckOut_OnClick()
        {
            WineCartItems.ReplaceRange(Wines.Where(s => s.IsChecked.Value));
            _navService.NavigateTo(ViewModelLocator.CheckOutFinal, WineCartItems);
        }

        private void List_OnClick()
        {
            IsListVisible = true;
            IsTileVisible = false;
        }

        private void Tile_OnClick()
        {
            IsListVisible = false;
            IsTileVisible = true;
        }

        #endregion
    }
}
