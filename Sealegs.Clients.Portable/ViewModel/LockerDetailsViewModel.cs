using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using MoreLinq;
using Acr.UserDialogs;
using FormsToolkit;
using Xamarin.Forms;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using Sealegs.Clients.Portable.Locator;
using Sealegs.Clients.Portable.ViewModel;

namespace Sealegs.Clients.Portable
{
    public class LockerDetailsViewModel : BaseViewModel
    {
        #region Fields 

        private readonly INavigationService _navigationService;
        
        #endregion

        #region CTOR 

        public LockerDetailsViewModel(INavigationService navigation)
        {
            _navigationService = navigation;
            LockerName = "New Locker";
        }

        #endregion

        #region Initialize

        private void Initialize()
        {
            LockerImageUri = Locker.ProfileImage;
            LockerName = Locker.DisplayName;
            ToggleActive = Locker.IsActive != null && Locker.IsActive.Value ? "Mark Locker InActive" : "Mark Locker Active";
        }

        #endregion

        #region Properties

        #region LockerTypes 

        public List<LockerType> LockerTypes { get; set; }

        #endregion

        #region WineVarietals 

        public List<WineVarietal> WineVarietals { get; set; }

        #endregion

        #region Locker 

        private LockerMember _locker;
        public LockerMember Locker
        {
            get => _locker;
            set
            {
                _locker = value;
                RaisePropertyChanged();
                Initialize();
            }
        }

        #endregion

        #region LockerName

        public string Lockername;
        public string LockerName
        {
            get => Lockername;
            set
            {
                Lockername = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region LockerImageUri

        private ImageSource _lockerImageUri;
        public ImageSource LockerImageUri
        {
            get => _lockerImageUri;
            set
            {
                _lockerImageUri = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ToggleActive

        private string _toggleActive;
        public string ToggleActive
        {
            get => _toggleActive;
            set
            {
                _toggleActive = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #endregion

        #region Relay Commands

        public RelayCommand CheckedInCommand => new RelayCommand(CheckedIn_OnClick);
        public RelayCommand CheckedOutCommand => new RelayCommand(CheckedOut_OnClick);
        public RelayCommand WinesCommand => new RelayCommand(Wine_OnClick);
        public RelayCommand WinesCardsCommand => new RelayCommand(WineCardView_OnClick);
        public RelayCommand InActiveCommand => new RelayCommand(InActive_OnClick);
        public RelayCommand DeleteCommand => new RelayCommand(Delete_OnClick);
        public RelayCommand EditCommand => new RelayCommand(Edit_OnClick);
        public RelayCommand CameraCommand => new RelayCommand(Camera_OnClick);
        public RelayCommand FavoriteCommand => new RelayCommand(async () => await ExecuteFavoriteCommandAsync());

        #endregion

        #region Event Handlers

        private void CheckedIn_OnClick()
        {
            (_navigationService as ISealegsNavigationService).PopupNavigateTo(ViewModelLocator.AddWine, Locker, WineVarietals);
        }

        private void CheckedOut_OnClick()
        {
            _navigationService.NavigateTo(ViewModelLocator.CheckOut, Locker);
        }

        private void Wine_OnClick()
        {
            _navigationService.NavigateTo(ViewModelLocator.WineCarousel, new Tuple<LockerMember, List<WineVarietal>>(Locker, WineVarietals));
        }

        private void WineCardView_OnClick()
        {
            _navigationService.NavigateTo(ViewModelLocator.WineCardView, Locker);
        }

        #region InActive_OnClick

        private async void InActive_OnClick()
        {
            try
            {
                IsBusy = true;

                var confirm = await UserDialogs.Instance.ConfirmAsync($"Are you sure you want to deactivate locker {Locker.DisplayName}", "DEACTIVATE LOCKER?", "Yes", "No");
                if (!confirm)
                    return;

                Toast.SendToast($"Deactivating locker {Locker.DisplayName}...");

                var deactivated = await LockerMemberDb.UpdateLocker(Locker);
                if (deactivated)
                {
                    Locker.IsActive = !Locker.IsActive.Value;
                    ToggleActive = Locker.IsActive.Value ? "Mark Locker InActive" : "Mark Locker Active";
                    UserDialogs.Instance.ShowSuccess($"Deactivated locker {Locker.DisplayName}", 3000);
                }
                else
                    await UserDialogs.Instance.AlertAsync($"Unable to Deactivate locker {Locker.DisplayName}", "Please contact support!", "OK");
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "InActive_OnClick");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion 

        #region Delete_OnClick

        private async void Delete_OnClick()
        {
            try
            {
                IsBusy = true;

                var confirm = await UserDialogs.Instance.ConfirmAsync($"Are you sure you want to delete locker {Locker.DisplayName}", "DELETE LOCKER WARNING??", "Yes", "No");
                if (!confirm)
                    return;

                Toast.SendToast($"Deleting locker {Locker.DisplayName}...");
                var deleted = await LockerMemberDb.DeleteLocker(Locker.Id); 
                if (deleted)
                    UserDialogs.Instance.ShowSuccess($"Deleted locker {Locker.DisplayName}", 3000);
                else
                    await UserDialogs.Instance.AlertAsync($"Unable to delete locker {Locker.DisplayName}", "Please contact support!", "OK");

                if (deleted)
                    _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "Delete_OnClick");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion 

        private void Edit_OnClick()
        {
            (_navigationService as ISealegsNavigationService).PopupNavigateTo(ViewModelLocator.AddEditLocker, Locker, LockerTypes);
        }

        #region Camera_OnClick

        private async void Camera_OnClick()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var media = DependencyService.Get<MediaService>();
                var image = await media.SelectSource();
                if (image.Item1 == null && image.Item2 != null)
                {
                    DependencyService.Get<ILogger>().Report(image.Item2);
                    MessagingService.Current.SendMessage(MessageKeys.Error, image.Item2);
                    return;
                }
                else if (image.Item1 == null && image.Item2 == null)
                    return;

                LockerImageUri = image.Item2;

                // Store image
                var storage = DependencyService.Get<IAzureBlobStorage>();

                // Set image from URI
                Toast.SendToast("Saving locker profile image...");
                var imageName = await storage.UploadFileAsync(Addresses.AzureStorageBlobContainer, Addresses.LockersStorage, image.Item4);
                if (!imageName.Item2)
                {
                    DependencyService.Get<ILogger>().Report(imageName.Item1);
                    MessagingService.Current.SendMessage(MessageKeys.Error, imageName.Item1);
                    return;
                }
                LockerImageUri = Locker.ProfileImage = LockerMemberDb.GetFullImageName(imageName.Item1);

                await LockerMemberDb.UpdateLocker(Locker);

                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                {
                    Title = "Locker Profile Image",
                    Message = $"Locker {Locker.DisplayName} profile image has been saved!",
                    Cancel = "OK"
                });
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "Camera_OnClicked");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                MessagingService.Current.SendMessage(MessageKeys.UpdateLocker, Locker.ProfileImage);
                IsBusy = false;
            }
        }

        #endregion

        #region ExecuteFavoriteCommandAsync

        private async Task ExecuteFavoriteCommandAsync()
        {
            IsBusy = true;
            try
            {
                IsBusy = true;
                Toast.SendToast($"Togging favorite for locker {Locker.DisplayName} to {Locker.IsFavorite.ToString()}...");
                var toggled = await FavoriteService.ToggleFavorite(Locker);
                if (toggled)
                    UserDialogs.Instance.ShowSuccess($"Toggled locker {Locker.DisplayName} favorite {Locker.IsFavorite.ToString()}");
                else
                    await UserDialogs.Instance.AlertAsync($"Unable to change locker {Locker.DisplayName} favorite status to {Locker.IsFavorite.ToString()}", "Please contact support!", "OK");
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "OnSaveLockerAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion 

        #endregion
    }
}