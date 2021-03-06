using System;
using FindFriend.Business.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FindFriend.Web.Filters
{
    public class AddExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                ArgumentNullException => new NotFoundObjectResult(context.Exception.Message),
                AccessException => new BadRequestObjectResult(context.Exception.Message),
                ArgumentOutOfRangeException => new BadRequestObjectResult(context.Exception.Message),
                ArgumentException => new BadRequestObjectResult(context.Exception.Message),
                _ => new BadRequestObjectResult(
                    $"Unhandled error occured. {context.Exception}: {context.Exception.Message}")
            };
            base.OnException(context);
        }
    }
}