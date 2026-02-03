using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Webster.Filters
{
    public class AuthorizeCandidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (role != "Candidate")
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
