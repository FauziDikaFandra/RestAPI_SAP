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
using System.Threading;
using System.Globalization;
namespace IDU_REST.Logic.SAP
{
    public class SAP_EXR
    {
        public static List<EXR> GetEXR(SAPbobsCOM.Company oCompany)
        {
            Utils control = new Utils();

            string sql = "select Convert(Date,RateDate)as'RateDate',Currency,Rate from ORTT ";
            Recordset rs = control._IDU_Recordset(oCompany, sql);
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("id-ID");
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Parse(rs.GetAsXML());
            List<EXR> list = (from data in xDoc.Element("BOM").Element("BO").Element("ORTT").Elements("row")
                              select new EXR
                              {
                                  DateRate =  DateTime.ParseExact(data.Element("RateDate").Value, "yyyyMMdd", 
                                  CultureInfo.InvariantCulture, DateTimeStyles.None),
                                  Rate = Convert.ToDouble(data.Element("Rate").Value),
                                  Currency = data.Element("Currency").Value
                              }).ToList();

            return list;

        }

        public static List<EXR> GetEXR(SAPbobsCOM.Company oCompany,DateTime RateDate)
        {
            Utils control = new Utils();

            string sql = "select RateDate,Currency,Rate from ORTT where  RateDate = '"+ RateDate.ToString("yyyy/MM/dd",CultureInfo.InvariantCulture) +"'";
            Recordset rs = control._IDU_Recordset(oCompany, sql);

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Parse(rs.GetAsXML());
            List<EXR> list = (from data in xDoc.Element("BOM").Element("BO").Element("ORTT").Elements("row")
                              select new EXR
                              {
                                  DateRate = DateTime.ParseExact(data.Element("RateDate").Value,
                                  "yyyyMMdd", CultureInfo.InvariantCulture,DateTimeStyles.None),
                                  Rate = Convert.ToDouble(data.Element("Rate").Value),
                                  Currency = data.Element("Currency").Value
                              }).ToList();

            return list;

        }

        public static List<EXR> GetEXR(SAPbobsCOM.Company oCompany, DateTime RateDate1,DateTime RateDate2)
        {
            Utils control = new Utils();

            string sql = "select RateDate,Currency,Rate from ORTT where  RateDate between '" + RateDate1.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) + "' and '"+ RateDate2.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) + "'";
            Recordset rs = control._IDU_Recordset(oCompany, sql);

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Parse(rs.GetAsXML());
            List<EXR> list = (from data in xDoc.Element("BOM").Element("BO").Element("ORTT").Elements("row")
                              select new EXR
                              {
                                  DateRate = DateTime.ParseExact(data.Element("RateDate").Value,
                                  "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                                  Rate = Convert.ToDouble(data.Element("Rate").Value),
                                  Currency = data.Element("Currency").Value
                              }).ToList();

            return list;

        }

        public static string AddData(SAPbobsCOM.Company oCompany, EXR model)
        {
             
            RTNMANVAL rtn = new RTNMANVAL();
            int errCode; string errMessage, strResult = "";
            SAPbobsCOM.SBObob oSBob = null;
            SAPbobsCOM.Recordset rs = null;
            try
            {
                oCompany.StartTransaction();
                
                oSBob = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
                rs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                rs = oSBob.GetLocalCurrency();
                rs = oSBob.GetSystemCurrency();

                oSBob.SetCurrencyRate(model.Currency,model.DateRate,model.Rate,true);
                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);

                strResult = "OK";
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
    }
}