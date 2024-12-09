using Azure.Core;
using ChatBot.AppCode;
using ChatBot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ChatBot.Controllers
{

    [ServiceFilter(typeof(SessionAdmin))]
    public class AIController : Controller
    {

        //To call the API
        private readonly HttpClient _httpClient;
        private readonly dynamic baseUrl;
        IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionService _sessionService;
        public AIController(ISessionService sessionService, HttpClient httpclient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpclient;
            _sessionService = sessionService;
            _httpContextAccessor = httpContextAccessor;
            var request = _httpContextAccessor.HttpContext.Request;
            baseUrl = $"{request.Scheme}://{request.Host.Value}/"; _httpClient.BaseAddress = new Uri(baseUrl);
        }



        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AIDashBoard()
        {
            bool res = false;
            string msg = string.Empty;
            try
            {
                AIChat pchat = new AIChat();

                var UserName = _sessionService.GetString("UserName");
                var UserId = _sessionService.GetInt32("UserID");

                pchat.UserID = (int)UserId;
                string apiUrl = baseUrl + "api/User/UserRecordGet";
                string json = JsonConvert.SerializeObject(pchat);

                var token = _sessionService.GetString("JWTToken");
                if (!string.IsNullOrEmpty(token))
                {

                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                    request.Headers.Add("Authorization", "Bearer " + token);

                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Content = content;

                    HttpResponseMessage response = await _httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {

                    }
                }

                return View(pchat);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                res = false;
                return RedirectToAction("Error", "Home");
            }
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
                _sessionService.SetString("LoginAuth", "Auth");
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
