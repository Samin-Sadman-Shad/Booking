using Booking.Data;
using Booking.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly HotelContext _context;

        public RatingsController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> Get()
        {
            return await _context.Ratings.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> Get([FromRoute] int? id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating is null)
            {
                return NotFound();
            }
            return rating;
        }

        [HttpGet("{hotelId}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetByHotel([FromRoute] int? hotelId)
        {
            if (hotelId is null)
            {
                return BadRequest();
            }

            var rating = await _context.Ratings.Where(r => r.HotelId == hotelId).ToListAsync();
            if (rating is null)
            {
                return NotFound();
            }

            return rating;
        }



        [HttpPost]
        [ProducesResponseType<Rating>(StatusCodes.Status201Created)]
        public async Task<ActionResult<Rating>> Create(Rating? rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = rating.Id }, rating);
        }

        [HttpPut("{id}")]
        [ProducesResponseType<Rating>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Rating>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Rating>(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int? id, Rating rating)
        {
            if (id is null)
            {
                return BadRequest();
            }

            if (id !=rating.Id)
            {
                return BadRequest();
            }

            _context.Entry(rating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsRatingsExists(id))
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

        bool IsRatingsExists(int? id)
        {
            return _context.Ratings.Any(h => id == h.Id);
        }
    }
}
