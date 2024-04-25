using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Application.Services.Interface
{
    public interface IDashboardService
    {
        Task<RadialBarChartDto> GetTotalBookingRadialChartData();
        Task<RadialBarChartDto> GetRegisteredUsersRadialChartData();
        Task<RadialBarChartDto> GetRevenueRadialChartData();
        Task<PieChartDto> GetBookingPieChartData();
        Task<LineChartDto> GetMemberAndBookingLineChartData();
    }
}
