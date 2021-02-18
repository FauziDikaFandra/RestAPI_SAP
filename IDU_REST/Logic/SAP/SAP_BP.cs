using IDU_REST.Models;
using Newtonsoft.Json;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace IDU_REST.Logic.SAP
{
    public class SAP_BP
    {
        public static List<BP> GetData(SAPbobsCOM.Company oCompany)
        {
            Utils control = new Utils();

            string sql = "select CardCode, CardName ,Currency,Address,DebPayAcct,CardType, " +
                          " (CASE WHEN CardType = 'C' Then 'Customer' " +
                          " WHEN CardType = 'S' Then 'Supplier' " +
                          " else 'Lead' " +
                          " end) as 'TypeName' " +
                         " from ocrd";

            Recordset rs = control._IDU_Recordset(oCompany, sql);

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Parse(rs.GetAsXML());

            List<BP> list = (from data in xDoc.Element("BOM").Element("BO").Element("ocrd").Elements("row")
                             select new BP
                             {
                                 CardCode = data.Element("CardCode").Value,
                                 CardName = data.Element("CardName").Value,
                                 Address = data.Element("Address").Value,
                                 Currency = data.Element("Currency").Value,
                                 BPType = data.Element("TypeName").Value,
                                 DebtPayAccount = data.Element("DebPayAcct").Value
                             }).ToList();

            return list;
        }

        public static string update(Company oCompany, BP model,string BPCode)
        {
            SAPbobsCOM.BusinessPartners oObject;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();
                oObject = oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                oObject.GetByKey(BPCode);
                oObject.CardName = model.CardName;
                if (model.BPType == "Vendor")
                {
                    oObject.CardType = BoCardTypes.cCustomer;
                }
                else if (model.BPType == "Lead")
                {
                    oObject.CardType = BoCardTypes.cLid;
                }
                else
                {
                    oObject.CardType = BoCardTypes.cSupplier;
                }
                oObject.Currency = model.Currency;
                oObject.DebitorAccount = model.DebtPayAccount;
                oObject.Address = model.Address;
                int statusBP = oObject.Update();
                if (statusBP == 0)
                {
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
                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
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
        public static string AddData(Company oCompany, BP model)
        {
            SAPbobsCOM.BusinessPartners oObject;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {
                oCompany.StartTransaction();
                oObject = oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                oObject.CardCode = model.CardCode;
                oObject.CardName = model.CardName;
                if (model.BPType =="Vendor")
                {
                    oObject.CardType = BoCardTypes.cCustomer;
                }
                else if (model.BPType == "Lead")
                {
                    oObject.CardType = BoCardTypes.cLid;
                }
                else
                {
                    oObject.CardType = BoCardTypes.cSupplier;
                }
                oObject.Currency = model.Currency;
                oObject.DebitorAccount = model.DebtPayAccount;
                oObject.Address = model.Address;
                int statusBP = oObject.Add();
                if (statusBP == 0)
                {
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
                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
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
        public static BP GetData(SAPbobsCOM.Company oCompany, string id)
        {
            Utils control = new Utils();

            string sql = " select CardCode, CardName,Currency,Address,DebPayAcct,CardType, " +
                          " (CASE WHEN CardType = 'C' Then 'Customer' " +
                          " WHEN CardType = 'S' Then 'Supplier' " +
                          " else 'Lead' " +
                          " end) as 'TypeName' " +
                        " from ocrd where CardCode = '" + id + "'";

            Recordset rs = control._IDU_Recordset(oCompany, sql);

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Parse(rs.GetAsXML());
            XElement xEle = xDoc.Element("BOM").Element("BO").Element("ocrd").Element("row");

            BP model = new BP() {
                CardCode = xEle.Element("CardCode").Value,
                CardName = xEle.Element("CardName").Value,
                Address = xEle.Element("Address").Value,
                Currency = xEle.Element("Currency").Value,
                BPType = xEle.Element("TypeName").Value,
                DebtPayAccount = xEle.Element("DebPayAcct").Value
            };

            return model;
        }

    }
}