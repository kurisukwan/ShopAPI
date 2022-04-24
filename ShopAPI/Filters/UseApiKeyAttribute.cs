using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopAPI.Filters
{
    public class UseApiKeyAttribute : Attribute, IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var apiKey = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>("ApiKey");

            if (!context.HttpContext.Request.Query.TryGetValue("code", out var code))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (!apiKey.Equals(code))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
