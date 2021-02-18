using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class APINV_HEADER
    {
        public int series { get; set; }
        public string cardCode { get; set; }
        public DateTime postingDate { get; set; }
        public DateTime docDueDate { get; set; }
        public DateTime documentDate { get; set; }

        public List<APINV_DETAIL> lines = new List<APINV_DETAIL>();
    }

    public class APINV_DETAIL
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int UomEntry { get; set; }
        public string UomCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public int BaseEntry { get; set; }
        public int BaseLine { get; set; }
    }
}