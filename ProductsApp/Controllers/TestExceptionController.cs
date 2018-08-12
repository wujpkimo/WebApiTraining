using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ProductsApp.Controllers
{
    [MyException]
    [RoutePrefix("TestException")]
    public class TestExceptionController : ApiController
    {
        // GET: TestException
        [HttpGet]
        [Route("")]
        public IHttpActionResult Test()
        {
            throw new Exception("TEST");
        }
    }
}