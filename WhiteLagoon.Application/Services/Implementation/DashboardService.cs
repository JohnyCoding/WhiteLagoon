﻿using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        static int previousMonthYear = DateTime.Now.Month == 1 ? DateTime.Now.Year - 1 : DateTime.Now.Year;
        readonly DateTime previousMonthStartDate = new(previousMonthYear, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PieChartDto> GetBookingPieChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.BookingDate >= DateTime.Now.AddDays(-30) && (b.Status != SD.StatusPending || b.Status == SD.StatusCancelled));

            var customerWithOneBooking = totalBookings.GroupBy(b => b.UserId).Where(b => b.Count() == 1).Select(c => c.Key).ToList();

            int bookingsByNewCustomer = customerWithOneBooking.Count();
            int bookingsByReturningCustomer = totalBookings.Count() - bookingsByNewCustomer;

            PieChartDto pieChartDto = new()
            {
                Labels = ["New Customer Bookings", "Returning Customer Bookings"],
                Series = [bookingsByNewCustomer, bookingsByReturningCustomer]
            };

            var countByCurrentMonth = totalBookings.Count(b => b.BookingDate >= currentMonthStartDate && b.BookingDate <= DateTime.Now);
            var countByPreviousMonth = totalBookings.Count(b => b.BookingDate >= previousMonthStartDate && b.BookingDate <= currentMonthStartDate);

            return pieChartDto;
        }

        public async Task<LineChartDto> GetMemberAndBookingLineChartData()
        {
            var bookingData = _unitOfWork.Booking.GetAll(b => b.BookingDate >= DateTime.Now.AddDays(-30) && b.BookingDate.Date <= DateTime.Now)
                .GroupBy(b => b.BookingDate.Date)
                .Select(b => new
                {
                    DateTime = b.Key,
                    NewBookingCount = b.Count()
                });

            var customerData = _unitOfWork.User.GetAll(u => u.CreatedAt >= DateTime.Now.AddDays(-30) && u.CreatedAt.Date <= DateTime.Now)
                .GroupBy(u => u.CreatedAt.Date)
                .Select(u => new
                {
                    DateTime = u.Key,
                    NewCustomerCount = u.Count()
                });

            var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime,
                (booking, customer) => new
                {
                    booking.DateTime,
                    booking.NewBookingCount,
                    NewCustomerCount = customer.Select(c => c.NewCustomerCount).FirstOrDefault()
                });

            var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime,
                (customer, booking) => new
                {
                    customer.DateTime,
                    NewBookingCount = booking.Select(b => b.NewBookingCount).FirstOrDefault(),
                    customer.NewCustomerCount
                });

            var mergedData = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();

            var newBookingData = mergedData.Select(x => x.NewBookingCount).ToArray();
            var newCustomerData = mergedData.Select(x => x.NewCustomerCount).ToArray();
            var categories = mergedData.Select(x => x.DateTime.ToString("dd/MM/yyy")).ToArray();

            List<ChartData> chartDataList =
            [
                new ChartData
                {
                    Name = "New Bookings",
                    Data = newBookingData
                },
                new ChartData
                {
                    Name = "New Members",
                    Data = newCustomerData
                }
            ];
            LineChartDto lineChartDto = new()
            {
                Categories = categories,
                Series = chartDataList
            };

            return lineChartDto;
        }

        public async Task<RadialBarChartDto> GetRegisteredUsersRadialChartData()
        {
            var totalUsers = _unitOfWork.User.GetAll();

            var countByCurrentMonth = totalUsers.Count(u => u.CreatedAt >= currentMonthStartDate && u.CreatedAt <= DateTime.Now);
            var countByPreviousMonth = totalUsers.Count(u => u.CreatedAt >= previousMonthStartDate && u.CreatedAt <= currentMonthStartDate);

            return SD.GetRadialChartDataModel(totalUsers.Count(), countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<RadialBarChartDto> GetRevenueRadialChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.Status != SD.StatusPending || b.Status == SD.StatusCancelled);

            var totalRevenue = Convert.ToInt32(totalBookings.Sum(b => b.TotalCost));

            var countByCurrentMonth = totalBookings.Where(b => b.BookingDate >= currentMonthStartDate && b.BookingDate <= DateTime.Now).Sum(b => b.TotalCost);
            var countByPreviousMonth = totalBookings.Where(b => b.BookingDate >= previousMonthStartDate && b.BookingDate <= currentMonthStartDate).Sum(b => b.TotalCost);

            return SD.GetRadialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<RadialBarChartDto> GetTotalBookingRadialChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.Status != SD.StatusPending || b.Status == SD.StatusCancelled);

            var countByCurrentMonth = totalBookings.Count(b => b.BookingDate >= currentMonthStartDate && b.BookingDate <= DateTime.Now);
            var countByPreviousMonth = totalBookings.Count(b => b.BookingDate >= previousMonthStartDate && b.BookingDate <= currentMonthStartDate);

            return SD.GetRadialChartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth);
        }
    }
}
