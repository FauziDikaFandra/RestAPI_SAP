using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
   
        public class DO_HEADER
        {
            public string DocEntry { get; set; }
            public string DocNum { get; set; }
            public int Series { get; set; }
            public string SeriesName { get; set; }
            public string CardCode { get; set; }
            public string CardName { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public DateTime PostingDate { get; set; }
            public DateTime DocumentDate { get; set; }
            public DateTime DocDueDate { get; set; }
            public DateTime TaxDate { get; set; }
            public double TotalBeforeDiscount { get; set; }
            public double Doctotal { get; set; }
            public string SalesPersonCode { get; set; }
            public string SalesPersonName { get; set; }
            public double TotalTransaction { get; set; }
            public string Remarks { get; set; }
            public string ContactPersonCode { get; set; }
            public string ContactPersonName { get; set; }
            public string CustomerRefNumber { get; set; }
           
            public List<DO_DETAIL> Lines = new List<DO_DETAIL>(); 
        }
        public class DO_DETAIL
        {

            public int LineNum { get; set; }
            public string ItemCode { get; set; }
            public string Description { get; set; }
            public string ItemName { get; set; }
            public int UomEntry { get; set; }
            public string UOMName { get; set; }
            public string UomCode { get; set; }
            public double Quantity { get; set; }
            public double Price { get; set; }
            public string WarehouseCode { get; set; }
            public string WarehouseName { get; set; }
            public double DiscountPercent { get; set; }
            public double DiscountSum { get; set; }
            public double LineTotal { get; set; }
            public string CostCenterCode { get; set; }
            public string CostCenterName { get; set; }
            public string ProjectCode { get; set; }
            public string ProjectName { get; set; }

        }
    
}