using IDU_REST.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace IDU_REST.Logic.SAP
{
    public class SAP_GetListStock
    {
        public static List<GetListStock> GetData(SAPbobsCOM.Company oCompany, string ItemGroup, DateTime UpdateFrom, DateTime UpdateTo )
        {
            Utils control = new Utils();
            XDocument xDoc = new XDocument();
            string sql = "SELECT T1.WhsCode,T2.ItmsGrpNam as 'ItemGroup',T0.ItemCode as 'ItemNo',T0.ItemName as 'ItemDescription', " +
                         "T0.INUoMEntry as 'UOM',T1.OnHand as 'InStock',T1.IsCommited as 'Commited',T1.OnOrder as 'Ordered', " +
                         "(T1.OnHand + T1.OnOrder - T1.IsCommited) as 'Available',  T1.AvgPrice as 'ItemPrice',((T1.OnHand + T1.OnOrder - T1.IsCommited) * isnull(T1.AvgPrice,0)) as 'Total' " +
                         " FROM  OITM T0" +
                         " LEFT JOIN OITW T1 on T1.ItemCode = T0.ItemCode" +
                         " LEFT JOIN OITB T2 on T2.ItmsGrpCod = T2.ItmsGrpCod " +
                         " Where T2.ItmsGrpNam = '" + ItemGroup + "' and T0.UpdateDate between '"+ UpdateFrom +"' and '"+ UpdateTo +"'";
            Recordset rs = control._IDU_Recordset(oCompany, sql);
            xDoc = XDocument.Parse(rs.GetAsXML());

            List<GetListStock> List = (from data in xDoc.Element("BOM").Element("BO").Element("OITM").Elements("row")
                                       select new GetListStock
                                       {
                                           WhsCode = data.Element("WhsCode").Value,
                                           ItemGroup = data.Element("ItemGroup").Value,
                                           ItemNo = data.Element("ItemNo").Value,
                                           ItemDescription = data.Element("ItemDescription").Value,
                                           UOM = data.Element("UOM").Value,
                                           InStock = Convert.ToDecimal(data.Element("InStock").Value),
                                           Commited = Convert.ToDecimal(data.Element("Commited").Value),
                                           Ordered = Convert.ToDecimal(data.Element("Ordered").Value),
                                           Available = Convert.ToDecimal(data.Element("Available").Value),
                                           ItemPrice = Convert.ToDecimal(data.Element("ItemPrice").Value),
                                           Total = Convert.ToDecimal(data.Element("Total").Value)
                                       }).ToList();
            return List;

        
        }
    }
}