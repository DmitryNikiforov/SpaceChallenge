using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;

namespace SpaceHerders.Services
{
    public class InMemoryUsersLocationService : IUsersLocationService
    {
        private readonly IDictionary<Guid, Point> _data = new ConcurrentDictionary<Guid, Point>();
 
        public async Task<ICollection<Point>> GetCloseUsers(Point point, double radius)
        {
            return _data.Values;
        }

        public async Task<Point> GetLastUserPosition(Guid userId)
        {
            Point coordinates;
            _data.TryGetValue(userId, out coordinates);

            return coordinates;
        }

        public async Task UpdateUserPosition(Guid userId, Point point)
        {
            _data[userId] = point;
        }
    }
}
