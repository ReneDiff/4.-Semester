using System;

namespace ElSpot.Models
{
    public class PriceEntry
    {
        public DateTime HourDK { get; set; }
        public string? PriceArea { get; set; }
        public double SpotPriceDKK { get; set; }
    }

}