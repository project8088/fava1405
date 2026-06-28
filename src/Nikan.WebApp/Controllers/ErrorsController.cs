using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;

namespace Nikan.WebApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]

    public class ErrorsController : ControllerBase
    {

        [Route("error")]

        public  ApiResult  Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; // Your exception
            var code = 500; // Internal Server Error by default

             
            Response.StatusCode = code; // You can use HttpStatusCode enum instead

            return new ApiResult(false,ApiResultStatusCode.ServerError,   exception.Message); // Your error model
        }


    }
}