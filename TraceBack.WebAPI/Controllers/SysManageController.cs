using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraceBack.Model;

namespace TraceBack.WebAPI.Controllers
{
    public class SysManageController : ApiController
    {
        [HttpPost]
        public string SysManage(WebApiPara objPara)
        {
            try
            {
                return BLL.SysManage.SysManageAPI(objPara);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(
                        new WebApiResult
                        {
                            Result = 0,
                            Data = null,
                            Message = ex.Message
                        }
                    );
            }
        }
        public string Options()
        {
            return null; // HTTP 200 response with empty body
        }
    }
}