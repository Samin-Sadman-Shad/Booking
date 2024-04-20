namespace Booking.Model
{
    public class Price
    {
        public int Id { get; set; }

        public float Accommodation { get; set; }
        public float Food {  get; set; }
        
        public float Sports { get; set; }
        public float Casino { get; set; }
        public float Music { get; set; }

        public float ServiceCharge { get; set; }

        public Hotel Hotel { get; set; }
    }
}
