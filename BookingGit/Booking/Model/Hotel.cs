using System.ComponentModel.DataAnnotations;

namespace Booking.Model
{
    public class Hotel
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }

        public ICollection<Rating>? Ratings { get; set; }
        public Price?  Price { get; set; }

        public Location? Location { get; set; }
    }
}
