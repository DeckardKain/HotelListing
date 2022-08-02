using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Hotels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHotelsRespository _hotelsRespository;

        public HotelsController(IMapper mapper, IHotelsRespository hotelsRespository)
        {
            this._mapper = mapper;
            this._hotelsRespository = hotelsRespository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetHotelDTO>>> GetHotels()
        {
            var hotels = await _hotelsRespository.GetAllAsync();
            var records = _mapper.Map<List<GetHotelDTO>>(hotels);
                       
            return Ok(records);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetHotelDTO>> GetHotel(int id)
        {
            var hotelDTO = await _hotelsRespository.GetAsync(id);

            if (hotelDTO == null)
            {
                return NotFound();
            }

            var hotel = _mapper.Map<Hotel>(hotelDTO);

            return Ok(hotel);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDTO hotelDTO)
        {
            if (id != hotelDTO.Id)
            {
                return BadRequest();
            }
            var hotel = await _hotelsRespository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            _mapper.Map(hotelDTO, hotel);

            try
            {
                /*await _context.SaveChangesAsync();*/ // this will now save the changes made in line 73 with the mapper.
                await _hotelsRespository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDTO createHotelDTO)
        {
            var hotel = _mapper.Map<Hotel>(createHotelDTO);

            await _hotelsRespository.AddAsync(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelsRespository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }
                       
            await _hotelsRespository.DeleteAsync(id);

            return NoContent();
        }


        private bool HotelExists(int id)
        {
            /*return _context.Countries.Any(e => e.Id == id);*/

            return _hotelsRespository.Exists(id).Result; // ask Trevoir about this. I am bypassing calling it async. is that OK?
        }
    }
}
