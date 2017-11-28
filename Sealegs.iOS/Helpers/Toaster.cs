using Xamarin.Forms;

using ToastIOS;
using UIKit;
using CoreGraphics;

using Sealegs.Clients.Portable;
using Sealegs.iOS;

[assembly:Dependency(typeof(Toaster))]
namespace Sealegs.iOS
{
    public class Toaster : IToast
    {
        public void SendToast(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
                {
                    Toast.MakeText(message, Toast.LENGTH_LONG).SetCornerRadius(0).Show();
                });
        }
    }
}
