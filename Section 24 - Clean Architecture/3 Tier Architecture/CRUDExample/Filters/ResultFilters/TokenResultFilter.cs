using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters
{
    public class TokenResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            //this cookie will be sent to the browser. The browser will store the cookie into the browser memory.
            context.HttpContext.Response.Cookies.Append("Auth-Key", "A100");
        }
    }
}
