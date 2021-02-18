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
    public class SAP_GRFROMPROD
    {
        public RTGIPROD GetInfo(Company oCompany,Int32 DocEntry, string result)
        {

            Utils control = new Utils();
            RTGIPROD info = new RTGIPROD();
            XDocument xDoc = new XDocument();
   

            string sql = "SELECT   T0.DocNum as 'Number',T3.SeriesName as 'SeriesNo',T4.DocNum as 'OrderNumber',T1.ItemCode ,T1.Dscription as 'ItemName'," +
                         " T1.WhsCode,T5.WhsName,T1.Quantity,T1.Price,isnull(T2.CalcPrice,0) as 'ItemCost'" +
                         " FROM OIGN T0 INNER JOIN IGN1 T1 on T0.DocEntry = T1.DocEntry  " +
                         " LEFT JOIN OINM T2 on T2.CreatedBy = T0.DocEntry and T2.TransType = T0.ObjType and T2.ItemCode = T1.ItemCode and T2.BaseLine = T1.LineNum " +
                         " LEFT JOIN NNM1 T3 on T3.Series = T0.Series LEFT JOIN OWOR T4 on T4.DocEntry = T1.BaseEntry and T4.ObjType = T1.BaseType " +
                         " LEFT JOIN OWHS T5 on T5.WhsCode = T1.WhsCode " +
                         "where T0.Docentry = '" + DocEntry + "'";


            Recordset rs = control._IDU_Recordset(oCompany, sql);

           
            xDoc = XDocument.Parse(rs.GetAsXML());
            XElement xEle = xDoc.Element("BOM").Element("BO").Element("OIGN").Element("row");

            RTGIPROD model = new RTGIPROD()
            {
                ItemCode = xEle.Element("ItemCode").Value,
                ItemName = xEle.Element("ItemName").Value,
                OrderNumber = xEle.Element("OrderNumber").Value,
                SeriesNo = xEle.Element("SeriesNo").Value,
                WarehouseCode = xEle.Element("WhsCode").Value,
                WarehouseName = xEle.Element("WhsName").Value,
                Quantity = Convert.ToDecimal(xEle.Element("Quantity").Value),
                UnitPrice = Convert.ToDecimal(xEle.Element("Price").Value),
                ItemCost = Convert.ToDecimal(xEle.Element("ItemCost").Value),
                value = DocEntry.ToString() 
            };

            return model;
            
        }

        public RTGIPROD AddData(Company oCompany, GRFROMPROD model)
        {
            Documents oObject = null;
            RTNMANVAL rtn = new RTNMANVAL();
            RTGIPROD rtn2 = new RTGIPROD();
            rtn2 = null;
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();

                oObject = oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);

                oObject.DocDate = model.DocumentDate;
                //oObject.DocDueDate = model.DocumentDate;
                //oObject.TaxDate = model.DocumentDate;
                oObject.DocType = BoDocumentTypes.dDocument_Items;
                oObject.Comments = model.Remarks + " Create By WebService";
                oObject.Reference2 = model.Reference2;
                for (int i = 0; i < model.Detil.Count; i++)
                {
                    //oObject.Lines.ItemCode = model.Detil[i].ItemCode;
                    //oObject.Lines.WarehouseCode = model.Detil[i].WarehouseCode;
                    oObject.Lines.Quantity = model.Detil[i].Quantity;
                    oObject.Lines.BaseEntry = model.Detil[i].BaseEntry;
                    oObject.Lines.BaseType = 202;
                    oObject.Lines.TransactionType = BoTransactionTypeEnum.botrntComplete;
                    //oObject.Lines.BaseLine = model.Detil[i].BaseLine;
                    oObject.Lines.Add();
                }

                int addStatus = oObject.Add();

                if (addStatus == 0)
                {
                    if (strResult == "")
                    {
                        strResult = oCompany.GetNewObjectKey();
                        rtn2 = GetInfo(oCompany, Convert.ToInt32(oCompany.GetNewObjectKey()), strResult);
                    }
                    else
                    {
                        strResult = strResult + " | " + oCompany.GetNewObjectKey();
                        rtn2 = GetInfo(oCompany, Convert.ToInt32(oCompany.GetNewObjectKey()), strResult);
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

            return rtn2;
        }
    }
}