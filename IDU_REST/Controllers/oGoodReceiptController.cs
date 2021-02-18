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
    public class oGoodReceiptController : ApiController
    {
        // GET api/ogoodreceipt
        public string Get()
        {
            //return new string[] { "value1", "value2" };
            return "Function is not available";
        }

        // GET api/ogoodreceipt/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/ogoodreceipt
        public RTNMANVAL Post([FromBody]GR_HEADER value)
        {
            RTNMANVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;
            string newKey = "";

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                newKey = Logic.SAP.SAP_GR.AddData(oCompany, value);

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

        // PUT api/ogoodreceipt/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/ogoodreceipt/5
        public void Delete(int id)
        {
        }
    }
}
