using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class AmenityService : IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateAmenity(Amenity amenity)
        {
            _unitOfWork.Amenity.Add(amenity);
            _unitOfWork.Save();
        }

        public void DeleteAmenity(Amenity amenity)
        {
            _unitOfWork.Amenity.Remove(amenity);
            _unitOfWork.Save();
        }

        public IEnumerable<Amenity> GetAllAmenities(bool shouldIncludeProperties = false)
        {
            return _unitOfWork.Amenity.GetAll(includeProperties: shouldIncludeProperties ? "Villa" : "");
        }

        public Amenity GetAmenityById(int id)
        {
            return _unitOfWork.Amenity.Get(x => x.Id == id);
        }

        public void UpdateAmenity(Amenity amenity)
        {
            _unitOfWork.Amenity.Update(amenity);
            _unitOfWork.Save();
        }
    }
}
