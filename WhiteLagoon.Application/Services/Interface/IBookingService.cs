using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface
{
    public interface IBookingService
    {
        void CreateBooking(Booking booking);
        Booking GetBookingById(int id);
        IEnumerable<Booking> GetAllBookings(string userId = "", string? statusFilterList = "");
        void UpdateStatus(int bookingId, string bookingStatus, int villaNumber);
        void UpdateStripePaymentId(int bookingId, string sessionId, string paymentIntentId);
        public IEnumerable<int> GetCheckedInVillaNumbers(int villaId);
    }
}
