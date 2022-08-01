using HotelListing.API.Data;

namespace HotelListing.API.Contracts
{
    public interface ICountriesRespository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);
    }
}
