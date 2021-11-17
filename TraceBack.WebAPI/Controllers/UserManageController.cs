using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;
using TraceBack.Model;

namespace TraceBack.WebAPI.Controllers
{
    public class UserManageController : ApiController
    {
        [HttpPost]
        public string UserManage(WebApiPara objPara)
        {
            try
            {
                return BLL.UserManage.UserManageAPI(objPara);
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