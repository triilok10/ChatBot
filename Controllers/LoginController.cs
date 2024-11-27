using ChatBot.Models;
using ChatBot.AppCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System;

namespace ChatBot.Controllers
{
    public class LoginController : Controller
    {


        #region "Constructor"
        //To call the API
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
        public async Task<IActionResult> RegistrationSubmit(AIUser pAIUser)
        {
            bool res = false;
            string Message = "";
            int? UserID = 0;
            string UserName = "";
            try
            {
                string Url = baseUrl + "api/LoginAPI/RegisterUser";
                string Json = JsonConvert.SerializeObject(pAIUser);

                StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

                //Checking the AntiFrogery Token
                var tokenFromForm = Request.Form["__RequestVerificationToken"].FirstOrDefault();
                if (string.IsNullOrWhiteSpace(tokenFromForm))
                {
                    Message = "CSRF token missing.";
                    TempData["ErrorMessage"] = Message; return RedirectToAction("Contact", "Home");
                }
                var request = new HttpRequestMessage(HttpMethod.Post, Url) { Content = content };
                var antiforgeryCookie = Request.Cookies[".AspNetCore.Antiforgery"];
                if (!string.IsNullOrWhiteSpace(tokenFromForm)) { request.Headers.Add("X-XSRF-TOKEN", tokenFromForm); }
                if (!string.IsNullOrWhiteSpace(antiforgeryCookie)) { request.Headers.Add("Cookie", $".AspNetCore.Antiforgery={antiforgeryCookie}"); }

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    dynamic responseBody = await response.Content.ReadAsStringAsync();
                    AIUser obj = JsonConvert.DeserializeObject<AIUser>(responseBody);

                    if (obj.Status == true)
                    {
                        _sessionService.SetString("UserName", obj.UserName);
                        _sessionService.SetInt32("UserID", obj.AIUserID);
                    }
                }
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
