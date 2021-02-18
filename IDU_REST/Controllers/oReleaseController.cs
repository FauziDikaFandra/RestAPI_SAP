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
    public class oReleaseController : ApiController
    {
        public string Get()
        {
            return "Function is not available";
        }

        // POST api/values
        public string Get(int id)
        {
            return "Function is not available";
        }

        // POST api/values
        public RTNMANVAL Post([FromBody]REL value)
        {
            RTNMANVAL returnVal = null;
            string newKey = "";

            try
            {
                Logic.Connection.Connection.Release(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                returnVal = new RTNMANVAL()
                {
                    errorCode = "0",
                    message = "Company connection released",
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
        public string Put(int id, [FromBody]PO_HEADER value)
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