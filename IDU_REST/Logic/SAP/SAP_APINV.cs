using IDU_REST.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Logic.SAP
{
    public class SAP_APINV
    {
        public static string AddData(Company oCompany, APINV_HEADER model)
        {
            Documents oObject = null;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();

                oObject = oCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);

                oObject.Series = model.series;
                oObject.CardCode = model.cardCode;
                oObject.DocDate = model.postingDate;
                oObject.DocDueDate = model.docDueDate;
                oObject.TaxDate = model.documentDate;
                oObject.DocType = BoDocumentTypes.dDocument_Items;

                for (int i = 0; i < model.lines.Count; i++)
                {
                    oObject.Lines.ItemCode = model.lines[i].ItemCode;
                    oObject.Lines.WarehouseCode = model.lines[i].WarehouseCode;
                    oObject.Lines.UoMEntry = model.lines[i].UomEntry;
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