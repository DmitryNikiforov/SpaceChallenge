using System.Collections.Generic;
using System.Threading.Tasks;
using SpaceHerders.Web.Models;

namespace SpaceHerders.Web.Services
{
    public interface ICrowdsourcedPointsService
    {
        Task<ICollection<Point>> GetClosePoint(GeoCoordinates point, double radius);

        Task CreateCrowdsourcedPoint(Point point);
    }
}
