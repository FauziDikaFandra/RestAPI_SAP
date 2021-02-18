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
    public class oDeliveryOrderController : ApiController
    {
        // GET api/odeliverorder
        public string Get()
        {
           // return new string[] { "value1", "value2" };
            return "Function is not available";
        }

        // GET api/odeliverorder/5
        public RTNVAL Get(int id)
        {
            RTNVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                DO_HEADER model = Logic.SAP.SAP_DO.GetData(oCompany, id);
                int count = 0;
                string message = "No Data Found";

                if (model.DocEntry != "")
                {
                    count = 1; message = "Get Data Successfull";
                }

                returnVal = new DOSKALARVAL()
                {
                    errorCode = "0",
                    message = message,
                    values = model,
                    recordCount = count
                };
            }
            catch (Exception e)
            {
                returnVal = new DOSKALARVAL()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }
            return returnVal;
        }

        // POST api/odeliverorder
        public RTNMANVAL Post([FromBody]DO_HEADER value)
        {
            RTNMANVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;
            string newKey = "";

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                newKey = Logic.SAP.SAP_DO.AddData(oCompany, value);

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

        // PUT api/odeliverorder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/odeliverorder/5
        public void Delete(int id)
        {
        }
    }
}
