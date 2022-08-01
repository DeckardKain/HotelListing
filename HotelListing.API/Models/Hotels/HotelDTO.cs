﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Models.Hotels
{
    public class HotelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        [ForeignKey(nameof(CountryId))]
        public int CountryId { get; set; }

    }
}