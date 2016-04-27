using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpaceHerders.Web.Models;

namespace SpaceHerders.Web.Services
{
    public class InMemoryUsersLocationService : IUsersLocationService
    {
        private readonly IDictionary<Guid, GeoCoordinates> _data = new ConcurrentDictionary<Guid, GeoCoordinates>();
 
        public async Task<ICollection<GeoCoordinates>> GetCloseUsers(GeoCoordinates point, double radius)
        {
            return _data.Values;
        }

        public async Task<GeoCoordinates> GetLastUserPosition(Guid userId)
        {
            GeoCoordinates coordinates;
            _data.TryGetValue(userId, out coordinates);

            return coordinates;
        }

        public async Task UpdateUserPosition(Guid userId, GeoCoordinates point)
        {
            _data[userId] = point;
        }
    }
}
