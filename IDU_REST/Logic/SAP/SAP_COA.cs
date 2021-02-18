using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDU_REST.Models;
using Newtonsoft.Json;
using SAPbobsCOM;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
namespace IDU_REST.Logic.SAP
{
    public class SAP_COA
    {

        public static List<COA> GetData(SAPbobsCOM.Company oCompany)
        {
            Utils control = new Utils();

            string sql = "AcctCode, AcctName,ActCurr,Postable,FatherNum,Levels,ActType, " +
                         "(CASE WHEN ActType = 'N' then 'Other' " +
                         " WHEN ActType = 'I' then 'Sales' " +
                         " WHEN ActType = 'E' then 'Expenditure'" +
                         " ELSE 'Not Defined'" +
                         "   end) 'TypeName'" +
                         "from OACT";

            Recordset rs = control._IDU_Recordset(oCompany, sql);

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Parse(rs.GetAsXML());
            List<COA> list = (from data in xDoc.Element("BOM").Element("BO").Element("OACT").Elements("row")
                             select new COA
                             {
                                 AccountCode = data.Element("AcctCode").Value,
                                 AccountName = data.Element("AcctName").Value,
                                 AccountCurrency = data.Element("ActCurr").Value,
                                 PostAble = data.Element("PostAble").Value,
                                 FatherAccount = data.Element("FatherNum").Value,
                                 AccountType = data.Element("TypeName").Value
                             }).ToList();

            return list;
        }

        public static List<COA> GetData(SAPbobsCOM.Company oCompany,string id)
        {
            Utils control = new Utils();

            string sql = "AcctCode, AcctName,ActCurr,Postable,FatherNum,Levels,ActType, " +
                         "(CASE WHEN ActType = 'N' then 'Other' " +
                         " WHEN ActType = 'I' then 'Sales' " +
                         " WHEN ActType = 'E' then 'Expenditure'" +
                         " ELSE 'Not Defined'" +
                         "   end) 'TypeName'" +
                         "from OACT where AcctCode = '"+ id +"'";

            Recordset rs = control._IDU_Recordset(oCompany, sql);

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Parse(rs.GetAsXML());
            List<COA> list = (from data in xDoc.Element("BOM").Element("BO").Element("OACT").Elements("row")
                              select new COA
                              {
                                  AccountCode = data.Element("AcctCode").Value,
                                  AccountName = data.Element("AcctName").Value,
                                  AccountCurrency = data.Element("ActCurr").Value,
                                  PostAble = data.Element("PostAble").Value,
                                  FatherAccount = data.Element("FatherNum").Value,
                                  AccountType = data.Element("TypeName").Value
                              }).ToList();

            return list;
        }
        public static string update(Company oCompany, COA model,string AcctCode)
        {
            SAPbobsCOM.ChartOfAccounts oObject;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();
                oObject = oCompany.GetBusinessObject(BoObjectTypes.oChartOfAccounts);
                oObject.GetByKey(AcctCode);
                oObject.Name = model.AccountName;
                oObject.AcctCurrency = model.AccountCurrency;
                if (model.AccountType == "Revenues")
                {
                    oObject.AccountType = BoAccountTypes.at_Revenues;
                }
                else if (model.AccountType == "Expenses")
                {
                    oObject.AccountType = BoAccountTypes.at_Expenses;
                }
                else
                {
                    oObject.AccountType = BoAccountTypes.at_Other;
                }
                oObject.FatherAccountKey = model.FatherAccount;
                int statusCOA = oObject.Update();
                if (statusCOA == 0)
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                    if (strResult == "")
                    {
                        strResult = oCompany.GetNewObjectKey();

                    }
                    else
                    {
                        strResult = strResult + "," + oCompany.GetNewObjectKey();
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
            }
            catch (Exception e)
            {
                if (oCompany.InTransaction)
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                };
                throw;
            }
            return strResult;
        }
        public static string AddData(Company oCompany, COA model)
        {
            SAPbobsCOM.ChartOfAccounts oObject;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();
                oObject = oCompany.GetBusinessObject(BoObjectTypes.oChartOfAccounts);
                oObject.Code = model.AccountCode;
                oObject.Name = model.AccountName;
                oObject.AcctCurrency = model.AccountCurrency;
                if (model.AccountType == "Revenues")
                {
                    oObject.AccountType = BoAccountTypes.at_Revenues;
                }
                else if(model.AccountType == "Expenses")
                {
                    oObject.AccountType = BoAccountTypes.at_Expenses;
                }
                else
                {
                    oObject.AccountType = BoAccountTypes.at_Other;
                }
                oObject.FatherAccountKey = model.FatherAccount;
                int statusCOA = oObject.Add();
                if (statusCOA==0)
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                    if (strResult == "")
                    {
                        strResult = oCompany.GetNewObjectKey();

                    }
                    else
                    {
                        strResult = strResult + "," + oCompany.GetNewObjectKey();
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
            }
            catch(Exception e)
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