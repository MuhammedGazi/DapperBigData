using HealthBigData.Dtos.DashboardDtos;
using HealthBigData.Dtos.DefaultDtos;

namespace HealthBigData.Repositories.ChartRepositories
{
    public interface IDashboardChartRepository
    {
        Task<DashboardFullDto> GetDashboardChartAsync();
    }
}
