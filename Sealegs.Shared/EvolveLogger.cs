using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

using Xamarin.Forms;

using Sealegs.Clients.Portable;

[assembly:Dependency(typeof(SealegsLogger))]
namespace Sealegs.Clients.Portable
{
    public class SealegsLogger : ILogger
    {
        bool enableHockeyApp = false;

        #region ILogger implementation
       
        public virtual void TrackPage(string page, string id = null)
        {
            Debug.WriteLine("Evolve Logger: TrackPage: " + page.ToString() + " Id: " + id ?? string.Empty);

            if (!enableHockeyApp)
                return;
#if __ANDROID__

            HockeyApp.Android.Metrics.MetricsManager.TrackEvent($"{page}Page");
#elif __IOS__
            HockeyApp.iOS.BITHockeyManager.SharedHockeyManager?.MetricsManager?.TrackEvent($"{page}Page");
#endif
        }

        public virtual void Track(string trackIdentifier)
        {
            Debug.WriteLine("Evolve Logger: Track: " + trackIdentifier);

            if (!enableHockeyApp)
                return;

#if __ANDROID__
            HockeyApp.Android.Metrics.MetricsManager.TrackEvent(trackIdentifier);
#elif __IOS__
            HockeyApp.iOS.BITHockeyManager.SharedHockeyManager?.MetricsManager?.TrackEvent(trackIdentifier);
#endif
        }

        public virtual void Track(string trackIdentifier, string key, string value)
        {
            Debug.WriteLine("Evolve Logger: Track: " + trackIdentifier + " key: " + key + " value: " + value);

            if (!enableHockeyApp)
                return;
            
            trackIdentifier = $"{trackIdentifier}-{key}-{@value}";

#if __ANDROID__
            HockeyApp.Android.Metrics.MetricsManager.TrackEvent(trackIdentifier);
#elif __IOS__
            HockeyApp.iOS.BITHockeyManager.SharedHockeyManager?.MetricsManager?.TrackEvent(trackIdentifier);
#endif
        }

        public virtual void Report(String error, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Evolve Logger: Report: " + error);

        }

        public virtual void Report(Exception exception = null, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Evolve Logger: Report: " + exception);

        }

        public virtual void Report(Exception exception, IDictionary extraData, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Evolve Logger: Report: " + exception);
        }

        public virtual void Report(Exception exception, string key, string value, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Evolve Logger: Report: " + exception + " key: " + key + " value: " + value);
        }

        #endregion
    }
}

