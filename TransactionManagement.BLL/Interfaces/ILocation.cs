using TransactionManagement.DTO.Models;

namespace TransactionManagement.BLL.Interfaces
{
    public interface ILocation
    {
        public Task<LocationDTO> GetLocationInfoAsync(string coordinates);
        public Task<string> GetCityAsync(double latitude, double longitude);
        public Task<string> GetTimeZoneAsync(double latitude, double longitude);
    }
}