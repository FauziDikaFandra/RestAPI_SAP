using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class PR_HEADER
    {
        public int series { get; set; }
        public int reqType { get; set; }
        public string requester { get; set; }
        public DateTime postingDate { get; set; }
        public DateTime docDueDate { get; set; }
        public DateTime documentDate { get; set; }
        public DateTime reqDate { get; set; }

        public List<PR_DETAIL> lines = new List<PR_DETAIL>();
    }

    public class PR_DETAIL
    {
        public string itemCode { get; set; }
        public int requiredQuantity { get; set; }
        public double infoPrice { get; set; }
        public string taxCode { get; set; }
        public string costCenter { get; set; }
        public string siteID { get; set; }
        public string siteName { get; set; }
        public string vendor { get; set; }
    }
}