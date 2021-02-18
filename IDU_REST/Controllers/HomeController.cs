using IDU_REST.Models;
using IDU_REST.Logic;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IDU_REST.Logic.SAP;
using IDU_REST.Logic.Connection;

namespace IDU_REST.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        //public string Get()
        //{
        //    return "Function is not available";
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "Function is not available";
        //}

        //public string Post()
        //{
        //    return "Function is not available";
        //}
        //public string Put(int id, [FromBody]BP value)
        //{
        //    return "Function is not available";
        //}

        //// DELETE api/values/5
        //public string Delete(int id)
        //{
        //    return "Function is not available";
        //}
    }
}
