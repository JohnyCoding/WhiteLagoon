﻿using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        private readonly ApplicationDbContext _context;

        public AmenityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Amenity entity)
        {
            _context.Amenities.Update(entity);
        }
    }
}
