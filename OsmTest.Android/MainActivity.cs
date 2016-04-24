using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Locations;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using GeoJSON.Net.Geometry;
using Org.Osmdroid.Views.Overlay.Mylocation;
using OsmDroid;
using OsmTest.Android.Model;
using OsmTest.Android.Services;
using Environment = Android.OS.Environment;

using OsmDroid.Api;
using OsmDroid.TileProvider;
using OsmDroid.TileProvider.TileSource;
using OsmDroid.Util;
using OsmDroid.Views;
using OsmDroid.Views.Overlay;
using OsmSharp.Geo.Geometries;
using OsmSharp.UI.Map.Styles;
using OsmTest.Android.Adapter;
using OsmTest.Android.Views;
using OsmTest.Core.Services;
using AlertDialog = Android.Support.V7.App.AlertDialog;


namespace OsmTest.Android
{
	[Activity (Label = "Moses", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light")]
	public class MainActivity : ActionBarActivity, IDialogInterfaceOnClickListener
   {
      /// <summary>
      /// Holds the mapview.
      /// </summary>
      private MapView _mapView;

	   ApiService _service = null;


      private CancellationTokenSource _cancellationTokenSource;
	   CustomLocationProvider _provider = null;
      private string css = @"
node
{
    color:#f00;
    width:2;
    opacity:0.7;
    fill-color:#fc0;
    fill-opacity:0.3;
}
way
{
    color:#03f;
    width:5;
    opacity:0.6;
}
area
{
    color:#03f;
    width:2;
    opacity:0.7;
    fill-color:#fc0;
    fill-opacity:0.3;
}
relation node, relation way, relation relation
{
    color:#d0f;
}";
      private IMapController _mapController;
	   IGeoObjectsService _geoService = null;
      //private MapView _mapView;

	   private RelativeLayout _sos = null;
	   protected RelativeLayout _layers = null;
	   protected RelativeLayout _myLocation = null;

      GeoPoint _centreOfMap = new GeoPoint(-6.3423888, 35.392372);
      protected async override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);
         SetContentView(Resource.Layout.Main);
         _service = new ApiService();
         SupportActionBar.DisplayOptions = (int)ActionBarDisplayOptions.ShowCustom;
         SupportActionBar.SetCustomView(Resource.Layout.ActionBar_Moses);
         _sos = SupportActionBar.CustomView.FindViewById<RelativeLayout>(Resource.Id.sos);
         _myLocation = SupportActionBar.CustomView.FindViewById<RelativeLayout>(Resource.Id.where_i);
         _layers = SupportActionBar.CustomView.FindViewById<RelativeLayout>(Resource.Id.layers);

         _sos.Click += delegate (object sender, EventArgs args)
         {
            Toast.MakeText(this, "Help me!!!!", ToastLength.Long).Show();
         };
         _layers.Click += delegate (object sender, EventArgs args)
         {
            AlertDialog.Builder builderSingle = new AlertDialog.Builder(this);
            builderSingle.SetTitle("Type");

            ArrayAdapterWithIcon arrayAdapter = new ArrayAdapterWithIcon(
                    this,
                    Android.Resource.Layout.select_dialog_singlechoice_material);
            arrayAdapter.Add(new ListItem() { ImageResourceId = Resource.Drawable.tse_tse });
            arrayAdapter.Add(new ListItem() { ImageResourceId = Resource.Drawable.grass });
            arrayAdapter.Add(new ListItem() { ImageResourceId = Resource.Drawable.water });
            arrayAdapter.Add(new ListItem() { ImageResourceId = Resource.Drawable.fire });
            builderSingle.SetNegativeButton("Cancel", NegativeHandler);
            builderSingle.SetAdapter(arrayAdapter, this);
            builderSingle.Show();
         };
         _myLocation.Click += delegate (object sender, EventArgs args)
         {
            _mapController.SetCenter(_centreOfMap);
         };
         //StyleInterpreter interpreter = null;
         try
         {
            bool isTile = true;
            //List<OsmGeo> osm = null;
            if (isTile)
            {
               _mapView = FindViewById<MapView>(Resource.Id.mapview);
               _mapView.SetTileSource(TileSourceFactory.DefaultTileSource);
               _mapView.SetBuiltInZoomControls(true);
               //_mapView.SetUseDataConnection(false);

               _provider = new CustomLocationProvider(this);
               _provider.StartLocationProvider(new MyLocationNewOverlay(this, _mapView));
               
               //List<OverlayItem> overlayItemArray = new List<OverlayItem>();
               //OverlayItem olItem = new OverlayItem("Here", "SampleDescription", new GeoPoint(34.878039, -10.650));
               //overlayItemArray.Add(olItem);
               //olItem.SetMarker(Resources.GetDrawable(Resource.Drawable.cloud));
               //overlayItemArray.Add(new OverlayItem("Hi", "You're here", new GeoPoint(34.888039, -10.660)));


               DefaultResourceProxyImpl defaultResourceProxyImpl = new DefaultResourceProxyImpl(this);
               //ItemizedIconOverlay myItemizedIconOverlay = new ItemizedIconOverlay(overlayItemArray, null, defaultResourceProxyImpl);
               //_mapView.Overlays.Add(myItemizedIconOverlay);

               //PathOverlay myPath = new PathOverlay(Color.Red, this);
               //myPath.AddPoint(new GeoPoint(34.878039, -10.650));
               //myPath.AddPoint(new GeoPoint(34.888039, -10.660));
               //_mapView.Overlays.Add(myPath);

               _mapController = _mapView.Controller;
               _mapController.SetZoom(6);
               
               _mapController.SetCenter(_centreOfMap);

               _geoService = new CouchDbGeoObjectsService();
               
               //var firstPoint = ((GeoJSON.Net.Geometry.Point) points.Features[0].Geometry);
               var firstPoint = new OsmSharp.Geo.Geometries.Point(new OsmSharp.Math.Geo.GeoCoordinate(33, -10));
               //double x = ((GeographicPosition) firstPoint.Coordinates).Latitude;
               //double y = ((GeographicPosition)firstPoint.Coordinates).Longitude;
               //List<OverlayItem> overlayItemArray = new List<OverlayItem>();
               //OverlayItem olItem = new OverlayItem("Here", "SampleDescription", new GeoPoint(x, y));
               //overlayItemArray.Add(olItem);
               //olItem.SetMarker(Resources.GetDrawable(Resource.Drawable.cloud));
               
               //ItemizedIconOverlay newPoints = new ItemizedIconOverlay(overlayItemArray, null, defaultResourceProxyImpl);
               //_mapView.Overlays.Add(newPoints);
            }
            else
            {
               /*List<OsmGeo> osm = await _service.DownloadArea(34.878039, -10.465, 36, -9.077);
               if (osm != null)
               {
                  Native.Initialize();

                  // initialize map.
                  var map = new Map();
                  interpreter = new MapCSSInterpreter(css);

                  IDataSourceReadOnly source = new MemoryDataSource(osm.ToArray());
                  var layer = map.AddLayerOsm(source, interpreter);

                  _mapView = new MapView(this, new MapViewSurface(this));
                  _mapView.Map = map;
                  _mapView.MapMaxZoomLevel = 17; // limit min/max zoom because MBTiles sample only contains a small portion of a map.
                  _mapView.MapMinZoomLevel = 1;
                  _mapView.MapTilt = 0;
                  _mapView.MapCenter = new GeoCoordinate(-9.2, 36);
                  _mapView.MapZoom = 2;
                  _mapView.MapAllowTilt = false;

                  //OsmSharp.Data.SQLite.SQLiteConnectionBase sqLiteConnection = new SQLiteConnection("osmMap");
                 
                  frame.AddView(_mapView);
                  var textLabel = FindViewById<TextView>(Resource.Id.text_label);
                  textLabel.Text = "Изображение подгрузилось";
                  Toast.MakeText(this, "Read success", ToastLength.Long).Show();
               }
               else
               {
                  Toast.MakeText(this, "OSM is null", ToastLength.Long).Show();
               }*/
            }
            
         }
         catch (Exception exception)
         {
            Toast.MakeText(this, exception.Message, ToastLength.Long).Show();
            SetContentView(Resource.Layout.Main);
         }
      }

	   public void ChangeCenterPoint(Location location)
	   {
	      _centreOfMap = new GeoPoint(location);
         _provider.StopLocationProvider();
	   }

	   protected override void OnStart()
	   {
	      base.OnStart();
        
	   }

	   protected async override void OnResume()
	   {
	      base.OnResume();
	   }

	   protected override void OnPause()
	   {
	      base.OnPause();
      }
      

	   private void NegativeHandler(object sender, DialogClickEventArgs dialogClickEventArgs)
	   {
         ((AlertDialog)sender).Dismiss();
	   }

	   //private async Task<MvxGeoLocation> UpdateLocation()
	   //{
	   //   _cancellationTokenSource?.Cancel();
	   //   _cancellationTokenSource = new CancellationTokenSource();
    //     var locationService = Mvx.Resolve<LocationService>();
	   //   return await locationService.GetLocation(_cancellationTokenSource);
	   //}
	   public void OnClick(IDialogInterface dialog, int which)
	   {
	      for (int i = _mapView.Overlays.Count - 1; i >= 0; i--)
	      {
	         _mapView.Overlays.RemoveAt(i);
	      }
         _mapView.Invalidate();
	      switch (which)
	      {
            case 0:
	         {
               GeoPoint leftTopGeo = new GeoPoint(-1.686000, 33.558542);
               GeoPoint rightBottomGeo = new GeoPoint(-11.409878, 42.633248);
               Bitmap tanzania = BitmapFactory.DecodeResource(Resources, Resource.Drawable.tsetse_tanzania);
	            Overlay newOverlay = new BitmapOverlay(this, leftTopGeo, rightBottomGeo, tanzania);
	            _mapView.Overlays.Add(newOverlay);
	         }
	         break;
            case 1:
	         {
               GeoPoint leftTopGeo = new GeoPoint(39.301256, -20.008362);
               GeoPoint rightBottomGeo = new GeoPoint(-37.787103, 51.759213);
               Bitmap tanzania = BitmapFactory.DecodeResource(Resources, Resource.Drawable.green_africa);
               Overlay newOverlay = new BitmapOverlay(this, leftTopGeo, rightBottomGeo, tanzania);
               _mapView.Overlays.Add(newOverlay);
            }
               break;
            case 2:
	         {
               var points = _geoService.GetRivers(null, 0);
	            GeoPoint lastPoint = null;
	            foreach (GeoJsonObject geoJsonObject in points)
	            {
                  PathOverlay newOverlay = new PathOverlay(Color.Blue, this);
                  foreach (List<double> coordinate in geoJsonObject.geometry.coordinates)
	               {
	                  var point = new GeoPoint(coordinate[0], coordinate[1]);
                     newOverlay.AddPoint(point);
	                  lastPoint = point;
	               }
                  _mapView.Overlays.Add(newOverlay);
               }
               _mapController.SetCenter(lastPoint);
               
               }
	            break;
            case 3:
               {
                  GeoPoint leftTopGeo = new GeoPoint(39.301256, -20.008362);
                  GeoPoint rightBottomGeo = new GeoPoint(-37.787103, 51.759213);
                  Bitmap tanzania = BitmapFactory.DecodeResource(Resources, Resource.Drawable.fire_africa);
                  Overlay newOverlay = new BitmapOverlay(this, leftTopGeo, rightBottomGeo, tanzania);
                  _mapView.Overlays.Add(newOverlay);
               }
               break;
         }


         //54.259213
      }
   }
}


