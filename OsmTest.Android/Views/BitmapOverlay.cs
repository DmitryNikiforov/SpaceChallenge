//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.Graphics;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using OsmDroid;
//using OsmDroid.Util;
//using OsmDroid.Views;
//using OsmDroid.Views.Overlay;

//namespace OsmTest.Android.Views
//{
//   class BitmapOverlay : Overlay
//   {
//      public BitmapOverlay(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
//      {
//      }

//      public BitmapOverlay(Context p0) : base(p0)
//      {
//      }

//      public BitmapOverlay(IResourceProxy p0) : base(p0)
//      {
//      }

//      private void drawPlane(Canvas cs, Point ctr, float bearing)
//      {

//         Paint paint = new Paint();
//         paint.Color = Color.Red;
//         paint.AntiAlias = true;

//         Bitmap planeBM = Bitmap.CreateBitmap(cs.Width, cs.Height, Bitmap.Config.Argb8888);
//         planeBM.Density = cs.Density;
//         Canvas c = new Canvas(planeBM);

//         Rect r = new Rect();

//         //Point center = new Point(cs.getWidth() / 2, cs.getHeight() /2);
//         Point center = new Point(0, 0);

//         // Draw fuselage
//         r.Left = center.X - PLANE_WIDTH / 2;
//         r.Right = r.Left + PLANE_WIDTH;
//         r.Top = center.Y - PLANE_SIZE / 3;
//         r.Bottom = r.Top + PLANE_SIZE;

//         c.DrawRect(r, paint);

//         // Draw wing (REMOVED)

//         // Draw stabilizer    (REMOVED)

//         // TODO Draw Speed vector


//         // "Merging" canvas

//         Matrix merge = new Matrix(cs.Matrix);
//         //merge.setTranslate(0, 0);
//         //merge.setRotate(bearing, center.x, center.y);
//         cs.DrawBitmap(planeBM, merge, paint);
//         cs.Save();

//      }

//      public override void Draw(Canvas p0, MapView p1, bool p2)
//      {
//         if (location != null)
//         {
//            Point locPoint = new Point();
//            GeoPoint locGeoPoint = new GeoPoint(location);
//            Projection pj = mapView.getProjection();
//            pj.ToMercatorPixels(locGeoPoint.Latitude, , locPoint);

//            this.drawPlane(c, locPoint, location.getBearing());
//         }
//      }
//   }
//}