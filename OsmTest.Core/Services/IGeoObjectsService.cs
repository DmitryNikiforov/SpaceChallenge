using System.Collections.Generic;
using GeoJSON.Net.Feature;

namespace OsmTest.Core.Services
{
    public interface IGeoObjectsService
    {
        ICollection<GeoJsonObject> GetRivers(Feature point, double radius);
    }
}