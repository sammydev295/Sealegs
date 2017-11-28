using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using FormsToolkit;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using Sealegs.Clients.Portable.Locator;
using Xamarin.Forms;

using Sealegs.DataObjects;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class CheckOutFinalViewModel : BaseViewModel
    {
        #region Observabale Collections

        public ObservableRangeCollection<Wine> SelectedWines { get; } = new ObservableRangeCollection<Wine>();

        public bool IsWaiting { get; set; }
        #endregion

        #region CTOR

        private INavigationService _navService;
        public CheckOutFinalViewModel(INavigationService navigation)
        {
            _navService = navigation;
            //SelectedWines.ReplaceRange(selectedWines);
        }

        #endregion

        #region Relay Command
        public RelayCommand<Wine> AddQuantityCommand => new RelayCommand<Wine>(Add_OnClick);

        public RelayCommand<Wine> RemoveQuantityCommand =>new RelayCommand<Wine>(Remove_OnClick);

        public RelayCommand CheckOutCommand=>new RelayCommand(CheckOut_OnClick);
        #endregion

        #region Events
        private  void Add_OnClick(Wine wine)
        {
            if (IsWaiting)return;

            IsWaiting = true;

            if (wine.CheckoutQuantity < wine.Quantity)
            {
                int selectedIndex = SelectedWines.Select((w, index) => new { w, index }).First(w => w.w.Id == wine.Id).index;
                Wine selectedWine = SelectedWines[selectedIndex];
                ++selectedWine.CheckoutQuantity;
                SelectedWines.RemoveAt(selectedIndex);
                SelectedWines.Insert(selectedIndex, selectedWine);
            }
            else
            {
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message,
                       new MessagingServiceAlert
                       {
                           Message = $"Quantity available to checkout is {wine.Quantity}",
                           Cancel = "OK"
                       });
            }

            IsWaiting = false;

        }
        private void Remove_OnClick(Wine wine)
        {
            if (IsWaiting) return;

            IsWaiting = true;

            if (wine.CheckoutQuantity != 0)
            {
                int selectedIndex = SelectedWines.Select((w, index) => new { w, index }).First(w => w.w.Id == wine.Id).index;
                Wine tempWine = SelectedWines[selectedIndex];
                --tempWine.CheckoutQuantity;
                SelectedWines.RemoveAt(selectedIndex);
                SelectedWines.Insert(selectedIndex, tempWine);
            }

            IsWaiting = false;
        }

        private void CheckOut_OnClick()
        {
            _navService.NavigateTo(ViewModelLocator.Signature,SelectedWines);
        }
        #endregion
    }
}
