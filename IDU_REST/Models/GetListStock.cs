using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Models
{
    public class GetListStock
    {
        public string WhsCode { get; set; }
        public string ItemGroup { get; set; }
        public string ItemNo { get; set; }
        public string ItemDescription { get; set; }
        public string UOM { get; set; }
        public decimal InStock { get; set; }
        public decimal Commited { get; set; }
        public decimal Ordered { get; set; }
        public decimal Available { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal Total { get; set; }

    }
}