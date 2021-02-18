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
    public class oGetListStockController : ApiController
    {
        //
        // GET: /GetListStock/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public RTNVAL Get(string ItemGroup, DateTime UpdateFrom, DateTime UpdateTo)
        {
            RTNVAL returnVal = null;
            SAPbobsCOM.Company oCompany = null;

            try
            {
                oCompany = Company.GetCompany(Properties.Settings.Default.StrDbServer, Properties.Settings.Default.StrDbUserName, Properties.Settings.Default.StrDbPassword,
                    Properties.Settings.Default.StrDbName, Properties.Settings.Default.StrSapB1UserName, Properties.Settings.Default.StrSapB1Password,
                    Properties.Settings.Default.StrSapB1LicenseServer);

                List<GetListStock> model = Logic.SAP.SAP_GetListStock.GetData(oCompany, ItemGroup,UpdateFrom, UpdateTo);
                int count = 0;
                string message = "No Data Found";



                returnVal = new GetListStockVal()
                {
                    errorCode = "0",
                    message = "Get Data Successfull",
                    Value = model,
                    recordCount = model.Count
                    
                };
                
            }
            catch (Exception e)
            {
                returnVal = new GetListStockVal()
                {
                    errorCode = "-1",
                    message = e.Message.ToString()
                };
            }
            return returnVal;
        }
    }
}
