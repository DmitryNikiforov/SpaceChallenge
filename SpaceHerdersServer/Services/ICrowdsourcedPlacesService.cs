using System.Collections.Generic;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using SpaceHerders.Models;

namespace SpaceHerders.Services
{
    public interface ICrowdsourcedPlacesService
    {
        Task<ICollection<CrowdsourcedPlace>> GetClosePlaces(Point point, double radius);

        Task<bool> CreateCrowdsourcedPoint(CrowdsourcedPlace place);
    }
}
