using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IDU_REST.Controllers
{
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
        public void Post([FromBody]string value)
        {
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
