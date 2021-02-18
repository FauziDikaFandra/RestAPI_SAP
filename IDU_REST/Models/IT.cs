using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class IT_HEADER
    {
        public int DocEntry { get; set; }
        public string DocNum { get; set; }
        public Int32 Number { get; set; }
        public string Series { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string POSOutlet { get; set; }
        public string FromWarehouseCode { get; set; }
        public string FromWarehouseName { get; set; }
        public string ToWarehouseCode { get; set; }
        public string ToWarehouseName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Status { get; set; }
        public string ShipTo { get; set; }
        public string NoSPK { get; set; }
        public string NoSPKDetil { get; set; }
        public string SPKDetail { get; set; }
        public string NoSIP2 { get; set; }
        public DateTime TglSIP2 { get; set; }
        public string NoBAP { get; set; }
        public DateTime TglBAP { get; set; }
        public string Security { get; set; }
        public string Operator { get; set; }
        public string jam { get; set; }
        public string PetugasQA { get; set; }
        public string NoSPK2 { get; set; }
        public string UraianKerja { get; set; }
        public string NoInvoicePajak { get; set; }
        public float Downpayment { get; set; }
        public string DownpaymentAmount { get; set; }
        public decimal PriceList { get; set; }
        public string JournalRemarks { get; set; }
        public string SalesEmployee {get;set;}
        public decimal Quantity { get; set; }
        public string UOM { get; set; }

        public List<IT_DETAIL> lines = new List<IT_DETAIL>();
    }

    public class IT_DETAIL
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int UomEntry { get; set; }
        public string UomCode { get; set; }
        public double Quantity { get; set; }
        public int BaseEntry { get; set; }
        public int BaseLine { get; set; }
        public string ToWarehouseCode { get; set; }
        public string ToWarehouseName { get; set; }
    }
}