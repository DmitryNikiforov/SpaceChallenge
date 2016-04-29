using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;

namespace SpaceHerders.Services
{
    public interface IUsersLocationService
    {
        Task<ICollection<Point>> GetCloseUsers(Point point, double radius);

        Task<Point> GetLastUserPosition(Guid userId);

        Task UpdateUserPosition(Guid userId, Point point);
    }
}