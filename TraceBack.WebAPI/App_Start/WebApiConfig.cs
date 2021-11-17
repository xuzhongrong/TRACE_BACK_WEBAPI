using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TraceBack.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            //第一个参数是指定的域（www.baidu.com 多个域可以以","分隔）



            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
