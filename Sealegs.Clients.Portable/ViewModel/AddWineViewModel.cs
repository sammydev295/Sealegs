using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;

using Xamarin.Forms;
using MvvmHelpers;
using FormsToolkit;
using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;

using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class AddWineViewModel : BaseViewModel
    {
        #region CTOR

        public INavigationService _navService;
        public AddWineViewModel(INavigationService navigation)
        {
            _navService = navigation;
        }

        #endregion

        #region Initialize (2 overloads)

        public async Task Initialize(LockerMember locker, List<WineVarietal> wineVarietals)
        {
            try
            {
                Locker = locker;
                Wine = Wine.Defaults;
                Wine.LockerID = locker.LockerMemberID;
                VintageList.Add("unknown");
                SelectedImage = null;
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "Initialize");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
        }

        public async Task Initialize(Wine wine, List<WineVarietal> wineVarietals)
        {
            try
            {
                Wine = wine;
                WineVarietals = new ObservableRangeCollection<WineVarietal>(wineVarietals);
                SelectedVarietal = wineVarietals.Any(v => v.WineVarietalId == wine.WineVarietalId) ? wineVarietals.First(v => v.WineVarietalId == wine.WineVarietalId) : null;
                VintageList.Add("unknown");
                SelectedImage = null;
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "Initialize");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
        }

        #endregion

        #region Properties

        public ObservableRangeCollection<string> BottleSizeList => new ObservableRangeCollection<string>(new List<string>()
        {
            "750 ml","1.0 l","1.5 l", "3.0 l", "6.0 l", "other"
        });

        public ObservableRangeCollection<string> VintageList => new ObservableRangeCollection<string>(Enumerable.Range(1930, DateTime.Now.Year - 1929).Reverse().Select(v => v.ToString()));

        ObservableRangeCollection<WineVarietal> _wineVarietals;
        public ObservableRangeCollection<WineVarietal> WineVarietals
        {
            get => _wineVarietals;
            set
            {
                _wineVarietals = value;
                RaisePropertyChanged();
            }
        }

        private bool _oCRTextAlso = true;
        public bool OCRTextAlso
        {
            get => _oCRTextAlso;
            set
            {
                _oCRTextAlso = value;
                RaisePropertyChanged();
            }
        }

        #region SelectedVarietal 

        private WineVarietal _selectedVarietal;
        public WineVarietal SelectedVarietal
        {
            get => _selectedVarietal;
            set
            {
                _selectedVarietal = value;
                RaisePropertyChanged();
            }
        }

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
            }
        }

        #endregion

        #region Wine 

        private Wine _wine;
        public Wine Wine
        {
            get => _wine;
            set
            {
                _wine = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Relay Commands

        public RelayCommand AddCommand => new RelayCommand(Save);

        public RelayCommand AddImageCommand => new RelayCommand(AddImage);

        #endregion

        #region Events Handler Support Methods

        #region Save

        private async void Save()
        {
            if (IsBusy) return;
            bool result = false;
            try
            {
                IsBusy = true;

                Wine.WineVarietalId = SelectedVarietal?.WineVarietalId;
                Toast.SendToast($"{ButtonName} wine ...");
                if (SelectedImage != null)
                {
                    // Store image
                    var storage = DependencyService.Get<IAzureBlobStorage>();

                    // Set image from URI
                    Toast.SendToast("Saving wine label image to cloud...");
                    SelectedImage.Position = 0;
                    var imageName = await storage.UploadFileAsync(Addresses.AzureStorageBlobContainer, Addresses.WinesStorage, SelectedImage);
                    if (!imageName.Item2)
                    {
                        DependencyService.Get<ILogger>().Report(imageName.Item1);
                        MessagingService.Current.SendMessage(MessageKeys.Error, imageName.Item1);
                        return;
                    }
                    Wine.ImagePath = WinesDb.GetFullImageName(imageName.Item1);
                }

                if (ButtonName == "Update")
                    result = await WinesDb.UpdateWine(Wine);
                else
                    result = await WinesDb.InsertWine(Wine);
                if (result)
                {
                    MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                    {
                        Title = $"Wine {ButtonName}d",
                        Message = $"Wine {Wine.WineTitle} has been {ButtonName}d!",
                        Cancel = "OK"
                    });
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync($"Sorry something went wrong during wine {Wine.WineTitle} {ButtonName}d!");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "Save");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                if(result) MessagingService.Current.SendMessage((ButtonName == "Update") ? MessageKeys.UpdateWine : MessageKeys.InsertWine, Wine);
                (_navService as ISealegsNavigationService).PopupGoBack();
                IsBusy = false;
            }
        }

        #endregion

        #region AddImage

        private Stream SelectedImage = null;
        private async void AddImage()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var media = DependencyService.Get<MediaService>();
                var image = await media.TakePhoto();
                if (image.Item1 == null && image.Item2 != null)
                {
                    DependencyService.Get<ILogger>().Report(image.Item2);
                    MessagingService.Current.SendMessage(MessageKeys.Error, image.Item2);
                    return;
                }
                else if (image.Item1 == null && image.Item2 == null)
                    return;

                SelectedImage = image.Item4;
                Wine.ImagePath = image.Item2;

                // OCR label text possibly
                if (!OCRTextAlso)
                {
                    MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                    {
                        Title = "Wine Label Image Captured",
                        Message = $"Wine {Wine.WineTitle}, {Wine.Vintage} label image has been captured!",
                        Cancel = "OK"
                    });
                    return;
                }

                var cognition = DependencyService.Get<IAzureCognitiveServices>();
                SelectedImage.Position = 0;
                Toast.SendToast("Recognizing wine label text...");
                var notes = await cognition.DescribeTextAsync(SelectedImage);
                if (!notes.Item2)
                {
                    DependencyService.Get<ILogger>().Report(notes.Item1);
                    MessagingService.Current.SendMessage(MessageKeys.Error, notes.Item1);
                    return;
                }
                Wine.Notes = notes.Item1;

                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                {
                    Title = "Wine Label Image & Text",
                    Message = $"Wine {Wine.WineTitle}, {Wine.Vintage} label image has been captured and image text has been extracted!",
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
                IsBusy = false;
            }
        }

        #endregion

        #endregion
    }
}
