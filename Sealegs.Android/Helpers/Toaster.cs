using System;

using Android.App;
using Plugin.CurrentActivity;
using Android.Widget;

using Xamarin.Forms;

using Sealegs.Clients.Portable;
using Sealegs.Droid;

[assembly:Dependency(typeof(Toaster))]
namespace Sealegs.Droid
{
    public class Toaster : IToast
    {
        public void SendToast(string message)
        {
            var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;  
            Device.BeginInvokeOnMainThread(() =>
                {
                    Toast.MakeText(context, message, ToastLength.Long).Show();
                });
        }
    }
}

