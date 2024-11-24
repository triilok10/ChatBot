using ChatBot.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    public class LoginController : Controller
    {
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
