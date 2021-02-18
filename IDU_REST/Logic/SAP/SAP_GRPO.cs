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
    public class SAP_GRPO
    {
        public static GRPO_HEADER GetData(SAPbobsCOM.Company oCompany, int id)
        {
            Utils control = new Utils();
            List<GRPO_DETAIL> details = new List<GRPO_DETAIL>();
            XDocument xDoc = new XDocument();
            XDocument xDocDetail = new XDocument();

            string sql = "select T0.DocEntry ,T0.DocNum as 'NumberForm',T0.DocDate as 'PostingDate',T0.DocDueDate as 'DueDate',T0.TaxDate as 'DocumentDate' "+
                         ",T0.CardCode as 'VendorCode',T0.CardName as 'VendorName',T0.DocStatus as 'Status',T0.Comments as 'Remarks'  FROM OPDN T0 where T0.DocEntry = " + id;
            Recordset rs = control._IDU_Recordset(oCompany, sql);
            xDoc = XDocument.Parse(rs.GetAsXML());

            sql = "Select T1.ItemCode as 'ItemNo',T1.Dscription as 'ItemDescription',T1.Quantity,T1.Price,T1.Currency,T1.LineTotal as 'Total',T1.WhsCode,T1.AcctCode as 'GLAccount',T1.OcrCode as 'CostCenter',T1.unitMsr as 'UOM'" +
                ",T1.BaseEntry, T1.BaseLine "+
                "FROM PDN1 T1 " +
                "WHERE T1.docentry = " + id;
            rs = control._IDU_Recordset(oCompany, sql);
            xDocDetail = XDocument.Parse(rs.GetAsXML());

            XElement xEle = xDoc.Element("BOM").Element("BO").Element("OPDN").Element("row");
            XElement xEleDetail = xDocDetail.Element("BOM").Element("BO").Element("PDN1");
            
            foreach (var item in xEleDetail.Elements("row"))
	        {
                GRPO_DETAIL model_detail = new GRPO_DETAIL()
                {
                    ItemCode = item.Element("ItemNo").Value,
                    ItemDescription = item.Element("ItemDescription").Value,
                    UOM =  item.Element("UOM").Value,
                    Quantity = Convert.ToDouble(item.Element("Quantity").Value),
                    Price = Convert.ToDouble(item.Element("Price").Value),
                    WarehouseCode = item.Element("WhsCode").Value,
                    GLAccount = item.Element("GLAccount").Value, 
                    CostCenter = item.Element("CostCenter").Value,
                    Currency = item.Element("Currency").Value,
                    Total = Convert.ToDecimal(item.Element("Total").Value)
              
                };

                details.Add(model_detail);
	        }

            GRPO_HEADER model = new GRPO_HEADER()
            {
 
                DocEntry = xEle.Element("DocEntry").Value,
                NumberForm = xEle.Element("NumberForm").Value,
                VendorCode = xEle.Element("VendorCode").Value,
                VendorName = xEle.Element("VendorName").Value,
                Status = xEle.Element("Status").Value,
                Remarks = xEle.Element("Remarks").Value,
                PostingDate = DateTime.ParseExact(xEle.Element("PostingDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture,DateTimeStyles.None),
                DueDate = DateTime.ParseExact(xEle.Element("DueDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None),
                DocumentDate = DateTime.ParseExact(xEle.Element("DocumentDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None),

                lines = details
            };

            return model;
        }

        public static List<GRPO_HEADER> GetDataList(SAPbobsCOM.Company oCompany, DateTime FromDate,DateTime ToDate)
        {
            Utils control = new Utils();
            List<GRPO_DETAIL> details = new List<GRPO_DETAIL>();
            XDocument xDoc = new XDocument();
            XDocument xDocDetail = new XDocument();

            string sql = "select T0.DocEntry ,T0.DocNum as 'NumberForm',T0.DocDate as 'PostingDate',T0.DocDueDate as 'DueDate',T0.TaxDate as 'DocumentDate'"+
                         ",T0.CardCode as 'VendorCode',T0.CardName as 'VendorName',T0.DocStatus as 'Status',T0.Comments as 'Remarks'" +
                          " FROM OPDN T0 where DocDate between '" + FromDate + "' and '" + ToDate +"'";
            Recordset rs = control._IDU_Recordset(oCompany, sql);
            xDoc = XDocument.Parse(rs.GetAsXML());


            List<GRPO_HEADER> model = (from data in xDoc.Element("BOM").Element("BO").Element("OPDN").Elements("row")
                                       select new GRPO_HEADER
                                        {
                                            DocEntry = data.Element("DocEntry").Value,
                                            NumberForm = data.Element("NumberForm").Value,
                                            VendorCode = data.Element("VendorCode").Value,
                                            VendorName = data.Element("VendorName").Value,
                                            Status = data.Element("Status").Value,
                                            Remarks = data.Element("Remarks").Value,
                                            PostingDate = DateTime.ParseExact(data.Element("PostingDate").Value, "yyyyMMdd",
                                                            CultureInfo.InvariantCulture, DateTimeStyles.None),
                                            DueDate = DateTime.ParseExact(data.Element("DueDate").Value, "yyyyMMdd",
                                                            CultureInfo.InvariantCulture, DateTimeStyles.None),
                                            DocumentDate = DateTime.ParseExact(data.Element("DocumentDate").Value, "yyyyMMdd",
                                                            CultureInfo.InvariantCulture, DateTimeStyles.None)
                
                                        }
                                        ).ToList();

            return model;
        }

        public static string AddData(Company oCompany, GRPO_HEADER model)
        {
            Documents oObject = null;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();

                oObject = oCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);

                oObject.CardCode = model.VendorCode;
                oObject.DocDate = model.PostingDate;
                oObject.DocDueDate = model.DueDate;
                oObject.TaxDate = model.DocumentDate;
                oObject.DocType = BoDocumentTypes.dDocument_Items;
 

                for (int i = 0; i < model.lines.Count; i++)
                {
                    oObject.Lines.ItemCode = model.lines[i].ItemCode;
                    oObject.Lines.WarehouseCode = model.lines[i].WarehouseCode;
                    //oObject.Lines.UoMEntry = model.lines[i].UOM;
                    oObject.Lines.UnitPrice = model.lines[i].Price;
                    oObject.Lines.Quantity = model.lines[i].Quantity;

                    if (model.lines[i].BaseEntry != 0)
                    {
                        oObject.Lines.BaseEntry = model.lines[i].BaseEntry;
                        oObject.Lines.BaseLine = model.lines[i].BaseLine;
                        oObject.Lines.BaseType = Convert.ToInt32(BoObjectTypes.oPurchaseOrders);
                    }

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