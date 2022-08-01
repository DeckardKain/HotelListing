using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using AutoMapper;
using HotelListing.API.Contracts;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICountriesRespository _countriesRespository;

        public CountriesController(IMapper mapper, ICountriesRespository countriesRespository)
        {
            
            this._mapper = mapper;
            this._countriesRespository = countriesRespository;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDTO>>> GetCountries()
        {
            var countries = await _countriesRespository.GetAllAsync();
            var records = _mapper.Map<List<GetCountryDTO>>(countries);

            /*return await _context.Countries.ToListAsync();*/
            return Ok(records);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {
            /*var country = await _context.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);*/

            var country = await _countriesRespository.GetDetails(id);

            if (country == null)
            {
                return NotFound();
            }

            var countryDto = _mapper.Map<CountryDTO>(country);

            return Ok(countryDto);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDTO updateCountryDTO)
        {
            if (id != updateCountryDTO.Id)
            {
                return BadRequest();
            }

            /*_context.Entry(updateCountryDTO).State = EntityState.Modified;*/

            /*var country = await _context.Countries.FindAsync(id);*/

            var country = await _countriesRespository.GetAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCountryDTO, country); // This will then update the country from the DTO to the country in the DB.

            try
            {
                /*await _context.SaveChangesAsync();*/ // this will now save the changes made in line 73 with the mapper.
                await _countriesRespository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDTO createCountryDTO)
        {
            /*var country = new Country
            {
                Name = createCountry.Name,
                ShortName = createCountry.ShortName
            };*/

            var country = _mapper.Map<Country>(createCountryDTO);

            /* _context.Countries.Add(country);
             await _context.SaveChangesAsync();*/

            await _countriesRespository.AddAsync(country);            

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            /*var country = await _context.Countries.FindAsync(id);*/
            var country = await _countriesRespository.GetAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            /*          _context.Countries.Remove(country);
                      await _context.SaveChangesAsync();*/

            await _countriesRespository.DeleteAsync(id);

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            /*return _context.Countries.Any(e => e.Id == id);*/

            return _countriesRespository.Exists(id).Result; // ask Trevoir about this. I am bypassing calling it async. is that OK?
        }
    }
}
