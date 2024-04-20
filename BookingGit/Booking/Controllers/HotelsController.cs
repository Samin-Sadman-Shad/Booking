using Booking.Data;
using Booking.Model;
using Booking.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using System;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly HotelContext _context;

        public HotelsController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> Get()
        {
            return await _context.Hotels.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> Get([FromRoute]int? id)
        {
            var hotel =  await _context.Hotels.FindAsync(id);
            if(hotel is null)
            {
                return NotFound();
            }
            return hotel;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Hotel>>> FilterByNameAndDesciption([FromQuery] string name = null, [FromQuery] string description = null)
        {
            if(String.IsNullOrEmpty(name) && String.IsNullOrEmpty(description))
            {
                return BadRequest();
            }
            var hotels = await _context.Hotels.Where(h => h.Name.Contains(name) || h.Description.Contains(description)).ToListAsync();
            return hotels;
        }


        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Hotel>>> FilterHotels([FromQuery]string city, [FromQuery] decimal? maxPrice, [FromQuery] Rate? minRating)
        {
            IQueryable<Hotel> query = _context.Hotels;

            if (!string.IsNullOrEmpty(city))
            {
                query = query
                    .Where(h => h.Location.City.Name.ToLower().Contains(city.ToLower()));
            }

            if (maxPrice.HasValue)
            {
                query = query
                    .Where(h => h.Price != null)
                    .Where(h => h.Price.AccommodationPrice + h.Price.FoodPrice + h.Price.ServiceCharge <= maxPrice);
            }

            if (minRating.HasValue)
            {
                query = query
                    .Where(h => h.Ratings.Any(r => r.Rate >= minRating));
            }

            return await query.ToListAsync();
        }


        [HttpGet("nearest")]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotelsByLocation([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double distance)
        {
            var nearests = await _context.Hotels.
                Select(h => new 
                { 
                    Hotel = h, 
                    Distance = GeoCalculator.CalculateDistance(latitude, longitude, h.Location.Latitude, h.Location.Longitude) 
                })
                .Where(x => x.Distance < distance)
                .OrderBy(x => x.Distance)
                .Select(x => x.Hotel)
                .ToListAsync();

            return nearests;
        }


        [HttpPost]
        [ProducesResponseType<Hotel>(StatusCodes.Status201Created)]
        public async Task<ActionResult<Hotel>> Create([Bind("Id, Name, Description")]Hotel? hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new {id = hotel.Id}, hotel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType<Hotel>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Hotel>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Hotel>(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int? id, [Bind("Name, Description")] Hotel hotel)
        {
            if(id is null)
            {
                return BadRequest();
            }

            if(id != hotel.Id)
            {
                return BadRequest();
            }

            _context.Entry(hotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsHotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        bool IsHotelExists(int? id)
        {
            return _context.Hotels.Any(h => id == h.Id);
        }

    }
}
