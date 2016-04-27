using System;
using GeoJSON.Net.Geometry;

namespace SpaceHerders.Models
{
    public class Place
    {
        public Guid PlaceId { get; set; }

        public Point Point { get; set; }

        public string Description { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
