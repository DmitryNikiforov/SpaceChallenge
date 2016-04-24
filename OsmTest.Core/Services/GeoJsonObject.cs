using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoJSON.Net;

namespace OsmTest.Core.Services
{
   public class Geometry
   {
      public List<List<double>> coordinates { get; set; }
      public string type { get; set; }
   }

   public class GeoJsonObject
   {
      public Geometry geometry { get; set; }
      public string name { get; set; }
      public string subtype { get; set; }
      public string type { get; set; }
      public string _id { get; set; }
      public string _rev { get; set; }
   }
}
