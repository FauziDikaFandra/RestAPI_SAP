using IDU_REST.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace IDU_REST.Logic.SAP
{
    public class SAP_ITEM
    {
        public static List<ITEM> GetData(SAPbobsCOM.Company oCompany)
        {
            Utils control = new Utils();

            string sql = "select ItemCode, ItemName from oitm";

            Recordset rs = control._IDU_Recordset(oCompany, sql);

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Parse(rs.GetAsXML());

            List<ITEM> list = (from data in xDoc.Element("BOM").Element("BO").Element("oitm").Elements("row")
                             select new ITEM
                             {
                                 ItemCode = data.Element("ItemCode").Value,
                                 ItemName = data.Element("ItemName").Value
                             }).ToList();

            return list;
        }

        public static ITEM GetData(SAPbobsCOM.Company oCompany, string id)
        {
            Utils control = new Utils();

            string sql = "select ItemCode, ItemName from oitm where ItemCode = '" + id + "'";

                Recordset rs = control._IDU_Recordset(oCompany, sql);

                XDocument xDoc = new XDocument();
                xDoc = XDocument.Parse(rs.GetAsXML());
                XElement xEle = xDoc.Element("BOM").Element("BO").Element("oitm").Element("row");

                ITEM model = new ITEM()
                {
                    ItemCode = xEle.Element("ItemCode").Value,
                    ItemName = xEle.Element("ItemName").Value
                };

                return model;
        }
        public static string AddData(Company oCompany, ITEM model)
        {
            Items  oObject = null;
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";

            try
            {

                oCompany.StartTransaction();
                oObject = oCompany.GetBusinessObject(BoObjectTypes.oItems);

                oObject.ItemCode  = model.ItemCode;
                oObject.ItemName  = model.ItemName;
                
               
               

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