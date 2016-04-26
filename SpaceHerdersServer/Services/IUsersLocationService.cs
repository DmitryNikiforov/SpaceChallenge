using System;
using System.Collections.Generic;
using Space_Herdsmans.Models;

namespace Space_Herdsmans.Services
{
    public interface IUsersLocationService
    {
        ICollection<GeoCoordinates> GetCloseUsers(GeoCoordinates point, double radius);

        GeoCoordinates GetLastUserPosition(Guid userId);

        void UpdateUserPosition(Guid userId, GeoCoordinates point);
    }
}