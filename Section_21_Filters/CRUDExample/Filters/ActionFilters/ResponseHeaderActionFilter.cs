using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : ActionFilterAttribute
    {
        private ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string _key;
        private readonly string Value;

        //public int Order { get; set; } 

        //you can't inject constructor injection here
        //public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value)
        public ResponseHeaderActionFilter(string key, string value)
        {
            //this._logger = logger;
            this._key = key;
            this.Value = value;
            //Order = order;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //_logger.LogInformation("{FilterName}.{MethodName} method - before", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            await next(); // call the next subsequent filter which is added in the chain. If there is no subseqent filter, it will call the action method.  

            //_logger.LogInformation("{FilterName}.{MethodName} method - after", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            context.HttpContext.Response.Headers[_key] = Value;
        }
    }
}
