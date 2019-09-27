using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieGEN.Models
{
    public class listingInfo
    {
        public string bestOfferEnabled { get; set; }
        public string buyItNowAvailable { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string listingType { get; set; }
        public string gift { get; set; }
        public string watchCount { get; set; }
    }
}
