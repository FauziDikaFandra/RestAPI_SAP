using IDU_REST.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDU_REST.Logic.SAP
{
    public class SAP_ARBA
    {
        public static string AddData(Company oCompany, ARBA model)
        {
            BlanketAgreement oObject = null;
            BlanketAgreementsService oService = null;

            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();

                oService = oCompany.GetCompanyService().GetBusinessService(ServiceTypes.BlanketAgreementsService);
                oObject = oService.GetDataInterface(BlanketAgreementsServiceDataInterfaces.basBlanketAgreement);

                oObject.BPCode = model.bpCode;
                oObject.UserFields.Item("U_IDU_SiteID").Value = model.siteID;
                oObject.UserFields.Item("U_IDU_SiteName").Value = model.siteName;
                oObject.UserFields.Item("U_IDU_SiteOp").Value = model.siteOp;
                oObject.Remarks = model.remarks;
                oObject.StartDate = model.startDate;
                oObject.EndDate = model.endDate;
                oObject.AgreementType = BlanketAgreementTypeEnum.atSpecific;

                BlanketAgreements_ItemsLine lines = oObject.BlanketAgreements_ItemsLines.Add();
                lines.ItemNo = "L00001";
                lines.PlannedQuantity = model.plannedQuantity;
                lines.UnitPrice = model.unitPrice;

                int addStatus = oService.AddBlanketAgreement(oObject).AgreementNo;

                if (addStatus != 0)
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