using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TraceBack.WebAPI.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public string Test()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
