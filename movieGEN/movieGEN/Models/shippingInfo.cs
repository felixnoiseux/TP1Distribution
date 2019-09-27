using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieGEN.Models
{
    public class shippingInfo
    {
        public Dictionary<string, string> shippingServiceCost { get; set; }
        public string shippingType { get; set; }
        public string shipToLocations { get; set; }
        public string expeditedShipping { get; set; }
        public string oneDayShippingAvailable { get; set; }
        public string handlingTime { get; set; }
    }
}
