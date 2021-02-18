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
    public class SAP_IT
    {
        public static IT_HEADER GetData(SAPbobsCOM.Company oCompany, int id)
        {
            Utils control = new Utils();
            List<IT_DETAIL> details = new List<IT_DETAIL>();
            XDocument xDoc = new XDocument();
            XDocument xDocDetail = new XDocument();

            string sql = "select  t0.docentry, t0.docnum, t0.DocDate, t0.TaxDate, t1.whscode, t1.whsname from OWTR t0 " +
                "join owhs t1 on t0.filler = t1.whscode where t0.docentry = " + id;
            Recordset rs = control._IDU_Recordset(oCompany, sql);
            xDoc = XDocument.Parse(rs.GetAsXML());

            sql = "select t1.LineNum, ItemCode, dscription, UomEntry, UomCode, Quantity, t2.WhsCode, t2.WhsName from WTR1 t1 " +
                "join owhs t2 on t1.WhsCode = t2.WhsCode where t1.docentry = " + id;
            rs = control._IDU_Recordset(oCompany, sql);
            xDocDetail = XDocument.Parse(rs.GetAsXML());

            XElement xEle = xDoc.Element("BOM").Element("BO").Element("OWTR").Element("row");
            XElement xEleDetail = xDocDetail.Element("BOM").Element("BO").Element("WTR1");
            
            foreach (var item in xEleDetail.Elements("row"))
	        {
                IT_DETAIL model_detail = new IT_DETAIL()
                {
                    LineNum = Convert.ToInt32(item.Element("LineNum").Value),
                    ItemCode = item.Element("ItemCode").Value,
                    Description = item.Element("dscription").Value,
                    UomEntry = Convert.ToInt32(item.Element("UomEntry").Value),
                    UomCode = item.Element("UomCode").Value,
                    Quantity = Convert.ToDouble(item.Element("Quantity").Value),
                    ToWarehouseCode = item.Element("WhsCode").Value,
                    ToWarehouseName = item.Element("WhsName").Value
                };

                details.Add(model_detail);
	        }

            IT_HEADER model = new IT_HEADER()
            {
                 
                DocEntry = Convert.ToInt32(xEle.Element("docentry").Value),
                DocNum = xEle.Element("docnum").Value,
                DocumentDate = DateTime.ParseExact(xEle.Element("DocDate").Value,"yyyyMMdd",
                                CultureInfo.InvariantCulture,DateTimeStyles.None),
                TaxDate = DateTime.ParseExact(xEle.Element("TaxDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None),
                FromWarehouseCode = xEle.Element("whscode").Value,
                FromWarehouseName = xEle.Element("whsname").Value,

                lines = details
            };

            return model;
        }

        public static string AddData(Company oCompany, IT_HEADER model)
        {
            StockTransfer oObject = null;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();

                oObject = oCompany.GetBusinessObject(BoObjectTypes.oStockTransfer);

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
                    oObject.Lines.BaseEntry = model.lines[i].BaseEntry;
                    oObject.Lines.BaseLine = model.lines[i].BaseLine;
                    oObject.Lines.BaseType = InvBaseDocTypeEnum.InventoryTransferRequest;
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