using System;

namespace ElSpot.Models{    
    public class ElSpotPrices
    {
        public long total { get; set; }
        public string? filters { get; set; }
        public int limit { get; set; }
        public string? dataset { get; set; }
        public PriceEntry[]? records { get; set; }
    }
}