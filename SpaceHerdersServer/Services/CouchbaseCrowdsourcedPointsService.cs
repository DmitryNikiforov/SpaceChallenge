using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using GeoJSON.Net.Geometry;
using SpaceHerders.Models;

namespace SpaceHerders.Services
{
    public class CouchbaseCrowdsourcedPointsService : ICrowdsourcedPointsService
    {
        private readonly IBucket _bucket;

        public CouchbaseCrowdsourcedPointsService(IBucket bucket)
        {
            _bucket = bucket;
        }

        public async Task<ICollection<Place>> GetClosePoint(Point point, double radius)
        {
            //_bucket.QueryAsync<View>()
            return await Task.FromResult(new Place[0]);
        }

        public async Task CreateCrowdsourcedPoint(Place place)
        {
            place.PlaceId = Guid.NewGuid();

            await _bucket.InsertAsync(new Document<Place>()
            {
                Id = place.PlaceId.ToString(),
                Content = place
            });
        }
    }
}
