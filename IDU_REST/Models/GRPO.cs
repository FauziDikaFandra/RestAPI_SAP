using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class GRPO_HEADER
    {
        public string DocEntry { get; set; }
        public string NumberForm { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime DueDate { get; set; }
 
 

        public List<GRPO_DETAIL> lines = new List<GRPO_DETAIL>();
    }

    public class GRPO_DETAIL
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string UOM { get; set; }
        public string UomCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string GLAccount { get; set; }
        public string CostCenter { get; set; }
        public decimal  Total { get; set; }
        public string Currency { get; set; } 
        public int BaseEntry { get; set; }
        public int BaseLine { get; set; }
    }
}