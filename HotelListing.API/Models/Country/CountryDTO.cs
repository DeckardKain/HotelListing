﻿using HotelListing.API.Models.Hotels;

namespace HotelListing.API.Models.Country
{
    public class CountryDTO : BaseCountryDTO
    {
        public int Id { get; set; }
        public List<HotelDTO> Hotels { get; set; }
    }
}
