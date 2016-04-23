using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Util;
using Java.Net;

namespace com.example.android.wifidirect
{
    public class FileTransferService: IntentService
    {
      private string _tag = "WifiDirect";
      private const int SocketTimeout = 5000;
        public static String ActionSendFile = "com.example.android.wifidirect.SEND_FILE";
        public static String ExtrasFilePath = "file_url";
        public static String ExtrasGroupOwnerAddress = "go_host";
        public static String ExtrasGroupOwnerPort = "go_port";

        public FileTransferService() 
            : base("FileTransferService")
        {
        }

        public FileTransferService(string name)
            : base(name)
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
            var context = ApplicationContext;
            if (intent.Action.Equals(ActionSendFile))
            {
                var fileUri = intent.GetStringExtra(ExtrasFilePath);
                var host = intent.GetStringExtra(ExtrasGroupOwnerAddress);
                var port = intent.GetIntExtra(ExtrasGroupOwnerPort, 8988);
                var socket = new Socket();

                try
                {
                    Log.Debug(_tag, "Opening client socket - ");
                    socket.Bind(null);
                    socket.Connect(new InetSocketAddress(host, port), SocketTimeout);

                    Log.Debug(_tag, "Client socket - " + socket.IsConnected);
                    var stream = socket.OutputStream;
                    var cr = context.ContentResolver;
                    Stream inputStream = null;
                    try
                    {
                        inputStream = cr.OpenInputStream(Android.Net.Uri.Parse(fileUri));
                    }
                    catch (FileNotFoundException e)
                    {
                        Log.Debug(_tag, e.ToString());
                    }
                    //DeviceDetailFragment.CopyFile(inputStream, stream);
                    Log.Debug(_tag, "Client: Data written");
                }
                catch (IOException e)
                {
                    Log.Debug(_tag, e.Message);
                }
                finally
                {
                    if (socket != null)
                    {
                        if (socket.IsConnected)
                        {
                            try
                            {
                                socket.Close();
                            }
                            catch (IOException e)
                            {
                                // Give up
                                Log.Debug(_tag, "Gave up on closing socket " + e.StackTrace);
                            }
                        }
                    }
                }
            }
        }
    }
}