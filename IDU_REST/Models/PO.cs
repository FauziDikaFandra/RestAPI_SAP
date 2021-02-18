using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class PO_HEADER
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string PrimaryNumberingPOS { get; set; }
        public string DocumentNumberingPOS { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string POSOutlet { get; set; }

        public List<PO_DETAIL> lines = new List<PO_DETAIL>();
    }

    public class PO_DETAIL
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
    }
}