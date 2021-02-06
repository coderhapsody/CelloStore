using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Administration.System;

namespace CelloStore.FrontEnd.Areas.Administration.Controllers
{
    [Authorize]
    public class SystemController : AdminController
    {
        private readonly SecurityProvider securityProvider;
        private readonly ConfigurationProvider configurationProvider;
        private readonly ProductProvider productProvider;
        private readonly ShowcaseProvider showcaseProvider;
        private readonly Mailer mailer;

        public SystemController(
            SecurityProvider securityProvider, 
            ConfigurationProvider configurationProvider, 
            ProductProvider productProvider,
            ShowcaseProvider showcaseProvider,
            Mailer mailer)
        {
            this.securityProvider = securityProvider;
            this.configurationProvider = configurationProvider;
            this.productProvider = productProvider;
            this.showcaseProvider = showcaseProvider;
            this.mailer = mailer;
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
                    if (securityProvider.ValidateUser(User.Identity.Name, model.NewPassword))
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

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Configuration()
        {
            var model = new ConfigurationViewModel();            
            model.SmtpServer = configurationProvider[ConfigurationKeys.SmtpServer];
            model.SmtpPort = Convert.ToInt16(configurationProvider[ConfigurationKeys.SmtpPort]);
            model.SmtpAuthentication = Convert.ToBoolean(configurationProvider[ConfigurationKeys.SmtpAuthentication]);
            model.SmtpUserName = configurationProvider[ConfigurationKeys.SmtpUserName];
            model.SmtpPassword = configurationProvider[ConfigurationKeys.SmtpPassword];
            model.SmtpSsl = Convert.ToBoolean(configurationProvider[ConfigurationKeys.SmtpSsl]);
            model.SystemSenderName = configurationProvider[ConfigurationKeys.SystemSenderName];
            model.SystemSenderAddress = configurationProvider[ConfigurationKeys.SystemSenderAddress];
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Configuration(FormCollection form, ConfigurationViewModel model)
        {
            ViewBag.Success = false;
            if (ModelState.IsValid)
            {
                try
                {
                    configurationProvider.UpdateConfigurations(model);
                    ViewBag.Success = true;
                }
                catch (Exception ex)
                {
                    HandleException(ex);                    
                }
            }
            return View(model);
        }

        public ActionResult CleanUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CleanUp(FormCollection form)
        {
            string[] productImages = productProvider.GetProductImages();
            string[] showcaseImages = showcaseProvider.GetShowcaseImages();
            var imageFilesNames = productImages.Concat(showcaseImages).ToList();
            ViewBag.TotalFiles = 0;
            await Task.Run(() =>
            {
                var files1 = Directory.EnumerateFiles(
                    Server.MapPath("~/Media/Products")).Select(file => new FileInfo(file).Name)
                    .Except(imageFilesNames).ToList();
                files1.ForEach(System.IO.File.Delete);

                var files2 = Directory.EnumerateFiles(
                    Server.MapPath("~/Media/Ads")).Select(file => new FileInfo(file).Name)
                    .Except(imageFilesNames).ToList();
                files2.ForEach(System.IO.File.Delete);

                ViewBag.TotalFiles = files1.Count + files2.Count;
            });
            
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> TestSendEmail()
        {
            string testMailDestination = configurationProvider[ConfigurationKeys.SystemSenderAddress];
            try
            {
                await mailer.SendMail(testMailDestination, null, "Belanja Bunga: Test Send Email",
                    "This is only a test mail");
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
            return Json("Success");
        }
    }
}