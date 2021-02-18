using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IDU_REST.Controllers
{
    public class oDeliverController : ApiController
    {
        // GET api/odeliver
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/odeliver/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/odeliver
        public void Post([FromBody]string value)
        {
        }

        // PUT api/odeliver/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/odeliver/5
        public void Delete(int id)
        {
        }
    }
}
