using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface
{
    public interface IAmenityService
    {
        IEnumerable<Amenity> GetAllAmenities(bool shouldIncludeProperties);
        Amenity GetAmenityById(int id);
        void CreateAmenity(Amenity amenity);
        void UpdateAmenity(Amenity amenity);
        void DeleteAmenity(Amenity amenity);
    }
}
