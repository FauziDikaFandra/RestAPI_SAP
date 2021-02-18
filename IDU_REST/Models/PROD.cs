using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class PROD
    {
        public string FormNumber {get;set;}
        public int DocEntry {get;set;}
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public string warehouse { get; set; }
        public string ProductNo { get; set; }
        public string ProductDescription { get; set; }
        public decimal plannedQty { get; set; }
        public string UOM { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Remarks { get; set; }
        public List<PROD_DETIL> Detil = new List<PROD_DETIL>();
    }

    public class PROD_DETIL
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int LineNum { get; set; }
        public decimal BaseQty { get; set; }
        public decimal PlannedQty { get; set; }
        public decimal IssueQty { get; set; }
        public string IssueType { get; set; }
        public string warehouse { get; set; }
        public string project { get; set; }

    }

    
}