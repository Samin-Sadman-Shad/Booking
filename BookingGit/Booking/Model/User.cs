namespace Booking.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Rating> Rating { get; set; }
    }
}
