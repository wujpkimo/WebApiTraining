using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ProductsApp.Models
{
    public class MyExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = new System.Net.Http.HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new ObjectContent<MyHttpError>(new MyHttpError()
                {
                    Error_Code = 1,
                    Message = "My Exception"
                }, GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
        }
    }
}