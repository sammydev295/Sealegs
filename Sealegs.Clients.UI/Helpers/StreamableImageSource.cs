using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Sealegs.Clients.UI.Helpers
{
    public class StreamableImageSource : BindableObject
    {
        private byte[] ImageRaw { get; set; }

        public Stream ImageStream
        {
            get { return ImageRaw != null ? new MemoryStream(ImageRaw) : null; }
            set
            {
                if (value != null)
                {
                    ImageRaw = new byte[value.Length];
                    value.Read(ImageRaw, 0, ImageRaw.Length);

                    SetValue(ImageSourceProperty, ImageSource.FromStream(() => new MemoryStream(ImageRaw)));
                }
            }
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(StreamableImageSource), null);

        public ImageSource ImageSource
        {
            get
            {
                return ImageStream != null
                    ? ImageSource.FromStream(() => ImageStream)
                    : (ImageSource)GetValue(ImageSourceProperty);
            }
            set
            {
                ImageRaw = null;
                SetValue(ImageSourceProperty, value);
            }
        }
    }
}
