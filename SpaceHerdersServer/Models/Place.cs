using System;
using GeoJSON.Net.Geometry;

namespace SpaceHerders.Models
{
    public class Place
    {
        public Guid PlaceId { get; set; }

        public Point Geometry { get; set; }
    }
}
