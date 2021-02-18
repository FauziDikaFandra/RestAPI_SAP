using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDU_REST.Models;
using SAPbobsCOM;
using System.Globalization;
using System.Xml.Linq;
namespace IDU_REST.Logic.SAP
{
    public class SAP_JV2
    {
        public static string AddData(Company oCompany, JV model)
        {
            SAPbobsCOM.JournalVouchers oObject;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";
            string GetCoa = string.Empty;
            try
            {
                oCompany.StartTransaction();

                oObject = oCompany.GetBusinessObject(BoObjectTypes.oJournalVouchers);
                oObject.JournalEntries.ReferenceDate = model.Refdate;
                oObject.JournalEntries.DueDate = model.DueDate;
                oObject.JournalEntries.TaxDate = model.TaxDate;
                oObject.JournalEntries.Reference = model.Ref1;
                oObject.JournalEntries.Reference2 = model.Ref2;
                oObject.JournalEntries.Reference3 = model.Ref3;
                oObject.JournalEntries.Memo = model.Remarks;

                oObject.JournalEntries.UserFields.Fields.Item("U_MaximoId").Value = model.MaximoId;
                oObject.JournalEntries.UserFields.Fields.Item("U_MaximoNumber").Value = model.MaximoNumber;

                if (model.ProjectId != null)
                {
                    if (model.ProjectId != "")
                    {
                        oObject.JournalEntries.ProjectCode = model.ProjectId;
                    }

                }

                for (int i = 0; i < model.Lines.Count; i++)
                {
                    string[] ParamAccount = model.Lines[i].AccountCode.Split('-');

                    if (ParamAccount.Count() > 0)
                    {
                        GetCoa = getConvertCOA(oCompany, ParamAccount[0], ParamAccount[1], ParamAccount[2]);
                    }
                    else
                    {
                        GetCoa = model.Lines[i].AccountCode;
                    }
                    
                    oObject.JournalEntries.Lines.AccountCode = GetCoa;
                    oObject.JournalEntries.Lines.Debit = Convert.ToDouble(model.Lines[i].DebitAmount);
                    oObject.JournalEntries.Lines.Credit = Convert.ToDouble(model.Lines[i].CreditAmount);
                    oObject.JournalEntries.Lines.LineMemo = model.Lines[i].LineMemo;
                    
                    if (model.Lines[i].ref1 != null) oObject.JournalEntries.Lines.AdditionalReference = model.Lines[i].ref1;
                    if (model.Lines[i].ref1 != null) oObject.JournalEntries.Lines.Reference1 = model.Lines[i].ref2;
                    if (model.Lines[i].ref2 != null) oObject.JournalEntries.Lines.Reference2 = model.Lines[i].ref3;
                    if (model.Lines[i].DebitAmountFC != 0 || model.Lines[i].CreditAmountFC != 0)
                    {
                        oObject.JournalEntries.Lines.FCCurrency = model.Lines[i].FCCurrency;
                        oObject.JournalEntries.Lines.FCDebit = Convert.ToDouble(model.Lines[i].DebitAmountFC);
                        oObject.JournalEntries.Lines.FCCredit = Convert.ToDouble(model.Lines[i].CreditAmountFC);
                    }

                    if (model.Lines[i].CostCenter != null)
                    {
                        if (model.Lines[i].CostCenter != "")
                        {
                            oObject.JournalEntries.Lines.CostingCode = model.Lines[i].CostCenter;
                        }
                    }
                    oObject.JournalEntries.Lines.Add();

                }
                int statusJV = oObject.Add();

                if (statusJV == 0)
                {
                    if (strResult == "")
                    {
                        strResult = oCompany.GetNewObjectKey();

                    }
                    else
                    {
                        strResult = strResult + " - " + oCompany.GetNewObjectKey();
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

        private static string getConvertCOA(SAPbobsCOM.Company oCompany,string Segment0, string Segment1, string Segment2)
        {
            Utils control = new Utils();
            string AccounCode = string.Empty;
            try
            {
                string sql = "select AcctCode from OACT where Segment_0 = '" + Segment0 + "' and Segment_1 = '" + Segment1 + "' and Segment_2 = '" + Segment2 + "'";
                Recordset rs = control._IDU_Recordset(oCompany, sql);
                if (rs.RecordCount > 0)
                {
                    AccounCode = rs.Fields.Item("AcctCode").Value;
                }
                else
                {
                    AccounCode = "Message not found";
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return AccounCode;
        }

    }

}