using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util.Logging;
using MvvmCross.Platform;
using MvvmCross.Plugins.Location;
using MvvmCross.Plugins.Location.Fused.Droid;

namespace OsmTest.Android.Services
{
   class LocationService
   {
      private IMvxLocationWatcher _locationWatcher;

      public LocationService()
      {
         _locationWatcher = new MvxAndroidFusedLocationWatcher();
      }

      public Task<MvxGeoLocation> GetLocation(CancellationTokenSource tokenSource = null)
      {
         var tcs = new TaskCompletionSource<MvxGeoLocation>();
         StopLocationWatcher();
         _locationWatcher.Start(new MvxLocationOptions()
            {
               Accuracy = MvxLocationAccuracy.Coarse,
               TrackingMode = MvxLocationTrackingMode.Background
            }, location =>
            {
               tcs.TrySetResult(location);
               _locationWatcher.Stop();
            },
            error =>
            {
               tcs.TrySetResult(null);
            });

         Task.Run(async () =>
         {
            if (tokenSource != null)
            {
               await Task.Delay(60000, tokenSource.Token);
            }
            else
            {
               await Task.Delay(60000);
            }

            if (!tcs.Task.IsCompleted)
            {
               MvxGeoLocation currLocation = null;
               try
               {
                  currLocation = _locationWatcher.CurrentLocation;
               }
               catch
               {
               }
               
               tcs.TrySetResult(currLocation);
            }
         });

         return tcs.Task;
      }

      public void StopLocationWatcher()
      {
         if (_locationWatcher != null && _locationWatcher.Started)
         {
            _locationWatcher.Stop();
         }
      }
   }
}