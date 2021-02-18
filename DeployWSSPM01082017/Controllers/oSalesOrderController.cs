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

    public class oSalesOrderController : ApiController
    {
        // GET api/osalesorder
        public string Get()
        {
            //return new string[] { "value1", "value2" };
            return "Function is not available";
        }

        // GET api/osalesorder/5
        public RTNVAL Get(int id)
        {
            RTNVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                SO_HEADER model = Logic.SAP.SAP_SO.GetData(oCompany, id);
                int count = 0;
                string message = "No Data Found";

                if (model.DocEntry != "")
                {
                    count = 1; message = "Get Data Successfull";
                }

                returnVal = new SOSKALARVAL()
                {
                    errorCode = "0",
                    message = message,
                    values = model,
                    recordCount = count
                };
            }
            catch (Exception e)
            {
                returnVal = new SOSKALARVAL()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }
            return returnVal;
        }

        // POST api/osalesorder
        //public void Post([FromBody]string value)
        //{
        //}
        public RTNMANVAL Post([FromBody]SO_HEADER  value)
        {
            RTNMANVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;
            string newKey = "";

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                newKey = Logic.SAP.SAP_SO.AddData(oCompany, value);

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
        // PUT api/osalesorder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/osalesorder/5
        public void Delete(int id)
        {
        }
    }
}
