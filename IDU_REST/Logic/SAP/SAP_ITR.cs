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
    public class SAP_ITR
    {

        public static string AddData(Company oCompany, ITR model)
        {
            StockTransfer oObject = null;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();

                oObject = oCompany.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest);

                oObject.DocDate = model.PostingDate;
                oObject.DueDate = model.DocumentDate;
                oObject.ToWarehouse = model.ToWarehouseCode;
                oObject.FromWarehouse = model.FromWarehouseCode;
                oObject.Address = model.ShipTo;
                oObject.UserFields.Fields.Item("U_MJTPO").Value = model.NoSPK;
                oObject.UserFields.Fields.Item("U_MJTSPK").Value = model.NoSPKDetil;
                oObject.UserFields.Fields.Item("U_MJT_NoSIP2").Value = model.NoSIP2;
                oObject.UserFields.Fields.Item("U_MJT_TglSIP2").Value = model.TglSIP2;
                oObject.UserFields.Fields.Item("U_MJTBASTB").Value = model.NoBAP; 
                oObject.UserFields.Fields.Item("U_MJTRCVD").Value = model.TglBAP;
                oObject.UserFields.Fields.Item("U_mjt_opr").Value = model.Operator;
                oObject.UserFields.Fields.Item("U_mjt_scr").Value = model.Security;
                oObject.UserFields.Fields.Item("U_mjt_jam2").Value = model.jam;
                oObject.UserFields.Fields.Item("U_mjt_qa").Value = model.PetugasQA;
                oObject.UserFields.Fields.Item("U_MJTPO").Value = model.NoSPK2;
                oObject.UserFields.Fields.Item("U_MJT_Uraian").Value = model.UraianKerja;
                oObject.UserFields.Fields.Item("U_MJT_NomorINV").Value = model.NoInvoicePajak;
                oObject.UserFields.Fields.Item("U_MJT_DP").Value = model.Downpayment;
                oObject.UserFields.Fields.Item("U_MJT_DP2").Value = model.DownpaymentAmount;

                for (int i = 0; i < model.lines.Count; i++)
                {
                    oObject.Lines.ItemCode = model.lines[i].ItemCode;
                    oObject.Lines.FromWarehouseCode = model.FromWarehouseCode;
                    oObject.Lines.WarehouseCode = model.lines[i].ToWarehouseCode;
                    oObject.Lines.Quantity = model.lines[i].Quantity;

                    oObject.Lines.Add();
                }

                int addStatus = oObject.Add();

                if (addStatus == 0)
                {
                    if (strResult == "")
                    {
                        strResult = oCompany.GetNewObjectKey();
                    }
                    else
                    {
                        strResult = strResult + " | " + oCompany.GetNewObjectKey();
                    }
                }
                else
                {
                    if (oCompany.InTransaction)
                    {
                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                    }

                    oCompany.GetLastError(out errCode, out errMessage);
                    throw new Exception("Error Code : " + errCode + " | Error Message : " + errMessage);
                }
                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
            }
            catch (Exception)
            {
                if (oCompany.InTransaction)
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                };
                throw;
            }

            return strResult;
        }
        public List<ITR> GetData(Company oCompany, Int32 DocEntry)
        {
            Utils control = new Utils();
            List<ITR> details = new List<ITR>();
            XDocument xDoc = new XDocument();
            XDocument xDocDetail = new XDocument();

            string sql = "SELECT T0.DocNum as  'Number',T1.SeriesName,T0.DocDate as 'PostingDate',T0.TaxDate as 'DocumentDate',T0.Filler as 'FromWhsCode'," +
                         " T2.WhsName as 'FromWhsName', T0.ToWhsCode, T3.WhsName as 'ToWhsName',T4.ItemCode,T4.Dscription as 'ItemName',isnull(T5.Price,0) as 'PriceList'," +
                         "T0.JrnlMemo as 'JournalRemarks',T0.Address as 'ShipTo' ,isnull(T5.CalcPrice,0) as 'ItemCost',T4.Quantity,T6.UomName,T4.LineNum " +
                         " FROM OWTQ T0 INNER JOIN NNM1 T1 on T1.Series = T0.Series INNER JOIN OWHS T2 on T2.WhsCode = T0.Filler INNER JOIN OWHS T3 on T3.WhsCode = T0.ToWhsCode " +
                         " INNER JOIN WTQ1 T4 on T4.DocEntry = T0.DocEntry  LEFT JOIN OINM T5 on T5.CreatedBy = T0.DocEntry and T5.TransType = T0.ObjType and T5.ItemCode = T4.ItemCode and T5.DocLineNum = T4.LineNum " +
                         " LEFT JOIN OUOM T6 on T6.UomEntry = T4.UomEntry where T0.Docentry = '" + DocEntry + "'";

            Recordset rs = control._IDU_Recordset(oCompany, sql);
            xDoc = XDocument.Parse(rs.GetAsXML());

            List<ITR> model = (from item in xDoc.Element("BOM").Element("BO").Element("OWTQ").Elements("row")
                               select new ITR
                               {
                                   Number = Convert.ToInt32(item.Element("Number").Value),
                                   Series = item.Element("SeriesName").Value,
                                   PostingDate = DateTime.ParseExact(item.Element("PostingDate").Value, "yyyyMMdd",
                                                 CultureInfo.InvariantCulture, DateTimeStyles.None),
                                   DocumentDate = DateTime.ParseExact(item.Element("DocumentDate").Value, "yyyyMMdd",
                                                   CultureInfo.InvariantCulture, DateTimeStyles.None),

                                   FromWarehouseCode = item.Element("FromWhsCode").Value,
                                   FromWarehouseName = item.Element("FromWhsName").Value,
                                   ToWarehouseCode = item.Element("ToWhsCode").Value,
                                   ToWarehouseName = item.Element("ToWhsName").Value,
                                   LineNum = Convert.ToInt32(item.Element("LineNum").Value),
                                   ItemCode = item.Element("ItemCode").Value,
                                   ItemName = item.Element("ItemName").Value,
                                   Quantity = Convert.ToDecimal(item.Element("Quantity").Value),
                                   ShipTo = item.Element("ShipTo").Value,
                                   JournalRemarks = item.Element("JournalRemarks").Value,
                                   UOM = item.Element("UomName").Value
                               }
                                ).ToList();
            return model;
            
        }
        public List<ITR> GetData(Company oCompany, DateTime FromDate, DateTime ToDate)
        {

            Utils control = new Utils();
            List<ITR> details = new List<ITR>();
            XDocument xDoc = new XDocument();
            XDocument xDocDetail = new XDocument();

            string sql = "SELECT T0.DocEntry, T0.DocNum as  'Number',T1.SeriesName,T0.DocDate as 'PostingDate',T0.TaxDate as 'DocumentDate',T0.Filler as 'FromWhsCode'," +
                         " T2.WhsName as 'FromWhsName', T0.ToWhsCode, T3.WhsName as 'ToWhsName',T4.ItemCode,T4.Dscription as 'ItemName',isnull(T5.Price,0) as 'PriceList'," +
                         "T0.JrnlMemo as 'JournalRemarks',T0.Address as 'ShipTo',T6.SlpName,T0.SlpCode,T4.LineNum " +
                         " FROM OWTQ T0 INNER JOIN NNM1 T1 on T1.Series = T0.Series INNER JOIN OWHS T2 on T2.WhsCode = T0.Filler INNER JOIN OWHS T3 on T3.WhsCode = T0.ToWhsCode " +
                         " INNER JOIN WTQ1 T4 on T4.DocEntry = T0.DocEntry  LEFT JOIN ITM1 T5 on T5.ItemCode = T4.ItemCode and T5.PriceList = 1 LEFT JOIN OSLP T6 on T6.SlpCode = T0.SlpCode " +
                         " where T0.DocDate Between '" + FromDate + "' and '" + ToDate + "'";
                    
            Recordset rs = control._IDU_Recordset(oCompany, sql);
            xDoc = XDocument.Parse(rs.GetAsXML());
  
                List<ITR> model = (from item in xDoc.Element("BOM").Element("BO").Element("OWTQ").Elements("row")
                                 select new ITR 
                                    {
                                        DocEntry = Convert.ToInt32(item.Element("DocEntry").Value),
                                        Number = Convert.ToInt32(item.Element("Number").Value),
                                        Series = item.Element("SeriesName").Value,
                                        PostingDate = DateTime.ParseExact(item.Element("PostingDate").Value, "yyyyMMdd",
                                                      CultureInfo.InvariantCulture, DateTimeStyles.None),
                                        DocumentDate = DateTime.ParseExact(item.Element("DocumentDate").Value, "yyyyMMdd",
                                                        CultureInfo.InvariantCulture, DateTimeStyles.None),

                                        FromWarehouseCode = item.Element("FromWhsCode").Value,
                                        FromWarehouseName = item.Element("FromWhsName").Value,
                                        ToWarehouseCode = item.Element("ToWhsCode").Value,
                                        ToWarehouseName = item.Element("ToWhsName").Value,
                                        LineNum = Convert.ToInt32(item.Element("LineNum").Value),
                                        ItemCode = item.Element("ItemCode").Value,
                                        ItemName = item.Element("ItemName").Value,
                                        PriceList = Convert.ToDecimal(item.Element("PriceList").Value),
                                        ShipTo = item.Element("ShipTo").Value,
                                        SalesEmployee =  item.Element("SlpName").Value,
                                        JournalRemarks =item.Element("JournalRemarks").Value
                                   
                                    }
                                    ).ToList();
                 return model;
            
        }
    }
}