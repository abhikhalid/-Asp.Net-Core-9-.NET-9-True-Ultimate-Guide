﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEnvironment) //constructor injection
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }


        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Exception filter {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}",nameof(HandleExceptionFilter),
                nameof(OnException), context.Exception.GetType().ToString(),context.Exception.Message);

           if(_hostEnvironment.IsDevelopment())
            {
                context.Result = new JsonResult(new { error = context.Exception.Message });
            }
            else
            {
                context.Result = new JsonResult(new { error = "An error occurred. Please try again later." });
            }
        }
    }
}
