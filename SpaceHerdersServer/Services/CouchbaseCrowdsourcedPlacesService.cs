using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using Couchbase.Views;
using GeoJSON.Net.Geometry;
using SpaceHerders.Models;

namespace SpaceHerders.Services
{
    public interface ICrowdsourcedPlacesService
    {
        Task<ICollection<CrowdsourcedPlace>> GetClosePlaces(SimplePoint start, SimplePoint end);

        Task CreateCrowdsourcedPoint(CrowdsourcedPlace place);
    }

    public class CouchbaseCrowdsourcedPlacesService : ICrowdsourcedPlacesService
    {
        private readonly IBucket _bucket;

        public CouchbaseCrowdsourcedPlacesService(IBucket bucket)
        {
            _bucket = bucket;
        }

        public async Task<ICollection<CrowdsourcedPlace>> GetClosePlaces(SimplePoint start, SimplePoint end)
        {
            var query = new SpatialViewQuery().From("doc", "crowdsourcedpoints")
                .Stale(StaleState.False)
                .StartRange(start.Longitude, start.Latitude)
                .EndRange(end.Longitude, end.Latitude);

            var result = await _bucket.QueryAsync<CrowdsourcedPlace>(query);
            return result.Rows.Select(x => x.Value).ToList();
        }

        public async Task CreateCrowdsourcedPoint(CrowdsourcedPlace place)
        {
            place.PlaceId = Guid.NewGuid();
            place.CreationTime = DateTime.UtcNow;

            var status = await _bucket.InsertAsync(place.PlaceId.ToString(), place).ConfigureAwait(false);

            if(!status.Success)
                throw new ApplicationException(status.Message, status.Exception);
        }
    }
}
