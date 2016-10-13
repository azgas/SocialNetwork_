using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace SocNet.Controllers
{
    public class TestRepoController : ApiController
    {
        [HttpPost]
        public /*IHttpActionResult*//*JsonResult*//*ActionResult*/JsonResult<string> Data([FromBody]testClass form)
        {
            string data;
            if (form.zminna == 1) {
                data = DateTime.Now.Month.ToString();
            }else
            {
                data = DateTime.Now.Year.ToString();
            }
            return Json(data);
        }
    }
    public class testClass
    {
        public int zminna { get; set; }
    }
}
