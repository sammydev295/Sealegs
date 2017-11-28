using System;
using System.IO;

using FormsToolkit;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using Splat;
using Xamarin.Forms;

using Sealegs.Utils;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        #region Fields

        private INavigationService _navService;
        
        #endregion

        #region CTOR

        public ProfileViewModel(INavigationService navigation)
        {
            _navService = navigation;
        }

        #endregion

        #region Initialize (3 overloads)

        public void Initialize(User user)
        {
            AdminViewVisibilty = true;
            LockerMemberViewVisibilty = false;

            FirstName = user.FirstName;
            ProfilePath = user.ProfilePath;
            Email = user.Email;
            FullName = user.FirstName + " " + user.LastName;
            Role = user.Role;
        }

        public void Initialize(LockerMember lockerMember)
        {
            AdminViewVisibilty = false;
            LockerMemberViewVisibilty = true;
            LockerMember = lockerMember;
            ProfilePath = LockerMember.ProfileImage.Contains("/") ? LockerMember.ProfileImage : Addresses.LockersStorageBaseAddress + LockerMember.ProfileImage;
        }

        public void Initialize(SealegsUser sealegsUser)
        {
            AdminViewVisibilty = true;
            LockerMemberViewVisibilty = false;
            SealegsUser = sealegsUser;
            ProfilePath = SealegsUser.AvatarImage.Contains("/") ? SealegsUser.AvatarImage : Addresses.LockersStorageBaseAddress + SealegsUser.AvatarImage;
        }

        #endregion

        #region Observable Properties

        #region User
        private User _user;
        public User UserProfile
        {
            get => _user;
            set
            {
                _user = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Locker Member

        private LockerMember _locker;
        public LockerMember LockerMember
        {
            get => _locker;
            set
            {
                _locker = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Sealegs User
        private SealegsUser _sealegsUser;
        public SealegsUser SealegsUser
        {
            get => _sealegsUser;
            set
            {
                _sealegsUser = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Master View Model
        private MasterViewModel _masterViewModel;
        public MasterViewModel MasterViewModel
        {
            get => _masterViewModel;
            set
            {
                _masterViewModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Visibilty

        private bool _lockerMemberViewVisibilty;
        public bool LockerMemberViewVisibilty
        {
            get => _lockerMemberViewVisibilty;
            set
            {
                _lockerMemberViewVisibilty = value;
                RaisePropertyChanged();
            }
        }

        private bool _adminViewVisibilty = true;
        public bool AdminViewVisibilty
        {
            get => _adminViewVisibilty;
            set
            {
                _adminViewVisibilty = value;
                RaisePropertyChanged();
            }
        } 
        #endregion

        #region LockersCount

        private bool _isLocker;
        public bool IsLocker
        {
            get => _isLocker;
            set
            {
                _isLocker = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region FullName

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Role

        private string _role;
        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Email

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region FirstName

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ProfilePath

        private ImageSource _profilePath;
        public ImageSource ProfilePath
        {
            get => _profilePath;
            set
            {
                _profilePath = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        private bool _isFIngerScanEnable;
        public bool IsFingerScanEnable
        {
            get => _isFIngerScanEnable;
            set
            {
                _isFIngerScanEnable = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Relay Commands

        public RelayCommand CameraCommand => new RelayCommand(Camera_OnClick);

        public RelayCommand EditProfileCommand => new RelayCommand(EditProfile_OnClick);
        
        #endregion

        #region Event Handlers

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

                 ProfilePath = ImageSource.FromStream(()=>new MemoryStream(image.Item3));
                 MasterViewModel.ProfilePath = ProfilePath;

                if (UserProfile.Role == SealegsUserRole.LockerMemberRole)
                    UploadLockerImage(image);
                if (UserProfile.Role == SealegsUserRole.AdminRole)
                    UploadAdminImage(image);

            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "Camera_OnClicked");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region EditProfile_OnClick

        private async void EditProfile_OnClick()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                if (UserProfile.Role == SealegsUserRole.LockerMemberRole)
                {
                    MasterViewModel.Username = LockerMember.DisplayName;
                    await  LockerMemberDb.UpdateLocker(LockerMember);
                }

                if (UserProfile.Role == SealegsUserRole.AdminRole)
                {
                    await DependencyService.Get<IAuth>().Update(SealegsUser);
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #endregion

        #region UploadLockerImage

        private async void UploadLockerImage(Tuple<ImageSource, string, Byte[], Stream> image)
        {
            LockerMember.ProfileImage = image.Item2;

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
            LockerMember.ProfileImage = imageName.Item1;

            await LockerMemberDb.UpdateLocker(LockerMember);

            MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
            {
                Title = "Locker Profile Image",
                Message = $"Locker {LockerMember.DisplayName} profile image has been saved!",
                Cancel = "OK"
            });
        }

        #endregion

        #region UploadAdminImage

        private async void UploadAdminImage(Tuple<ImageSource, string, Byte[], Stream> image)
        {
            try
            {
                SealegsUser.AvatarImage = image.Item2;

                // Store image
                var storage = DependencyService.Get<IAzureBlobStorage>();

                // Set image from URI
                Toast.SendToast("Saving  profile image...");
                var imageName = await storage.UploadFileAsync(Addresses.AzureStorageBlobContainer, Addresses.LockersStorage, image.Item4);
                if (!imageName.Item2)
                {
                    DependencyService.Get<ILogger>().Report(imageName.Item1);
                    MessagingService.Current.SendMessage(MessageKeys.Error, imageName.Item1);
                    return;
                }
                SealegsUser.AvatarImage = imageName.Item1;

                await DependencyService.Get<IAuth>().Update(SealegsUser);

                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                {
                    Title = "Profile Image",
                    Message = $"Locker {SealegsUser.FirstName} profile image has been saved!",
                    Cancel = "OK"
                });
            }
            catch (Exception e)
            {
            }
        }
        #endregion
    }
}
