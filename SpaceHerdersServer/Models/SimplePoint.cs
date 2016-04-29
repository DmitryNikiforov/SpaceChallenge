namespace SpaceHerders.Models
{
    public class SimplePoint
    {
        public SimplePoint(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public double Longitude { get; }

        public double Latitude { get; }
    }
}
