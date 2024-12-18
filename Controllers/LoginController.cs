﻿using ChatBot.Models;
using ChatBot.AppCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Antiforgery;  
using System;

namespace ChatBot.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly dynamic baseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionService _sessionService;
        private readonly IAntiforgery _antiforgery;  

        public LoginController(
            HttpClient httpclient,
            IHttpContextAccessor httpContextAccessor,
            ISessionService sessionService,
            IAntiforgery antiforgery)  
        {
            _httpClient = httpclient;
            _sessionService = sessionService;
            _httpContextAccessor = httpContextAccessor;
            _antiforgery = antiforgery; 
            var request = _httpContextAccessor.HttpContext.Request;
            baseUrl = $"{request.Scheme}://{request.Host.Value}/";
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            var LoginAuth = _sessionService.GetString("LoginAuth");
            if (!string.IsNullOrWhiteSpace(LoginAuth))
            {
                TempData["LoginAuth"] = "Please login!";
            }

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
                
                var tokenFromForm = Request.Form["__RequestVerificationToken"].FirstOrDefault();
                if (string.IsNullOrWhiteSpace(tokenFromForm))
                {
                    Message = "CSRF token missing.";
                    TempData["ErrorMessage"] = Message;
                    return RedirectToAction("Contact", "Home");
                }

               
                await _antiforgery.ValidateRequestAsync(HttpContext);

                string Url = baseUrl + "api/LoginAPI/RegisterUser";
                string Json = JsonConvert.SerializeObject(pAIUser);
                StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

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

            return RedirectToAction("AIDashBoard", "AI");
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> LoginSubmit(AIUser pAIUser)
        {
            string msg = "";
            bool res = false;
            string Message = "";
            string apiUrl = baseUrl + "api/LoginAPI/LoginAuthenticate";

            string Json = JsonConvert.SerializeObject(pAIUser);
            StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

            
            var tokenFromForm = Request.Form["__RequestVerificationToken"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(tokenFromForm))
            {
                Message = "CSRF token missing.";
                TempData["ErrorMessage"] = Message;
                return RedirectToAction("Contact", "Home");
            }

            
            await _antiforgery.ValidateRequestAsync(HttpContext);

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl) { Content = content };
            var antiforgeryCookie = Request.Cookies[".AspNetCore.Antiforgery"];
            if (!string.IsNullOrWhiteSpace(tokenFromForm)) { request.Headers.Add("X-XSRF-TOKEN", tokenFromForm); }
            if (!string.IsNullOrWhiteSpace(antiforgeryCookie)) { request.Headers.Add("Cookie", $".AspNetCore.Antiforgery={antiforgeryCookie}"); }

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                dynamic resBody = await response.Content.ReadAsStringAsync();
                AIUser obj = JsonConvert.DeserializeObject<AIUser>(resBody);
                if (obj.Status == false)
                {
                    TempData["errorMessage"] = "Please enter the Correct Username or Password";
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    _sessionService.SetString("JWTToken", obj.Token);
                    _sessionService.SetString("UserName", obj.UserName);
                    _sessionService.SetInt32("UserID", obj.AIUserID);
                }
            }

            return RedirectToAction("AIDashBoard", "AI");
        }

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
