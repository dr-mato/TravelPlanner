using Microsoft.EntityFrameworkCore;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class HotelInfoTwoRepository : Repository<HotelInfoTwo>, IHotelInfoTwoRepository
    {
        public HotelInfoTwoRepository(DataDbContext context) : base(context)
        {
        }

        public async Task<List<HotelInfoTwo>> GetHotels(HotelListTwoRequest hotelRequirements)
        {
            var hotels = await _context.HotelInfoTwos
                .Where(h => h.City == hotelRequirements.City)
                .ToListAsync();

            var filteredHotels = hotels
                .Where(h =>
                    (hotelRequirements.Amenities == null || hotelRequirements.Amenities.All(a => h.Amenities.Contains(a))) &&
                    (hotelRequirements.Stars == null || hotelRequirements.Stars.All(s => h.Stars.Contains(s)))
                )
                .ToList();

            return filteredHotels;
        }

        public async Task SaveHotelInfoAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
