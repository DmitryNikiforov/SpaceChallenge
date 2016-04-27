using System.Collections.Generic;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using SpaceHerders.Models;

namespace SpaceHerders.Services
{
    public interface ICrowdsourcedPointsService
    {
        Task<ICollection<Place>> GetClosePoint(Point point, double radius);

        Task CreateCrowdsourcedPoint(Place place);
    }
}
