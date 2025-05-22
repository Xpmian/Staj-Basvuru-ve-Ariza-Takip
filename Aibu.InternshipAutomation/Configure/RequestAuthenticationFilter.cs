using Aibu.InternshipAutomation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aibu.InternshipAutomation.Configure
{
    public class RequestAuthenticationFilter : IActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public RequestAuthenticationFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            /*var isLoggedIn = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

            if (!isLoggedIn)
            {
                if (context.Controller.GetType() != typeof(AccountController))
                {
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                }
            }*/
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
