using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmTest.Core.Models
{
    public class Point
    {
        public List<double> coordinates { get; set; }
        public int type => 0;
    }

    public class Coordinates
    {
        public int CrowdsourcedPlaceType { get; set; }
        public Guid CreatorId { get; set; }
        public Point Point { get; set; }
        public string Description { get; set; }
    }
}
