using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Acr.UserDialogs;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;

namespace Sealegs.Clients.Portable
{
    public class MediaService
    {
        /// <summary>
        /// Pick an existing picture from the camera roll or take a new picture
        /// </summary>
        /// <param name="App"></param>
        /// <returns>a Tuple with 2 items, the image source and the album file path to the selected or snapped picture</returns>
        public async Task<Tuple<ImageSource, string, Byte[], Stream>> SelectSource()
        {
            var selectedItem = await UserDialogs.Instance.ActionSheetAsync("Choose Media", "Cancel",
                null, null, "Take Photo", "Browse From Album");

            Tuple<ImageSource, string, Byte[], Stream> src = null;
            switch (selectedItem)
            {
                case "Take Photo":
                    src = await TakePhoto();
                    break;
                case "Browse From Album":
                    src = await PickPhoto();
                    break;
            }

            return src;
        }

        /// Pick an existing picture from the camera roll album
        /// </summary>
        /// <param name="App">Non null if method should display error and prompt messages to caller</param>
        /// <returns>a Tuple with 2 items, the image source and the album file path to the selected picture. If something goes wrong then the first item returned is null with the error message in the 2nd item</returns>
        public async Task<Tuple<ImageSource, string, Byte[], Stream>> PickPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await UserDialogs.Instance.AlertAsync("Photos Not Supported", ":( Permission not granted to take photo", "OK");
                return new Tuple<ImageSource, string, Byte[], Stream>(null, ":( Permission not granted to take photo", null, null);
            }

            MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
            {
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.Medium,
            });

            var path = file.Path;
            return new Tuple<ImageSource, string, Byte[], Stream>(ImageSource.FromStream(() =>
             {
                 var stream = file.GetStream();
                 file.Dispose();
                 return stream;
             }), path, ConvertStreamToByteArray(file.GetStream()), file.GetStream());
        }

        /// <summary>
        /// Take a new picture with the camera
        /// </summary>
        /// <param name="App">Non null if method should display error and prompt messages to caller</param>
        /// <returns>a Tuple with 2 items, the image source and the album file path to the snapped picture. If something goes wrong then the first item returned is null with the error message in the 2nd item</returns>
        /// <summary>
        public async Task<Tuple<ImageSource, string, Byte[], Stream>> TakePhoto()
        {
            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await UserDialogs.Instance.AlertAsync("Message", "Camera not Available!", "Okay");
                return new Tuple<ImageSource, string, Byte[], Stream>(null, ":( Camera not Available!", null, null);
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Directory = "Sealegs",
                SaveToAlbum = true,
                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = Device.OS == TargetPlatform.iOS ? 20 : 35,
                Name = "Sealegs " + DateTime.Now.ToString("yyyyMMddHHmmss")
            });

            var path = file.Path;
            return new Tuple<ImageSource, string, Byte[], Stream>(ImageSource.FromStream(() =>
             {
                 var stream = file.GetStream();
                 file.Dispose();
                 return stream;
             }), path, ConvertStreamToByteArray(file.GetStream()), file.GetStream());
        }

        /// <summary>
        /// Cature a new video with the camera
        /// </summary>
        /// <param name="App">Non null if method should display error and prompt messages to caller</param>
        /// <returns>The file path to the captured video. If something goes wrong then the null is returned</returns>
        /// <summary>
        public async Task<string> PickVideo()
        {
            if (!CrossMedia.Current.IsPickVideoSupported)
            {
                await UserDialogs.Instance.AlertAsync("Videos Not Supported", ":( Permission not granted to videos.", "OK");
                return null;
            }

            var file = await CrossMedia.Current.PickVideoAsync();
            if (file == null)
                return null;

            var filePath = file.Path;
            await UserDialogs.Instance.AlertAsync("Video Selected", "Location: " + file.Path, "OK");
            file.Dispose();

            return filePath;
        }

        /// <summary>
        /// Pick an existing video from the camera roll album
        /// </summary>
        /// <param name="App">Non null if method should display error and prompt messages to caller</param>
        /// <returns>The file path to the selected video. If something goes wrong then the null is returned</returns>
        /// <summary>
        public async Task<string> TakeVideo()
        {
            if (!CrossMedia.Current.IsTakeVideoSupported)
            {
                await UserDialogs.Instance.AlertAsync("Message", "Camera not Available !", "Okay");
                return null;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
            {
                Name = "video.mp4",
                Directory = "DefaultVideos",
            });

            if (file == null)
                return null;

            var filePath = file.Path;
            await UserDialogs.Instance.AlertAsync("Video Recorded", "Location: " + file.Path, "OK");

            file.Dispose();

            return filePath;
        }

        private byte[] ConvertStreamToByteArray(Stream stream)
        {
            byte[] imgBytes;
            using (var streamReader = new MemoryStream())
            {
                stream.CopyTo(streamReader);
                imgBytes = streamReader.ToArray();
            }

            return imgBytes;
        }
    }
}
