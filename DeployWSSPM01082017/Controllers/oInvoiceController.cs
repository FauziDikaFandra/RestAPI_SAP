using IDU_REST.Models;
using IDU_REST.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IDU_REST.Logic.SAP;
using IDU_REST.Logic.Connection;

namespace IDU_REST.Controllers
{
    [BasicAuthenticationFilter]
    public class oInvoiceController : ApiController
    {
        public string Get()
        {
            return "Function is not available";
        }
        
        // GET api/values/5
        public RTNVAL Get(int id)
        {
            RTNVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                ARINV_HEADER model = Logic.SAP.SAP_ARINV.GetData(oCompany, id);
                int count = 0;
                string message = "No Data Found";

                if (model.DocEntry != "")
                {
                    count = 1; message = "Get Data Successfull";
                }

                returnVal = new ARINVOICESKALARVAL()
                {
                    errorCode = "0",
                    message = message,
                    values = model,
                    recordCount = count
                };
            }
            catch (Exception e)
            {
                returnVal = new ARINVOICESKALARVAL()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }
            return returnVal;
        }

        // POST api/values
        public RTNMANVAL Post([FromBody]ARINV_HEADER value)
        {
            RTNMANVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;
            string newKey = "";

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                newKey = Logic.SAP.SAP_ARINV.AddData(oCompany, value);

                returnVal = new RTNMANVAL()
                {
                    errorCode = "0",
                    message = "Data has beed added",
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

            return returnVal;
        }

        // PUT api/values/5
        public string Put(int id, [FromBody]ARINV_HEADER value)
        {
            return "Function is not available";
        }

        // DELETE api/values/5
        public string Delete(int id)
        {
            return "Function is not available";
        }
    }
}