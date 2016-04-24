using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Couchbase.Lite;
using Couchbase.Lite.Auth;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace OsmTest.Core.Services
{
    public interface IGeoObjectsService
    {
        ICollection<GeoJsonObject> GetRivers(Feature point, double radius);
    }

    public class CouchDbGeoObjectsService : IGeoObjectsService
    {
        private readonly QueryEnumerator _docs;
        private readonly Database _db = Manager.SharedInstance.GetDatabase("main");

        public CouchDbGeoObjectsService()
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
         List<GeoJsonObject> objects = new List<GeoJsonObject>();
          for(int i = 0; i < 1000; i++)
          {
            var coord = _docs.Skip(i).Take(1).FirstOrDefault();
             if (coord == null)
             {
                break;
             }
            var serial = JsonConvert.SerializeObject(coord.Value);
            Debug.WriteLine(serial);
            var current = JsonConvert.DeserializeObject<GeoJsonObject>(serial);
            objects.Add(current);
         }
           return objects;
           //return new[] {first};
         //return _docs.Select(JsonConvert.SerializeObject).Select(JsonConvert.DeserializeObject<GeoJsonObject>).ToList();
      }
    }
}
