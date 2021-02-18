using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class BP
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string FederalTaxID { get; set; }
        public string Address { get; set; }
        public string BPType {get;set;}
        public string Currency { get; set; }
        public string DebtPayAccount { get; set; }
        
    }
}