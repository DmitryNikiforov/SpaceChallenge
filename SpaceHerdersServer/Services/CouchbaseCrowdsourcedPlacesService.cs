using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using GeoJSON.Net.Geometry;
using SpaceHerders.Models;

namespace SpaceHerders.Services
{
    public interface ICrowdsourcedPlacesService
    {
        Task<ICollection<CrowdsourcedPlace>> GetClosePlaces(Point point, double radius);

        Task CreateCrowdsourcedPoint(CrowdsourcedPlace place);
    }

    public class CouchbaseCrowdsourcedPlacesService : ICrowdsourcedPlacesService
    {
        private readonly IBucket _bucket;

        public CouchbaseCrowdsourcedPlacesService(IBucket bucket)
        {
            _bucket = bucket;
        }

        public async Task<ICollection<CrowdsourcedPlace>> GetClosePlaces(Point point, double radius)
        {
            //_bucket.QueryAsync<View>()
            return await Task.FromResult(new CrowdsourcedPlace[0]);
        }

        public async Task CreateCrowdsourcedPoint(CrowdsourcedPlace place)
        {
            place.PlaceId = Guid.NewGuid();

            var status = await _bucket.InsertAsync(place.PlaceId.ToString(), place).ConfigureAwait(false);

            if(!status.Success)
                throw new ApplicationException(status.Message, status.Exception);
        }
    }
}
