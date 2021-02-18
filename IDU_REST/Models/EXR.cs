using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class EXR
    {
        public string Currency { get; set; }
        public double Rate { get; set; }
        public DateTime DateRate { get; set; }
        public string RateDate { get; set; }
    }
}