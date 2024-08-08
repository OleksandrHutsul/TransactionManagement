using System.Globalization;
using Newtonsoft.Json.Linq;
using TransactionManagement.BLL.Interfaces;
using TransactionManagement.DTO.Models;

namespace TransactionManagement.BLL.Services
{
    public class LocationService : ILocation
    {
        private readonly HttpClient _httpClient;
        private readonly string _geoNamesUsername;

        public LocationService(HttpClient httpClient, string geoNamesUsername)
        {
            _httpClient = httpClient;
            _geoNamesUsername = geoNamesUsername;
        }

        public async Task<LocationDTO> GetLocationInfoAsync(string coordinates)
        {
            var coordinateParts = coordinates.Split(',');
            if (coordinateParts.Length != 2)
            {
                return null;
            }

            var latitude = double.Parse(coordinateParts[0].Trim(), CultureInfo.InvariantCulture);
            var longitude = double.Parse(coordinateParts[1].Trim(), CultureInfo.InvariantCulture);

            var city = await GetCityAsync(latitude, longitude);
            var timeZone = await GetTimeZoneAsync(latitude, longitude);

            return new LocationDTO
            {
                IANAZone = city,
                UTC = timeZone,
                Coordinates = coordinates
            };
        }

        public async Task<string> GetCityAsync(double latitude, double longitude)
        {
            var url = $"http://api.geonames.org/timezoneJSON?lat={latitude.ToString(CultureInfo.InvariantCulture)}&lng={longitude.ToString(CultureInfo.InvariantCulture)}&username={_geoNamesUsername}";
            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);
            var timeZoneId = json["timezoneId"]?.ToString(); 
            if (timeZoneId is null)
            {
                return "Not found time";
            }
            return timeZoneId;
        }

        public async Task<string> GetTimeZoneAsync(double latitude, double longitude)
        {
            var url = $"http://api.geonames.org/timezoneJSON?lat={latitude.ToString(CultureInfo.InvariantCulture)}&lng={longitude.ToString(CultureInfo.InvariantCulture)}&username={_geoNamesUsername}";
            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);
            var rawOffset = json["rawOffset"]?.ToString();
            if (rawOffset is null)
            {
                return "Not found IANA time zone";
            }
            return rawOffset;
        }
    }
}