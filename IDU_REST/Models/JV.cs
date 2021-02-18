using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class JV
    {
        public string TranId { get; set; }
        public DateTime Refdate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string Remarks { get; set; }
        public string ProjectId {get; set;}
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
        public string MaximoId { get; set; }
        public string MaximoNumber { get; set; }

        public List<JV_Detil> Lines = new List<JV_Detil>();

    }
    public class JV_Detil
    {
        public string AccountCode { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string CostCenter { get; set; }
        public string TaxId { get; set; }
        public string FCCurrency { get; set; }
        public decimal DebitAmountFC { get; set; }
        public decimal CreditAmountFC { get; set; }
        public string LineMemo { get; set; }
        public string ref1 { get; set; }
        public string ref2 { get; set; }
        public string ref3 { get; set; }
    }
}