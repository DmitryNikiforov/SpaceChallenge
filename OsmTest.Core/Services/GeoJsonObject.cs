using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoJSON.Net;

namespace OsmTest.Core.Services
{
    public class GeoJsonObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public JSGeometry Geometry { get; set; }

        public class JSGeometry
        {
            public GeoJSONObjectType Type;
            public double[][] Coordinates;
        }
    }
}
