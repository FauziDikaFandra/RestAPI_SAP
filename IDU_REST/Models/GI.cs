using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class GI_HEADER
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string PrimaryNumberingPOS { get; set; }
        public string DocumentNumberingPOS { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string Remarks { get; set; }
        public string Reference2 { get; set; }
        public List<GI_DETAIL> lines = new List<GI_DETAIL>();
    }
    public class GI_DETAIL
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int UomEntry { get; set; }
        public string UomCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string WarehouseCode { get; set; }
        public string AccountCode { get; set; }
        public string ProjectCode { get; set; }
        public string CostCenter { get; set; }
        public string WarehouseName { get; set; }
    }
}