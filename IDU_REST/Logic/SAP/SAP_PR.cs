using IDU_REST.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Logic.SAP
{
    public class SAP_PR
    {
        public static string AddData(Company oCompany, PR_HEADER model)
        {
            Documents oObject = null;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();

                oObject = oCompany.GetBusinessObject(BoObjectTypes.oPurchaseRequest);
                oObject.Series = model.series;
                oObject.ReqType = model.reqType;
                oObject.Requester = model.requester;
                oObject.DocDate = model.postingDate;
                oObject.DocDueDate = model.docDueDate;
                oObject.TaxDate = model.documentDate;
                oObject.RequriedDate = model.reqDate;

                for (int i = 0; i < model.lines.Count; i++)
                {
                    oObject.Lines.ItemCode = model.lines[i].itemCode;
                    oObject.Lines.UnitPrice = model.lines[i].infoPrice;
                    oObject.Lines.Quantity = model.lines[i].requiredQuantity;
                    oObject.Lines.TaxCode = model.lines[i].taxCode;
                    oObject.Lines.LineVendor = model.lines[i].vendor;
                    oObject.Lines.ProjectCode = model.lines[i].siteID;

                    oObject.Lines.UserFields.Fields.Item("U_IDU_SiteName").Value = model.lines[i].siteName;
                    oObject.Lines.CostingCode = model.lines[i].costCenter;

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