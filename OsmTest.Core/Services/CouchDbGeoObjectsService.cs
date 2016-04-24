using System;
using System.Collections.Generic;
using System.Linq;
using Couchbase.Lite;
using Couchbase.Lite.Auth;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace OsmTest.Core.Services
{
    public class CouchDbGeoObjectsService : IGeoObjectsService
    {
        private readonly QueryEnumerator _docs;
        private readonly Database _db = Manager.SharedInstance.GetDatabase("main");

        public CouchDbGeoObjectsService(string viewName)
        {
            var url = new Uri("http://104.236.29.68:4985/sync_gateway/");
            var auth = AuthenticatorFactory.CreateBasicAuthenticator("mobile", "123123");

            var pull = _db.CreatePullReplication(url);
            pull.Continuous = true;
            pull.Channels = new[] { "geo" };
            pull.Authenticator = auth;
            pull.Start();

            var view = _db.GetView("geo");
            view.SetMap((document, emit) =>
            {
                if (document.ContainsKey("geometry") && document.ContainsKey("name"))
                {
                    emit(new[] { document["geometry"] }, document);
                }
            }, "1");

            _docs = view.CreateQuery().Run();
        }

        public ICollection<GeoJsonObject> GetRivers(Feature point, double radius)
        {
            return _docs.Select(JsonConvert.SerializeObject).Select(JsonConvert.DeserializeObject<GeoJsonObject>).ToList();
        }
    }
}
