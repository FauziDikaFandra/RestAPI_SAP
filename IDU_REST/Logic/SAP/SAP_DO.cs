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
    public class SAP_DO
    {
        public static DO_HEADER GetData(SAPbobsCOM.Company oCompany, int id)
        {
            Utils control = new Utils();
            List<DO_DETAIL> details = new List<DO_DETAIL>();
            XDocument xDoc = new XDocument();
            XDocument xDocDetail = new XDocument();

            string sql = "select T0.DocEntry,T0.DocNum ,T0.Series,T1.SeriesName,T0.DocDate,T0.DocDueDate,T0.TaxDate,T0.CardCode,T0.CardName, " +
                "(select sum(TA.LineTotal) from DLN1 TA where TA.DocEntry = T0.DocEntry) as 'TotalBeforeDiscount',T0.DocTotal,T0.SlpCode,T2.SlpName as 'SalesEmploye', T0.Comments as 'Remarks' " +
                " ,T0.CntctCode as 'ContactPersonCode',T3.[Name] as 'ContactPersonName'  " +
                "FROM ODLN T0  INNER JOIN NNM1 T1 on T1.Series = T0.Series LEFT JOIN OSLP T2 on T2.SlpCode = T0.SlpCode  LEFT JOIN OCPR T3 on T3.CntctCode = T0.CntctCode" +
                " Where T0.DocEntry = " +id;

            Recordset rs = control._IDU_Recordset(oCompany, sql);
            xDoc = XDocument.Parse(rs.GetAsXML());

            sql = "select T0.LineNum,T0.ItemCode,T0.Dscription as 'ItemName',T0.Quantity,T0.Price,T0.DiscPrcnt,(T0.PriceBefDi * T0.DiscPrcnt) as 'DiscSum',T0.LineTotal, " +
                  " T0.WhsCode as 'WarehouseCode',T4.WhsName 'WarehouseName',T1.UomEntry as 'UOMCode',T1.UomName as 'UOMName', " +
                  " T0.OcrCode,T2.OcrName,T0.Project,T3.PrjName " +
                  " FROM DLN1 T0 LEFT JOIN OUOM T1 on T0.UomEntry = T0.UomEntry LEFT JOIN OOCR T2 on T2.OcrCode = T0.OcrCode LEFT JOIN OPRJ T3 on T3.PrjCode = T0.Project" +
                  " LEFT JOIN OWHS T4 on T4.WhsCode = T0.WhsCode " +
                  " WHERE T0.DocEntry =  " + id;
      
            rs = control._IDU_Recordset(oCompany, sql);
            xDocDetail = XDocument.Parse(rs.GetAsXML());

            XElement xEle = xDoc.Element("BOM").Element("BO").Element("ODLN").Element("row");
            XElement xEleDetail = xDocDetail.Element("BOM").Element("BO").Element("DLN1");

            foreach (var item in xEleDetail.Elements("row"))
            {
                DO_DETAIL model_detail = new DO_DETAIL()
                {
                    LineNum = Convert.ToInt32(item.Element("LineNum").Value),
                    ItemCode = item.Element("ItemCode").Value,
                    ItemName = item.Element("ItemName").Value,
                    UomEntry = Convert.ToInt32(item.Element("UOMCode").Value),
                    UomCode = item.Element("UOMName").Value,
                    Quantity = Convert.ToDouble(item.Element("Quantity").Value),
                    Price = Convert.ToDouble(item.Element("Price").Value),
                    WarehouseCode = item.Element("WarehouseCode").Value,
                    WarehouseName = item.Element("WarehouseName").Value,
                    DiscountPercent = Convert.ToDouble(item.Element("DiscPrcnt").Value),
                    DiscountSum = Convert.ToDouble(item.Element("DiscSum").Value),
                    LineTotal = Convert.ToDouble(item.Element("LineTotal").Value),
                    CostCenterCode = item.Element("OcrCode").Value,
                    CostCenterName = item.Element("OcrName").Value,
                    ProjectCode = item.Element("Project").Value,
                    ProjectName = item.Element("PrjName").Value
                };

                details.Add(model_detail);
            }

            DO_HEADER model = new DO_HEADER()
            {
                DocEntry = xEle.Element("DocEntry").Value,
                DocNum = xEle.Element("DocNum").Value,
                CustomerCode = xEle.Element("CardCode").Value,
                CustomerName = xEle.Element("CardName").Value,
                SalesPersonCode = xEle.Element("SlpCode").Value,
                SalesPersonName = xEle.Element("SalesEmploye").Value,
                PostingDate = DateTime.ParseExact(xEle.Element("DocDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None),
                DocDueDate = DateTime.ParseExact(xEle.Element("DocDueDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None),
                DocumentDate = DateTime.ParseExact(xEle.Element("TaxDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None),
                TotalTransaction = Convert.ToDouble(xEle.Element("DocTotal").Value),

                Lines = details
            };

            return model;
        }

        public List<DO_HEADER> GetData(SAPbobsCOM.Company oCompany, DateTime Fromdate, DateTime ToDate)
        {
            Utils control = new Utils();
            List<DO_HEADER> details = new List<DO_HEADER>();
            XDocument xDoc = new XDocument();
            XDocument xDocDetail = new XDocument();



            string sql = "select T0.DocEntry,T0.DocNum ,T0.Series,T1.SeriesName,T0.DocDate,T0.DocDueDate,T0.TaxDate,T0.CardCode as 'CustomerCode',T0.CardName as 'CustomerName', " +
                "(select sum(TA.LineTotal) from DLN1 TA where TA.DocEntry = T0.DocEntry) as 'TotalBeforeDiscount',T0.DocTotal,T2.SlpName as 'SalesEmploye', T0.Comments as 'Remarks' " +
                " ,T0.CntctCode as 'ContactPersonCode',T3.[Name] as 'ContactPersonName', T0.NumAtCard as 'CustomerRefNumber',T0.SlpCode, T0.DocTotal " + 
                "FROM ODLN T0  INNER JOIN NNM1 T1 on T1.Series = T0.Series LEFT JOIN OSLP T2 on T2.SlpCode = T0.SlpCode LEFT JOIN OCPR T3 on T3.CntctCode = T0.CntctCode " +
                "Where T0.DocDate between '" + Fromdate + "' and '"+ ToDate +"'";

                  Recordset rs = control._IDU_Recordset(oCompany, sql);
                  xDoc = XDocument.Parse(rs.GetAsXML());

          //  XElement xEle = xDoc.Element("BOM").Element("BO").Element("ODLN").Element("row");
       

  

            List<DO_HEADER> model = (from xEle in xDoc.Element("BOM").Element("BO").Element("ODLN").Elements("row")
                                     select new DO_HEADER
                                    {
                                        DocEntry = xEle.Element("DocEntry").Value,
                                        DocNum = xEle.Element("DocNum").Value,
                                        Series =Convert.ToInt32(xEle.Element("Series").Value),
                                        SeriesName = xEle.Element("SeriesName").Value,
                                        CardCode = xEle.Element("CustomerCode").Value,
                                        CardName = xEle.Element("CustomerName").Value,
                                        SalesPersonCode = xEle.Element("SlpCode").Value,
                                        SalesPersonName = xEle.Element("SalesEmploye").Value,
                                        ContactPersonCode = xEle.Element("ContactPersonCode").Value,
                                        ContactPersonName = xEle.Element("ContactPersonName").Value,
                                        Remarks = xEle.Element("Remarks").Value,
                                        PostingDate = DateTime.ParseExact(xEle.Element("DocDate").Value, "yyyyMMdd",
                                                        CultureInfo.InvariantCulture, DateTimeStyles.None),
                                        DocDueDate = DateTime.ParseExact(xEle.Element("DocDueDate").Value, "yyyyMMdd",
                                                        CultureInfo.InvariantCulture, DateTimeStyles.None),
                                        TaxDate = DateTime.ParseExact(xEle.Element("TaxDate").Value, "yyyyMMdd",
                                                        CultureInfo.InvariantCulture, DateTimeStyles.None),
                                        TotalTransaction = Convert.ToDouble(xEle.Element("DocTotal").Value),
                                        TotalBeforeDiscount = Convert.ToDouble(xEle.Element("TotalBeforeDiscount").Value),
                                        CustomerRefNumber = xEle.Element("CustomerRefNumber").Value
                
                                    }).ToList();

            return model;
        }
        public static string AddData(Company oCompany, DO_HEADER model)
        {
            Documents oObject = null;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {

                oCompany.StartTransaction();
                oObject = oCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);

                oObject.CardCode = model.CardCode;
                oObject.DocDate = model.PostingDate;
                oObject.DocDueDate = model.DocDueDate;
                oObject.TaxDate = model.DocDueDate;
                oObject.DocType = BoDocumentTypes.dDocument_Items;
                //oObject.DocTotal = model.TotalTransaction;
    
                oObject.SalesPersonCode = Convert.ToInt32(model.SalesPersonCode);

                for (int i = 0; i < model.Lines.Count; i++)
                {
                    oObject.Lines.ItemCode = model.Lines[i].ItemCode;
                    oObject.Lines.WarehouseCode = model.Lines[i].WarehouseCode;
                    //oObject.Lines.UoMEntry   = model.lines[i].UomEntry ;
                    oObject.Lines.DiscountPercent = model.Lines[i].DiscountPercent;
                    oObject.Lines.UnitPrice = model.Lines[i].Price;
                    oObject.Lines.Quantity = model.Lines[i].Quantity;

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
    }
}