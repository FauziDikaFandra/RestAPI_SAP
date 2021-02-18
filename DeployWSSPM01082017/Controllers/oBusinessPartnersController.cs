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
    public class oBusinessPartnersController : ApiController
    {
        public RTNVAL Get()
        {
            RTNVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName,Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password, 
                    Properties.Settings.Default.StrSapB1LicenseServer);

                List<BP> list = Logic.SAP.SAP_BP.GetData(oCompany);

                returnVal = new BPVAL() { errorCode = "0", message = "Get Data Successfull",
                                          values = list, recordCount = list.Count
                };
            }
            catch (Exception e)
            {
                returnVal = new BPVAL()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }

            return returnVal;
        }

        // GET api/values/5
        public RTNVAL Get(string id)
        {
            RTNVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                BP model = Logic.SAP.SAP_BP.GetData(oCompany, id);
                int count = 0;
                string message = "No Data Found";

                if (model.CardCode != "")
                {
                    count = 1; message = "Get Data Successfull";
                }

                returnVal = new BPSKALARVAL()
                {
                    errorCode = "0",
                    message = message,
                    values = model,
                    recordCount = count
                };
            }
            catch (Exception e)
            {
                returnVal = new BPSKALARVAL()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }

            return returnVal;
        }

        // POST api/values
        public string Post([FromBody]BP value)
        {
            return "Function is not available";
        }

        // PUT api/values/5
        public string Put(int id, [FromBody]BP value)
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