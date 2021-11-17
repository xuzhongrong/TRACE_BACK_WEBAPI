using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceBack.Model;

namespace TraceBack.BLL
{
    public class StuManage
    {
        public static string StuManageAPI(WebApiPara objPara)
        {
            WebApiResult apiResult = new WebApiResult();
            switch(objPara.ApiName)
            {

            }
            return JsonConvert.SerializeObject(apiResult);
        }
    }
}
