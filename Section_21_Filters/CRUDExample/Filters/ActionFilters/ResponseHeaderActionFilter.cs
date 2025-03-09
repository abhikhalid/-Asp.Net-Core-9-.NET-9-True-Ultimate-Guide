using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string _key;
        private readonly string Value;

        public int Order { get; set; }

        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value, int order)
        {
            this._logger = logger;
            this._key = key;
            this.Value = value;
            Order = order;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method - before", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            await next(); // call the next subsequent filter which is added in the chain. If there is no subseqent filter, it will call the action method.  

            _logger.LogInformation("{FilterName}.{MethodName} method - after", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            context.HttpContext.Response.Headers[_key] = Value;
        }
    }
}
