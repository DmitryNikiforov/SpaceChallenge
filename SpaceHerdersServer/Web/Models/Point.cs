using System;

namespace SpaceHerders.Web.Models
{
    public class Point
    {
        public PointType PointType { get; set; }

        public GeoCoordinates Location { get; set; }

        public Guid CreatorId { get; set; }
    }
}
