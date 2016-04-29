using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ModernHttpClient;
using Newtonsoft.Json;
using OsmSharp.Osm.Tiles;
using Color = Android.Graphics.Color;

namespace OsmTest.Android.Services
{
   public class AzureService
   {
      static AzureService()
      {
         ServicePointManager.DefaultConnectionLimit = 10;
         ServicePointManager.Expect100Continue = false;
         WebRequest.DefaultWebProxy = null;
         ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
         {
            return true;
         };
      }

      public async Task<string> DownloadStringAsync(string url, int timeout, CancellationTokenSource token = null)
      {
         try
         {
            using (var request = GetRequest(url))
            {
               request.Timeout = TimeSpan.FromMilliseconds(timeout);
               var requestMessage = GetMessage(url, HttpMethod.Get, "");
               HttpResponseMessage response = null;
               if (token != null)
                  response = await request.SendAsync(requestMessage, token.Token);
               else
                  response = await request.SendAsync(requestMessage);
               var res = await response.Content.ReadAsStringAsync();
               return res;
            }
         }
         catch (TaskCanceledException ex)
         {
            if (token != null && token.Token != null && token.Token.IsCancellationRequested)
               return string.Empty;
            else
               throw ex;
         }
      }

      public async Task<Stream> DownloadStreamAsync(string url, int timeout, CancellationTokenSource token = null)
      {
         try
         {
            using (var request = GetRequest(url))
            {
               request.Timeout = TimeSpan.FromMilliseconds(timeout);
               var requestMessage = GetMessage(url, HttpMethod.Get, "");
               HttpResponseMessage response = null;
               if (token != null)
                  response = await request.SendAsync(requestMessage, token.Token);
               else
                  response = await request.SendAsync(requestMessage);
               var res = await response.Content.ReadAsStreamAsync();
               return res;
            }
         }
         catch (TaskCanceledException ex)
         {
            if (token != null && token.Token != null && token.Token.IsCancellationRequested)
               return null;
            else
               throw ex;
         }
      }

      private HttpClient GetRequest(string url)
      {
         HttpClient webClient = new HttpClient(new NativeMessageHandler()) ;
         webClient.BaseAddress = new Uri(url);
         webClient.DefaultRequestHeaders.Accept.Clear();
         webClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
         return webClient;
      }

      private HttpRequestMessage GetMessage(string url, HttpMethod method, byte[] data = null)
      {
         HttpRequestMessage message = new HttpRequestMessage(method, url);
        
         message.Headers.Accept.Clear();
         message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
         if (data != null)
         {
            message.Content = new ByteArrayContent(data);
         }
         return message;
      }

      private HttpRequestMessage GetMessage(string url, HttpMethod method, string data = "")
      {
         HttpRequestMessage message = new HttpRequestMessage(method, url);
        
         message.Headers.Accept.Clear();
         message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
         if (!string.IsNullOrEmpty(data))
         {
            message.Content = new StringContent(data, Encoding.ASCII, "application/json");
         }
         return message;
      }

      
      public Stream CreateBitmap(string url)
      {
         //http://tah.openstreetmap.org/Tiles/tile/0/0/0.png
         //string strUrl = string.Format("http://tah.openstreetmap.org/Tiles/tile/{0}/{1}/{2}.png", this.zoomLevel, this.x, this.y);
         System.Net.WebRequest r = System.Net.WebRequest.Create(url);
         //attempt to retrieve from the local cache
         r.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheOnly);

         try
         {
            System.Net.WebResponse response = r.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();
            return responseStream;
         }
         catch (WebException wex)
         {
            if (wex.Status == WebExceptionStatus.CacheEntryNotFound)
            {
               //request not in local cache - retrieve asyncronously
               //GetBitmapAsync();
            }
            throw;
            //Bitmap blankBitmap = new Bitmap(256, 256);
            //using (Graphics g = Graphics.FromImage(blankBitmap))
            //{
            //   g.Clear(Color.LightGray);
            //}
            //return blankBitmap;
         }
      }

      public async Task<R> PostDataAsync<R>(string url, object data, int timeout = 15000, bool ascii = true,
         CancellationTokenSource token = null)
      {
         R result = default(R);
         using (var request = GetRequest(url))
         {
            request.Timeout = TimeSpan.FromMilliseconds(timeout);
            var message = await Task.Run(() =>
            {
               string jsonString = JsonConvert.SerializeObject(data);

               return GetMessage(url, HttpMethod.Post, jsonString);
            });

            HttpResponseMessage response = null;
            if (token != null)
            {
               response = await request.SendAsync(message, token.Token);
            }
            else
            {
               response = await request.SendAsync(message);
            }
            var res = await response.Content.ReadAsStringAsync();


            if (!string.IsNullOrEmpty(res))
            {
               if (typeof (R) == typeof (string))
               {
                  result = ((R) (object) res);
               }
               else
               {
                  result = JsonConvert.DeserializeObject<R>(res);
               }
            }
            return result;
         }
      }
   }
}