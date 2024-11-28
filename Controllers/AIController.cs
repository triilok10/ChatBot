using ChatBot.AppCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatBot.Controllers
{

    [ServiceFilter(typeof(SessionAdmin))]
    public class AIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AIDashBoard()
        {
            return View();
        }
    }

    public class SessionAdmin : ActionFilterAttribute
    {
        private readonly ISessionService _sessionService;

        public SessionAdmin(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? UserId = _sessionService.GetInt32("UserID");
            string Username = _sessionService.GetString("UserName");
            if (string.IsNullOrEmpty(Username) || UserId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
