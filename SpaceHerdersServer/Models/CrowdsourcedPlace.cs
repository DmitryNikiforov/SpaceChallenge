using System;

namespace SpaceHerders.Models
{
    public class CrowdsourcedPlace : Place
    {
        public CrowdsourcedPlaceType CrowdsourcedPlaceType { get; set; }

        public Guid CreatorId { get; set; }
    }
}
