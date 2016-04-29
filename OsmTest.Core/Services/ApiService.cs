using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using OsmSharp;
using OsmSharp.Math.Geo;
using OsmTest.Core.Models;

namespace OsmTest.Core.Services
{
    public interface IApiService
    {
        int GetRainChance(GeoCoordinate point);
    }

    public class ApiService : IApiService
    {
        private const string WeatherUrlFormat = @"http://api.wunderground.com/api/08bd3e5bb4fe996c/conditions/forecast/alert/q/{0},{1}.json";

        public int GetRainChance(GeoCoordinate point)
        {
            RootWeatherObject rootWeatherObject;
            var uri = string.Format(WeatherUrlFormat, point.Latitude, point.Longitude);
            var request = WebRequest.CreateHttp(uri);
            using (var responseStream = request.GetResponse().GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var converter = new JsonSerializer();
                rootWeatherObject = converter.Deserialize<RootWeatherObject>(jsonReader);
            }

            return rootWeatherObject?.forecast.simpleforecast.forecastday.First().pop ?? 0;
        }
        
        
    }
}
