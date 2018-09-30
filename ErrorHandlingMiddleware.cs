using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Root.Entity;

namespace Root
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly IHostingEnvironment Env = IoCHelper.Resolve<IHostingEnvironment>();


        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            bool isDev = Env.IsDevelopment();

/*            var actionOptions = context.Request.Query[nameof(ActionOptions)].ToString();

            ResultType resultType;

            if (!string.IsNullOrEmpty(actionOptions))
            {
                Enum.TryParse("")
            }*/

            context.Response.ContentType = "application/json";

            if (exception is InputException inputException)
            {
                if (!inputException.StatusCode.HasValue)
                {
                    inputException.StatusCode = 499;//其他错误
                }

                context.Response.StatusCode = inputException.StatusCode.GetValueOrDefault();
                if (isDev)
                {
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(new Response(inputException.StackTrace, inputException.Message,inputException.StatusCode.Value)));
                }
                return context.Response.WriteAsync(JsonConvert.SerializeObject(new Response("",inputException.Message,inputException.StatusCode.Value)));
            }
            context.Response.StatusCode = 500;

            if (isDev)
            {
                return context.Response.WriteAsync(JsonConvert.SerializeObject(new Response(exception.StackTrace,
                    exception.Message, (int) ResponseType.ServerError)));
            }
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new Response("","网络忙",(int)ResponseType.ServerError)));
        }
    }
}