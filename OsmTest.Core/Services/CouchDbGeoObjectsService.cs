using System;
using System.Collections.Generic;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;

namespace OsmTest.Core.Services
{
    public class CouchDbGeoObjectsService : IGeoObjectsService
    {
        public FeatureCollection GetCloseUsers(Feature point, double radius)
        {
           return new FeatureCollection(new List<Feature>() {new Feature(geometry: new Point(new GeographicPosition(-6.3423888, 30.392372)))}) ;
        }
    }
}
