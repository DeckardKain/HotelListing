using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class HotelsRespository : GenericRespository<Hotel>, IHotelsRespository
    {
        private readonly HotelListingDbContext _context;

        public HotelsRespository(HotelListingDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
