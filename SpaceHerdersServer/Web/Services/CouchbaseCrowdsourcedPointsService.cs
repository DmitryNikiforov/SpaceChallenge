using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpaceHerders.Web.Models;

namespace SpaceHerders.Web.Services
{
    public class CouchbaseCrowdsourcedPointsService : ICrowdsourcedPointsService
    {
        public async Task<ICollection<Point>> GetClosePoint(GeoCoordinates point, double radius)
        {
            throw new NotImplementedException();
        }

        public async Task CreateCrowdsourcedPoint(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
