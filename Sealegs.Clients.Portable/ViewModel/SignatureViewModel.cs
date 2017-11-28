using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using FormsToolkit;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;
using MoreLinq;
    
using MvvmHelpers;
using Sealegs.Clients.Portable.Locator;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class SignatureViewModel : BaseViewModel
    {
        #region Properties

        private INavigation Nav;

        public LockerMember Locker { get; set; }

        #endregion

        #region Observabale Collections

        public ObservableRangeCollection<Wine> SelectedWines { get; } = new ObservableRangeCollection<Wine>();

        #endregion

        #region CTOR

        private INavigationService _navService;
        public SignatureViewModel(INavigationService navigation)
        {
            _navService = navigation;
      
        }

        #endregion

        public Wine Wine { get; set; }
        public Stream ManagerSignature { get; set; }

        public Stream ClientSignature { get; set; }

        #region UpdateWineCheckoutCommand

        public RelayCommand UpdateWineCheckoutCommand=>new RelayCommand(UpdateWineCheckout);

        private async void UpdateWineCheckout()
        {
            if(IsBusy)
                return;

            try
            {
                IsBusy = true;

                var storage = DependencyService.Get<IAzureBlobStorage>();

                Toast.SendToast("Saving signatures...");
                var managerSignature = await storage.UploadFileAsync(Addresses.AzureStorageBlobContainer,Addresses.SignatureStorage, ManagerSignature);
                if (!managerSignature.Item2)
                {
                    DependencyService.Get<ILogger>().Report(managerSignature.Item1);
                    MessagingService.Current.SendMessage(MessageKeys.Error, managerSignature.Item1);
                    return;
                }
                var clientSignature = await storage.UploadFileAsync(Addresses.AzureStorageBlobContainer, Addresses.SignatureStorage, ClientSignature);
                if (!clientSignature.Item2)
                {
                    DependencyService.Get<ILogger>().Report(clientSignature.Item1);
                    MessagingService.Current.SendMessage(MessageKeys.Error, clientSignature.Item1);
                    return;
                }

                if (Wine != null)
                {
                    UpdateWine(Wine, managerSignature.Item1, clientSignature.Item1);
                    _navService.GoBack();
                }
                else
                {
                    foreach (var item in SelectedWines)
                    {
                        UpdateWine(item, managerSignature.Item1, clientSignature.Item1);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        _navService.GoBack();
                    }
                }

                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                {
                    Title = "Manager & Customer Signature Saved",
                    Message = $"Wine {Wine.WineTitle} check out signatures have been recorded!",
                    Cancel = "OK"
                });
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "UpdateWineCheckout");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
           
         
        }
        #region Methods

      
        private async void UpdateWine(Wine item,string managerSignature,string clientSignature)
        {
            item.CheckedOutDate = DateTime.Now;
            item.CheckedOutLockerMemberID = User.UserID.ToString();
            item.CheckedOutEmployeeSignature = managerSignature;
            item.IsChecked = true;
            item.CheckedOutMemberSignature = clientSignature;
            await WinesDb.UpdateWine(item);
        }
        #endregion
        #endregion
    }
}
