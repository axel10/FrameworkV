using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Root.Entity;

namespace Root
{
    public class BaseController : Controller
    {

        private readonly IBootOptions _bootOptions;


        public BaseController()
        {
            _bootOptions = IoCHelper.Resolve<IBootOptions>();
        }


        protected ActionResult Result(ActionOptions options, object model = null,JsonSerializerSettings settings=null)
        {
            if (settings==null)
            {
                settings = new JsonSerializerSettings();
            }

            if (_bootOptions.IsCamelCasePropertyNames)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }


            if (!options.ResultType.HasValue)
            {
                options.ResultType = ResultType.Json;
            }

            switch (options.ResultType)
            {
                case ResultType.Json:
                    Response.ContentType = "application/json; charset=utf-8";

                    if (model is IEnumerable data)
                    {
                        var total = HttpContext.Items["Total"];
                        if (total == null)
                        {
                            return Content(JsonConvert.SerializeObject(new Response(data), settings));
                        }

                        var resultDto = new ResultDto
                        {
                            Data = data,
                            Total = (int) total
                        };
                        return Content(JsonConvert.SerializeObject(new Response(resultDto), settings));
                    }
                    else
                    {
                        if (model == null)
                        {
                            return Content(JsonConvert.SerializeObject(new Response()));
                        }
                        return Content(JsonConvert.SerializeObject(new Response(model), settings));
                    }

                case ResultType.View:
                    return View(model);
            }

            return null;
        }

/*        protected ActionResult Result(object model = null)
        {
            //            var resultType = (ResultType) HttpContext.Request.Query["ResultType"].ToString();
            if (HttpContext.Request.Query["ResultType"] is int)
            {
                HttpContext.Request.Query["ResultType"]
            }
        }*/


        protected void SafeExecute(Action doing)
        {
            try
            {
                doing();
            }
            catch (InputException e)
            {
                HttpContext.Items["InputException"] = true;
                HttpContext.Items["Error"] = e;
                throw;
            }
        }

        protected Task<IActionResult> SafeExecute(Func<Task<IActionResult>> doing)
        {
            try
            {
                return doing();
            }
            catch (InputException e)
            {
                HttpContext.Items["InputException"] = true;
                HttpContext.Items["Error"] = e;
                throw;
            }
        }

        protected IActionResult SafeExecute(Func<IActionResult> doing, ActionOptions actionOptions)
        {
            try
            {
                return doing();
            }
            catch (InputException e)
            {
                HttpContext.Items["InputException"] = true;
                HttpContext.Items["Error"] = e;
                HttpContext.Items["actionOptions"] = actionOptions;
                throw;
            }
        }

        protected ActionResult SafeExecute(Func<ActionResult> doing)
        {
            try
            {
                return doing();
            }
            catch (InputException e)
            {
                HttpContext.Items["InputException"] = true;
                HttpContext.Items["Error"] = e;

                throw;
            }
        }

        protected IActionResult SafeExecute(Func<object> doing)
        {
            try
            {
                return (IActionResult) doing();
            }
            catch (InputException e)
            {
                HttpContext.Items["InputException"] = true;
                HttpContext.Items["Error"] = e;
                throw;
            }
        }

/*        public override void OnActionExecuted(ActionExecutedContext context)
        {
/*

            context.HttpContext.Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:8080";
            context.HttpContext.Response.Headers["Access-Control-Allow-Headers"] = "Origin, X-Requested-With, Content-Type, Accept, If-Modified-Since";
            context.HttpContext.Response.Headers["Access-Control-Allow-Credentials"] = "true";#1#

            base.OnActionExecuted(context);
        }*/
    }
}