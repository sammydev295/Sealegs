using Android.App;
using Android.Content;

using Gcm;

using Sealegs.Clients.Portable;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
namespace Sealegs.Droid
{
    // https://components.xamarin.com/gettingstarted/gcmclient
    // https://developer.xamarin.com/guides/cross-platform/application_fundamentals/notifications/android/
    // gcm package name: com.sealegs.lockerapp
    // gcm server api key: AIzaSyD3032vz499Lscl6P5Ul5Ib9oq6GZitNl4
    // gcm sender id: 381575223842
    [BroadcastReceiver(Permission = Constants.PERMISSION_GCM_INTENTS, Name = "com.sealegs.lockerapp.GcmBroadcastReceiver")]
    [IntentFilter(new[] { Intent.ActionBootCompleted })] // Allow GCM on boot and when app is closed   
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class GcmBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SENDERIDS = { ApiKeys.GoogleSenderId };
    }
}

