using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieGEN.Models
{
    public class Ebay
    {
        public string itemId { get; set; }
        public string title { get; set; }
        public string globalId { get; set; }
        public primaryCategory primaryCategory { get; set; }
        public string galleryURL { get; set; }
        public string viewItemURL { get; set; }
        public List<string> paymentMethod { get; set; }
        public string autoPay { get; set; }
        public string location { get; set; }
        public string country { get; set; }
        public shippingInfo shippingInfo { get; set; }
        public sellingStatus sellingStatus { get; set; }
        public listingInfo listingInfo { get; set; }
        public string returnsAccepted { get; set; }
        public condition condition { get; set; }
        public string isMultiVariationListing { get; set; }
        public string topRatedListing { get; set; }
    }
}
