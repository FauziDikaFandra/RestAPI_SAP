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
    public class oInventoryTransferRequestController : ApiController
    {
        //
        // GET: /oInventoryTransferRequest/

        public string Get()
        {
            //return new string[] { "value1", "value2" };
            return "Function is not available";
        }

        // GET api/ogoodreceipt/5
        public RTITR Get(int id)
        {
            RTITR returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);
                SAP_ITR GetProd = new SAP_ITR();
                List<ITR> model = GetProd.GetData(oCompany, id);
                int count = 0;
                string message = "No Data Found";

                if (model.Count != 0)
                {
                    count = 1; message = "Get Data Successfull";
                }

                returnVal = new RTITR()
                {
                    errorCode = "0",
                    message = message,
                    valueList = model,
                    RecordCount = model.Count
                };
            }
            catch (Exception e)
            {
                returnVal = new RTITR()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }

            return returnVal;
        }
        public RTITR Get(DateTime FromDate, DateTime ToDate)
        {
            RTITR returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);
                SAP_ITR GetProd = new SAP_ITR();
                List<ITR> model  = GetProd.GetData(oCompany, FromDate, ToDate);
                int count = 0;
                string message = "No Data Found";

                if (model.Count != 0)
                {
                    count = 1; message = "Get Data Successfull";
                }

                returnVal = new RTITR()
                {
                    errorCode = "0",
                    message = message,
                    valueList = model,
                    RecordCount = model.Count
                };
            }
            catch (Exception e)
            {
                returnVal = new RTITR()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }

            return returnVal;
        }
        // POST api/oInventoryTransferRequest
        public RTITR Post([FromBody]ITR value)
        {
            RTITR returnVal = null;
            SAPbobsCOM.Company oCompany = null;
            string newKey = "";

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                newKey = Logic.SAP.SAP_ITR.AddData(oCompany, value);

                returnVal = new RTITR()
                {
                    errorCode = "0",
                    message = "Data has beed added",
                    value = newKey,
                    Status = "OPEN",
                    FromWarehouse = value.FromWarehouseCode,
                    ToWarehouse = value.lines[0].ToWarehouseCode.ToString()
                };
            }
            catch (Exception e)
            {
                returnVal = new RTITR()
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
