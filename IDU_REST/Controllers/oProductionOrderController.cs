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
    public class oProductionOrderController :  ApiController

    {
        //
        // GET: /oProductionOrder/

        public string Get()
        {
            return "Function is not available";
        }

        public string Get(int id)
        {
            return "Function is not available";
        }
        // GET api/values/5
        public RTGIPROD Get(DateTime FromDate, DateTime ToDate)
        {
            RTGIPROD returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);
                SAP_PROD GetProd = new SAP_PROD();
                List<PROD> model =  GetProd.GetDataList(oCompany, FromDate,ToDate);
                int count = 0;
                string message = "No Data Found";

                if (model.Count != 0)
                {
                    count = 1; message = "Get Data Successfull";
                }

                returnVal = new RTGIPROD()
                {
                    errorCode = "0",
                    message = message,
                    valueLis = model,
                    RecordCount = model.Count
                };
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

        public string Post()
        {
            return "Function is not available";
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
