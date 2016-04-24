using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OsmDroid;
using OsmDroid.Api;
using OsmDroid.Util;
using OsmDroid.Views;
using OsmDroid.Views.Overlay;
using OsmSharp.IO.Xml.Kml.v2_1;

namespace OsmTest.Android.Views
{
   class BitmapOverlay : Overlay
   {
      private Location _location;
      private readonly Bitmap _bitmap;

      public BitmapOverlay(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
      {
      }

      public BitmapOverlay(Context p0) : base(p0)
      {
      }

      public BitmapOverlay(Context p0, /*Location location,*/ Bitmap bitmap) : base(p0)
      {
         //_location = location;
         _bitmap = bitmap;
      }

      public BitmapOverlay(IResourceProxy p0) : base(p0)
      {
      }

      //private void drawPlane(Canvas cs, Point center, float bearing)
      //{

      //   Paint paint = new Paint();
      //   paint.Color = Color.Red;
      //   paint.AntiAlias = true;

      //   Bitmap planeBM = Bitmap.CreateBitmap(_bitmap,);
      //   planeBM.Density = cs.Density;
      //   Canvas c = new Canvas(planeBM);

      //   Rect r = new Rect();
      //   //Point center = new Point(cs.Width / 2, cs.Height / 2);
      //   //Point center = new Point(0, 0);

      //   // Draw fuselage
      //   r.Left = center.X - cs.Width / 2;
      //   r.Right = r.Left + cs.Width;
      //   r.Top = center.Y - cs.Height / 3;
      //   r.Bottom = r.Top + cs.Height;

      //   c.DrawRect(r, paint);

      //   // Draw wing (REMOVED)

      //   // Draw stabilizer    (REMOVED)

      //   // TODO Draw Speed vector


      //   // "Merging" canvas

      //   Matrix merge = new Matrix(cs.Matrix);
      //   //merge.SetTranslate(loc.x - pCenter.x, loc.y - pCenter.y);
      //   //merge.PostRotate(bearing, loc.x, loc.y);
      //   cs.DrawBitmap(planeBM, merge, paint);
      //   cs.Save();

      //}

      public override void Draw(Canvas cs, MapView mapView, bool shadow)
      {
         if (_bitmap != null)
         {
            IProjection pj = mapView.Projection;

            GeoPoint leftTopGeo = new GeoPoint(-1.686000, 33.558542);
            Point leftTop = new Point();
            pj.ToPixels(leftTopGeo, leftTop);

            GeoPoint rightBottomGeo = new GeoPoint(-11.409878, 42.633248);
            Point rightBottom = new Point();
            pj.ToPixels(rightBottomGeo, rightBottom);

            //Point locPoint = new Point();
            //GeoPoint locGeoPoint = new GeoPoint(_location);
            
            //pj.ToPixels(locGeoPoint, locPoint);

            System.Diagnostics.Debug.WriteLine($"Draw tanzania fire. left {leftTop.X}, top {leftTop.Y}, right {rightBottom.X}, bottom {rightBottom.Y}");
            Rect rect = new Rect(leftTop.X, leftTop.Y, rightBottom.X, rightBottom.Y);
            cs.DrawBitmap(_bitmap, null, rect, new Paint());
            //this.drawPlane(cs, locPoint, _location.Bearing);
         }
      }
   }
}