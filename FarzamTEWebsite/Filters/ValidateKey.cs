using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FarzamTEWebsite.Filters
{
    public class ValidateKey : ActionFilterAttribute
    {
        private readonly string _key;
        public ValidateKey(string Key)
        {
            _key = Key;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            string api_key = context.HttpContext.Request.Headers["api-key"].ToString();
            if (String.IsNullOrEmpty(api_key) || api_key != _key)
                context.Result = new BadRequestObjectResult("You Don't Have a Permission to Access This Api");
            base.OnResultExecuting(context);
        }
    }
}
