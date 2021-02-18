using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IDU_REST.Controllers
{
    public class oIncommingPaymentController : ApiController
    {
        // GET api/oincommingpayment
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/oincommingpayment/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/oincommingpayment
        public void Post([FromBody]string value)
        {
        }

        // PUT api/oincommingpayment/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/oincommingpayment/5
        public void Delete(int id)
        {
        }
    }
}
