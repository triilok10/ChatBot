using ChatBot.Models;
using ChatBot.AppCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ChatBot.Controllers
{
    public class LoginController : Controller
    {


        #region "Constructor"

        private readonly HttpClient _httpClient;
        private readonly dynamic baseUrl;
        IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionService _sessionService;
        public LoginController(HttpClient httpclient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService)
        {
            _httpClient = httpclient;
            _sessionService = sessionService;
            _httpContextAccessor = httpContextAccessor;
            var request = _httpContextAccessor.HttpContext.Request;
            baseUrl = $"{request.Scheme}://{request.Host.Value}/"; _httpClient.BaseAddress = new Uri(baseUrl);
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        #region "Submit Registration Form"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegistrationSubmit(AIUser pAIUser)
        {
            bool res = false;
            string msg = "";
            try
            {

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("", "");
        }
        #endregion

        public PartialViewResult _TermsCondition(string hdnAcceptValue)
        {
            if (!string.IsNullOrWhiteSpace(hdnAcceptValue))
            {
                ViewBag.hdnAcceptValue = hdnAcceptValue;
                ViewBag.hdnAcceptVal = 1;
            }
            else
            {
                ViewBag.hdnAcceptValue = 0;
                ViewBag.hdnAcceptVal = 0;
            }
            return PartialView();
        }
    }
}
