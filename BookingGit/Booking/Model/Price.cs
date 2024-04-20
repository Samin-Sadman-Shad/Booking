using System;

namespace Booking.Model
{
    public class Price
    {
        public int Id { get; set; }

        public Decimal AccommodationPrice { get; set; }
        public Decimal FoodPrice {  get; set; }
        
/*        public float Sports { get; set; }
        public float Casino { get; set; }
        public float Music { get; set; }*/

        public Decimal ServiceCharge { get; set; }

        public Hotel Hotel { get; set; }
    }
}
