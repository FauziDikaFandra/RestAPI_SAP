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
    public class SAP_ARINV
    {
        public static ARINV_HEADER GetData(SAPbobsCOM.Company oCompany, int id)
        {
            Utils control = new Utils();
            List<ARINV_DETAIL> details = new List<ARINV_DETAIL>();
            XDocument xDoc = new XDocument();
            XDocument xDocDetail = new XDocument();

            string sql = "select U_PrimaryNumberingPOS, U_DocumentNumberingPOS, docentry, docnum, CardCode, " +
                "CardName, DocDate, DocDueDate, TaxDate, DocTotal, t0.slpcode, slpname from oinv t0 " +
                "join oslp t1 on t0.slpcode = t1.slpcode where docentry = " + id;

            Recordset rs = control._IDU_Recordset(oCompany, sql);
            xDoc = XDocument.Parse(rs.GetAsXML());

            sql = "select t1.LineNum, ItemCode, dscription, UomEntry, UomCode, Quantity,Price, t2.WhsCode, t2.WhsName, t1.DiscPrcnt from inv1 t1 " +
                "join owhs t2 on t1.WhsCode = t2.WhsCode where t1.docentry = " + id;
            rs = control._IDU_Recordset(oCompany, sql);
            xDocDetail = XDocument.Parse(rs.GetAsXML());

            XElement xEle = xDoc.Element("BOM").Element("BO").Element("oinv").Element("row");
            XElement xEleDetail = xDocDetail.Element("BOM").Element("BO").Element("inv1");
            
            foreach (var item in xEleDetail.Elements("row"))
	        {
                ARINV_DETAIL model_detail = new ARINV_DETAIL()
                {
                    LineNum = Convert.ToInt32(item.Element("LineNum").Value),
                    ItemCode = item.Element("ItemCode").Value,
                    Description = item.Element("dscription").Value,
                    UomEntry = Convert.ToInt32(item.Element("UomEntry").Value),
                    UomCode = item.Element("UomCode").Value,
                    Quantity = Convert.ToDouble(item.Element("Quantity").Value),
                    Price =  Convert.ToDouble(item.Element("Price").Value),
                    WarehouseCode = item.Element("WhsCode").Value,
                    WarehouseName = item.Element("WhsName").Value,
                    DiscountPercent = Convert.ToDouble(item.Element("DiscPrcnt").Value)          
                };

                details.Add(model_detail);
	        }

            ARINV_HEADER model = new ARINV_HEADER()
            {
                PrimaryNumberingPOS = xEle.Element("U_PrimaryNumberingPOS").Value,
                DocumentNumberingPOS = xEle.Element("U_DocumentNumberingPOS").Value,
                DocEntry = xEle.Element("docentry").Value,
                DocNum = xEle.Element("docnum").Value,
                CardCode = xEle.Element("CardCode").Value,
                CardName = xEle.Element("CardName").Value,
                SalesPersonCode = xEle.Element("slpcode").Value,
                SalesPersonName = xEle.Element("slpname").Value,
                DocumentDate = DateTime.ParseExact(xEle.Element("DocDate").Value,"yyyyMMdd",
                                CultureInfo.InvariantCulture,DateTimeStyles.None),
                DocDueDate = DateTime.ParseExact(xEle.Element("DocDueDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None),
                TaxDate = DateTime.ParseExact(xEle.Element("TaxDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None),
                TotalTransaction = Convert.ToDouble(xEle.Element("DocTotal").Value),

                lines = details
            };

            return model;
        }

        public static string AddData(Company oCompany, ARINV_HEADER model)
        {
            Documents oObject = null;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();

                oObject = oCompany.GetBusinessObject(BoObjectTypes.oInvoices);

                oObject.CardCode = model.CardCode ;
                oObject.DocDate = model.DocumentDate;
                oObject.DocDueDate = model.DocDueDate;
                oObject.TaxDate = model.DocDueDate;
                oObject.DocType = BoDocumentTypes.dDocument_Items;
                //oObject.DocTotal = model.TotalTransaction;
                oObject.UserFields.Fields.Item("U_DocumentNumberingPOS").Value = model.DocumentNumberingPOS;
                oObject.UserFields.Fields.Item("U_PrimaryNumberingPOS").Value = model.PrimaryNumberingPOS;
                oObject.SalesPersonCode = Convert.ToInt32(model.SalesPersonCode);

                for (int i = 0; i < model.lines.Count; i++)
                {
                    oObject.Lines.ItemCode = model.lines[i].ItemCode;
                    oObject.Lines.WarehouseCode = model.lines[i].WarehouseCode;
                    //oObject.Lines.UoMEntry   = model.lines[i].UomEntry ;
                    oObject.Lines.DiscountPercent = model.lines[i].DiscountPercent;
                    oObject.Lines.UnitPrice = model.lines[i].Price;
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
    }
}