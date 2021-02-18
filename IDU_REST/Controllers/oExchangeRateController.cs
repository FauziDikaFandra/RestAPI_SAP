using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IDU_REST.Logic.SAP;
using IDU_REST.Logic.Connection;
using IDU_REST.Models;
using IDU_REST.Logic;
using System.Globalization;
using System.Threading;
namespace IDU_REST.Controllers
{
    public class oExchangeRateController : ApiController
    {
        // GET api/exchangerate
         
            
        public RTNVAL Get()
        {
            RTNVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;
           
            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                List<EXR> list = Logic.SAP.SAP_EXR.GetEXR(oCompany);

                returnVal = new EXRVAL()
                {
                    errorCode = "0",
                    message = "Get Data Successfull",
                    values = list,
                    recordCount = list.Count
                };
            }
            catch (Exception e)
            {
                returnVal = new EXRVAL()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }

            return returnVal;
        }

        public RTNVAL Get(DateTime DateRange)
        {
            RTNVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                List<EXR> list = Logic.SAP.SAP_EXR.GetEXR(oCompany, DateRange);

                returnVal = new EXRVAL()
                {
                    errorCode = "0",
                    message = "Get Data Successfull",
                    values = list,
                    recordCount = list.Count
                };
            }
            catch (Exception e)
            {
                returnVal = new EXRVAL()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }

            return returnVal;
        }

        public RTNVAL Get(DateTime DateRange, DateTime DateRange2)
        {
            RTNVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                List<EXR> list = Logic.SAP.SAP_EXR.GetEXR(oCompany, DateRange, DateRange2);

                returnVal = new EXRVAL()
                {
                    errorCode = "0",
                    message = "Get Data Successfull",
                    values = list,
                    recordCount = list.Count
                };
            }
            catch (Exception e)
            {
                returnVal = new EXRVAL()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }

            return returnVal;
        }
        // GET api/exchangerate/5
       
        // POST api/exchangerate
        public RTNMANVAL Post([FromBody]EXR value)
        {
            RTNMANVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;
            string newKey = "";
            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                newKey = Logic.SAP.SAP_EXR.AddData(oCompany, value);

                returnVal = new RTNMANVAL()
                {
                    errorCode = "0",
                    message = "Data has beed added",
                    value = newKey
                };
            }
            catch(Exception e)
            {
                returnVal = new RTNMANVAL()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }
            return returnVal;
        }

        // PUT api/exchangerate/5
        public void Put([FromBody]EXR value)
        {
            RTNMANVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;
            string newKey = "";
            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                newKey = Logic.SAP.SAP_EXR.AddData(oCompany, value);

                returnVal = new RTNMANVAL()
                {
                    errorCode = "0",
                    message = "Data has beed updated",
                    value = newKey
                };
            }
            catch (Exception e)
            {
                returnVal = new RTNMANVAL()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }
        }

        // DELETE api/exchangerate/5
        public string Delete(int id)
        {
            return "this method Not Provided";
        }
    }
}
