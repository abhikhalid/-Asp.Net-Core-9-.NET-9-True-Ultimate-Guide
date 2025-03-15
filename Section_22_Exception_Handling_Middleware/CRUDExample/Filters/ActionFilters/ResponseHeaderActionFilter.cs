using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        bool IFilterFactory.IsReusable => false; //Currently ignore it.

        private string? Key { get; set; }
        private string? Value { get; set; }
        private int Order { get; set; }

        public ResponseHeaderFilterFactoryAttribute(string key, string value, int order)
        {
            this.Key = key;
            this.Value = value;
            this.Order = order;
        }


        //Controller -> FilterFactory -> Filter (Controller will call FilterFactory to create the filter object)
        //will be called by the framework to create the filter object (run-time)
        IFilterMetadata IFilterFactory.CreateInstance(IServiceProvider serviceProvider)
        {
            //return filter object
            var filter =  serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
            filter.Key = Key;
            filter.Value = Value;
            filter.Order = Order;

            return filter;
        }
    }


    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        public ILogger<ResponseHeaderActionFilter> _logger;
        public string Key { get; set; }
        public string Value { get; set; }

        public int Order { get; set; }

        //no need of initializing them here
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method - before", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            await next(); // call the next subsequent filter which is added in the chain. If there is no subseqent filter, it will call the action method.  

            _logger.LogInformation("{FilterName}.{MethodName} method - after", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            context.HttpContext.Response.Headers[Key] = Value;
        }
    }
}
