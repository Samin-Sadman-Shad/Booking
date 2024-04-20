using Booking.Model;
using Microsoft.EntityFrameworkCore;

namespace Booking.Data
{
    public class HotelContext:DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options) 
        {

        }

        public DbSet<Hotel> Hotels { get; set; } = null;
        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }

    }
}
