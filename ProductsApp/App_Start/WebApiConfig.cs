using Newtonsoft.Json.Serialization;
using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ProductsApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            config.Filters.Add(new MyExceptionAttribute());

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;

            //API回傳的json強制第一碼為小寫
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}