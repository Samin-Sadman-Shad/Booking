using System.ComponentModel.DataAnnotations;

namespace Booking.Model
{
    public class Rating
    {
        public int Id { get; set; }
        [Range(1,5, ErrorMessage ="Rating should be in between 1 to 5")]
        public int? Value { get; set; }

        public Rate? Rate { get; set; }

        public string? Comment { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }

    public enum Rate
    {
        One,
        Two,
        Three,
        Four,
        Five
    }
}
