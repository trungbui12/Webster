using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Webster.Filters
{
    public class AuthorizeManagerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (role != "Manager")
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
