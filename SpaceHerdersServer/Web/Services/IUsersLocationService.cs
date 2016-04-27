using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpaceHerders.Web.Models;

namespace SpaceHerders.Web.Services
{
    public interface IUsersLocationService
    {
        Task<ICollection<GeoCoordinates>> GetCloseUsers(GeoCoordinates point, double radius);

        Task<GeoCoordinates> GetLastUserPosition(Guid userId);

        Task UpdateUserPosition(Guid userId, GeoCoordinates point);
    }
}