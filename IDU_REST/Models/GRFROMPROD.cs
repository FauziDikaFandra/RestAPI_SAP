using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class GRFROMPROD
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string OtherID { get; set; }
        public string OtherNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string POSOutlet { get; set; }
        public string Reference2 { get; set; }
        public string Remarks { get; set; }

        public List<GRFROMPROD_DETIL> Detil = new List<GRFROMPROD_DETIL>();
    }
    public class GRFROMPROD_DETIL
    {
        public int BaseLine{ get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int UomEntry { get; set; }
        public string UomCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public int BaseEntry {get;set;}
        public int BasetType { get; set;}
    }
}