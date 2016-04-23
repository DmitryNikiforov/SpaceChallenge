using System;
using Couchbase.Lite;
using GeoJSON.Net.Feature;

namespace OsmTest.Core.Services
{
    public class CouchDbGeoObjectsService : IGeoObjectsService
    {
        private readonly string _viewName;
        private readonly Database _db = Manager.SharedInstance.GetDatabase("main");

        public CouchDbGeoObjectsService(Uri serverUrl, string viewName)
        {
            _viewName = viewName;

            //var replication = _db.CreatePullReplication(serverUrl);
            //replication.Continuous = true;
            //replication.Start();
        }

        public FeatureCollection GetCloseUsers(Feature point, double radius)
        {
            var res = new FeatureCollection();
            using (var query = _db.GetView(_viewName).CreateQuery())
            {
                foreach (var entry in query.Run())
                {
                    res.Features.Add(entry.ValueAs<Feature>());
                }
            }

            return res;
        }
    }
}
