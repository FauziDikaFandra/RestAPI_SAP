using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Logic
{
    public class Utils
    {
        public Recordset _IDU_Recordset(SAPbobsCOM.Company oCompany, string ssql){
            Recordset rs = null;

            rs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rs.DoQuery(ssql);

            return rs;
        }
    }
}