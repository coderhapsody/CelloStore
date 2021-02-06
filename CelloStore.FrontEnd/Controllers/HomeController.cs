using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using CaptchaMvc.HtmlHelpers;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.FrontEnd.Helpers;
using CelloStore.Providers;
using CelloStore.ViewModels;
using CelloStore.ViewModels.MailTemplate;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace CelloStore.FrontEnd.Controllers
{
    public class HomeController : BaseController
    {
        private readonly SecurityProvider securityProvider;
        private readonly ShowcaseProvider showcaseProvider;
        private readonly AreaProvider areaProvider;
        private readonly CartManager cartManager;
        private readonly Mailer mailer;
        private readonly EmailTemplateProvider emailTemplateProvider;

        public HomeController(
            SecurityProvider securityProvider,
            ShowcaseProvider showcaseProvider,
            AreaProvider areaProvider,
            CartManager cartManager,
            EmailTemplateProvider emailTemplateProvider,
            Mailer mailer)
        {
            this.securityProvider = securityProvider;
            this.showcaseProvider = showcaseProvider;
            this.areaProvider = areaProvider;
            this.cartManager = cartManager;
            this.emailTemplateProvider = emailTemplateProvider;
            this.mailer = mailer;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var areaDefault = areaProvider.DefaultArea;
            TempData["areaId"] = areaDefault.Id;
            TempData["areaName"] = areaDefault.Name.Replace(" ","");
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult AccountInfo()
        {
            var model = new AccountInfoViewModel();
            var person = securityProvider.GetPerson(User.Identity.Name);
            model.IsLoggedIn = !String.IsNullOrEmpty(User.Identity.Name);
            if (model.IsLoggedIn)
            {
                model.IsAdministrator = securityProvider.IsAdministrator(User.Identity.Name);
                model.AccountName = String.Format("{0} {1}", person.FirstName, person.LastName);
            }
            return PartialView("_AccountInfo", model);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Showcase()
        {
            var ads = showcaseProvider.GetAdvertisements();
            return PartialView("_Showcase", ads);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Bottom()
        {
            return PartialView("_Bottom");
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Footer()
        {
            return PartialView("_Footer");
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (!String.IsNullOrEmpty(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            var userName = form["UserName"];
            var password = form["Password"];
            var valid = securityProvider.ValidateUser(userName, password);
            if (valid)
            {
                if(securityProvider.IsAccountActivated(userName))
                {
                    FormsAuthentication.SetAuthCookie(userName, false);
                    securityProvider.LogInUser(userName);
                    cartManager.SetCartUserName();
                    return Json(new
                    {
                        Authenticated = true,
                        Activated = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        Authenticated = true,
                        Activated = false
                    });
                }
            }
            return Json(new
            {
                Authenticated = false,
                Activated = false
            });
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Cart()
        {
            return PartialView("_Cart");
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var forgetPasswordValidation = securityProvider.FindUserByEmailAddress(model.Email);
            if (forgetPasswordValidation != null && this.IsCaptchaValid("Invalid captcha words"))
            {
                var newPassword = securityProvider.ResetPassword(forgetPasswordValidation, model.Email);
                var viewModel = new ForgetPasswordMailViewModel();
                viewModel.CustomerName = String.Format("{0} {1}", 
                    forgetPasswordValidation.FirstName,
                    forgetPasswordValidation.LastName);
                viewModel.NewPassword = newPassword;

                
                var template = emailTemplateProvider.GetEmailTemplate(EmailTemplates.ForgetPassword);

                var config = new TemplateServiceConfiguration();
                using (var service = RazorEngineService.Create(config))
                {
                    var subject = service.RunCompile(template.Subject, "subject", typeof (ForgetPasswordMailViewModel),
                        viewModel);
                    var content = service.RunCompile(template.Content, "content", typeof (ForgetPasswordMailViewModel),
                        viewModel);
                    await mailer.SendMail(model.Email, template.Bcc, subject, content);
                }
                TempData["EmailAddress"] = model.Email;
                return RedirectToAction("ForgetPasswordSuccess");
            }
            ModelState.AddModelError("ForgetPassword", "Sorry we are unable to validate your account. Please try again.");
            return RedirectToAction("ForgetPassword");
        }

        [HttpGet]
        public ActionResult ForgetPasswordSuccess()
        {
            ViewBag.EmailAddress = TempData["EmailAddress"];
            return View();
        }
    }

}