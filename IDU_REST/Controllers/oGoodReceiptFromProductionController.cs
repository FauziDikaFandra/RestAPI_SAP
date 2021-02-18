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
    public class oGoodReceiptFromProductionController : ApiController
    {
        //
        // GET: /oGoodReceiptFromProduction/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public string Get()
        {
            return "Function is not available";
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "Function is not available";
        }

        public RTGIPROD Post([FromBody]GRFROMPROD value)
        {
            RTGIPROD returnVal = new RTGIPROD();
            SAPbobsCOM.Company oCompany = null;
            string newKey = "";
            
            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);
                    
                SAP_GRFROMPROD trans = new SAP_GRFROMPROD();

                    returnVal = trans.AddData(oCompany, value);

                
                    returnVal.errorCode = "0";
                    returnVal.message = "Data has beed added";
                    returnVal.value = newKey;
                 
            }
            catch (Exception e)
            {
                returnVal = new RTGIPROD()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }

            return returnVal;
        }

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
