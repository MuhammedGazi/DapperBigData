using HealthBigData.Dtos.DefaultDtos;

namespace HealthBigData.Repositories.HastaRepositories
{
    public interface ICityRepository
    {
        Task<CityDescriptionDto> GetCityAsync(string cityName);
    }
}
