using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.AuthorizationFilter
{
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //assume that we have created a cookie and sent the same to the browser. Then the browser stored the cookies into the browser memory. 
            // Once the cookie is sent the browser, same cookie will automatically be submitted for all the subsequent request to the server.
            if (context.HttpContext.Request.Cookies.ContainsKey("Auth-Key") == false)
            {
                //as soon as we set an not null value to 'context.Result' , that means we are short-circuiting the request pipeline and the request will not be passed to the controller.
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            //Suppose the requirement is 'Auth-Key' value should be "A100"
            if(context.HttpContext.Request.Cookies["Auth-Key"] != "A100")
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }
    }
}
