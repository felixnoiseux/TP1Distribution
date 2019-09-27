using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieGEN.Models
{
    public class sellingStatus
    {
        public Dictionary<string, string> currentPrice { get; set; }
        public Dictionary<string, string> convertedCurrentPrice { get; set; }

        public string sellingState { get; set; }
        public string timeLeft { get; set; }
    }
}
