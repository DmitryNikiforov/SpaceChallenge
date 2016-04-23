using GeoJSON.Net.Feature;

namespace OsmTest.Core.Services
{
    public interface IGeoObjectsService
    {
        FeatureCollection GetCloseUsers(Feature point, double radius);
    }
}