using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class ARBA
    {
        public string bpCode { get; set; }
        public string siteID { get; set; }
        public string siteName { get; set; }
        public string siteOp { get; set; }
        public string remarks { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int plannedQuantity { get; set; }
        public double unitPrice { get; set; }
    }
}