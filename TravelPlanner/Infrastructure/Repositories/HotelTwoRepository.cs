using Microsoft.EntityFrameworkCore;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Infrastructure.Data;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class HotelTwoRepository : Repository<HotelTwo>, IHotelTwoRepository
    {
        public HotelTwoRepository(DataDbContext context) : base(context)
        {
        }

        public async Task<List<HotelTwo>> GetHotels(HotelTwoRequest info)
        {
            return await _context.HotelTwos.Where(hotel =>
                           hotel.City == info.City &&
                                          hotel.NumberOfPeople == info.NumberOfPeople &&
                                                         hotel.CheckInDate == info.CheckInDate &&
                                                                        hotel.CheckOutDate == info.CheckOutDate
                                                                                   ).ToListAsync();
        }

        public async Task SaveHotelsAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
