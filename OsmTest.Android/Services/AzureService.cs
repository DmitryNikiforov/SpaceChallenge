using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ModernHttpClient;

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
         HttpClient webClient = new HttpClient(new NativeMessageHandler());

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
   }
}