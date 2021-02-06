using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using CaptchaMvc.HtmlHelpers;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.Utilities.Extensions.String;
using CelloStore.ViewModels;
using CelloStore.ViewModels.Administration.System;
using CelloStore.ViewModels.MailTemplate;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace CelloStore.FrontEnd.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly PersonProvider personProvider;
        private readonly SecurityProvider securityProvider;
        private readonly EmailTemplateProvider emailTemplateProvider;
        private readonly Mailer mailer;

        public AccountController(
            PersonProvider personProvider, 
            SecurityProvider securityProvider, 
            EmailTemplateProvider emailTemplateProvider,
            Mailer mailer)
        {
            this.personProvider = personProvider;
            this.securityProvider = securityProvider;
            this.emailTemplateProvider = emailTemplateProvider;
            this.mailer = mailer;
        }

        [AllowAnonymous]
        [ImportModelStateFromTempData]
        public ActionResult Registration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Registration(FormCollection form, RegistrationViewModel model)
        {
            if (ModelState.IsValid && this.IsCaptchaValid("Invalid captcha words"))
            {
                try
                {
                    var person = new Person();
                    Mapper.DynamicMap(model, person);
                    person.IsActive = true;
                    personProvider.RegisterAccount(person);
                    TempData["Person"] = person;
                    TempData["RegistrationViewModel"] = model;
                    return RedirectToAction("RegistrationSuccess");
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            this.IsCaptchaValid("Captcha is not valid");
            return RedirectToAction("Registration");
        }

        [AllowAnonymous]
        public async Task<ActionResult> RegistrationSuccess()
        {
            var person = TempData["Person"] as Person;
            var viewModel = TempData["RegistrationViewModel"] as RegistrationViewModel;

            if(person == null || viewModel == null)
                return RedirectToAction("Index");

            ViewBag.Email = person.Email;
            viewModel.ActivationUrl = Url.Action("Activation", "Account", 
                new {area = String.Empty, email = person.Email, activationCode = person.ChallangeCode},
                 protocol: Request.Url.Scheme);

            var cacheProvider = new InvalidatingCachingProvider();
            var config = new TemplateServiceConfiguration();
            config.CachingProvider = cacheProvider;
            var service = RazorEngineService.Create(config);
            Engine.Razor = service;

            var template = emailTemplateProvider.GetEmailTemplate(EmailTemplates.Registration);
            var subject = Engine.Razor.RunCompile(template.Subject, "subject", typeof(RegistrationViewModel), viewModel);
            cacheProvider.InvalidateAll();
            var content = Engine.Razor.RunCompile(template.Content, "content", typeof(RegistrationViewModel), viewModel);
            await mailer.SendMail(
                    String.Format("{0} {1}", person.FirstName, person.LastName), 
                    person.Email,
                    template.Bcc,
                    subject,
                    content);

            return View();
        }

       

        [HttpGet]
        public ActionResult Index()
        {
            var model = new MyAccountViewModel();
            var person = personProvider.GetPerson(User.Identity.Name);
            model.PersonInfo = person;
            return View(model);
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Edit()
        {
            var model = new MyAccountEditViewModel();
            var person = personProvider.GetPerson(User.Identity.Name);
            Mapper.DynamicMap(person, model);
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Edit(FormCollection form, MyAccountEditViewModel model)
        {
            var person = personProvider.GetPerson(User.Identity.Name);
            Mapper.DynamicMap(model, person);
            try
            {
                personProvider.UpdateAccountInfo(person);
                TempData["EditSuccess"] = true;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return RedirectToAction("Edit");
        }
        
        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult ChangePassword(FormCollection form, ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (securityProvider.ValidateUser(User.Identity.Name, model.OldPassword))
                    {
                        securityProvider.ChangePassword(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("ChangePasswordSuccess");
                    }

                    ModelState.AddModelError(String.Empty, "Old password is invalid.");                    
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                    Logger.ErrorException(ex.Message, ex);                    
                }
            }
            return RedirectToAction("ChangePassword");
        }

        [HttpGet]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        [HttpGet]
        [ImportModelStateFromTempData]
        [AllowAnonymous]
        public ActionResult Activation(string email, string activationCode)
        {
            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(activationCode))
            {
                if(email.IsValidEmailAddress() && securityProvider.ValidateRegistration(email.Trim(), activationCode.Trim()))
                {
                    securityProvider.ActivateAccount(email.Trim(), activationCode.Trim());
                    return RedirectToAction("ActivationSuccess");
                }
            }
            return View();
        }

        [HttpPost]
        [ExportModelStateToTempData]
        [AllowAnonymous]
        public ActionResult Activation(FormCollection form)
        {
            string email = form["Email"];
            string activationCode = form["ActivationCode"];
            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(activationCode))
            {
                if(email.IsValidEmailAddress() && securityProvider.ValidateRegistration(email.Trim(), activationCode.Trim()))
                {
                    securityProvider.ActivateAccount(email.Trim(), activationCode.Trim());
                    return RedirectToAction("ActivationSuccess");
                }
            }
            ModelState.AddModelError(String.Empty, "Sorry we cannot activate your account, please try again.");
            return RedirectToAction("Activation");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ActivationSuccess()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResendActivationCode()
        {
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResendActivationCode(FormCollection form)
        {
            try
            {
                var viewModel = new RegistrationViewModel();
                var person = personProvider.GetPerson(form["email"]);
                if(person != null)
                {
                    if(person.ActivatedWhen.HasValue)
                    {
                        return Json(new
                        {
                            Success = false,
                            ExceptionMessage = "Account " + form["email"] + " has been activated already."
                        });
                    }

                    Mapper.DynamicMap(person, viewModel);

                    viewModel.ActivationUrl = Url.Action("Activation", "Account",
                        new {area = String.Empty, email = viewModel.Email, activationCode = viewModel.ChallangeCode}, 
                        protocol: Request.Url.Scheme);

                    var cacheProvider = new InvalidatingCachingProvider();
                    var config = new TemplateServiceConfiguration();
                    config.CachingProvider = cacheProvider;
                    var service = RazorEngineService.Create(config);
                    Engine.Razor = service;

                    cacheProvider.InvalidateAll();
                    var template = emailTemplateProvider.GetEmailTemplate(EmailTemplates.Registration);
                    var subject = Engine.Razor.RunCompile(template.Subject, "subject", typeof (RegistrationViewModel),
                        viewModel);
                    var content = Engine.Razor.RunCompile(template.Content, "content", typeof (RegistrationViewModel),
                        viewModel);
                    await mailer.SendMail(
                        String.Format("{0} {1}", viewModel.FirstName, viewModel.LastName),
                        viewModel.Email,
                        template.Bcc,
                        subject,
                        content);
                }
                else
                {
                    return Json(new
                    {
                        Success = false,
                        ExceptionMessage =
                            "Cannot find registration information for " + form["email"]
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    ExceptionMessage = ex.Message
                });
            }
            return Json(new
            {
                Success = true                
            });
        }
    }
 
}