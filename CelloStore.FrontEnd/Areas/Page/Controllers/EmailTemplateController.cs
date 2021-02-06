using System;
using System.Web.Mvc;
using AutoMapper;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Page.EmailTemplate;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace CelloStore.FrontEnd.Areas.Page.Controllers
{
    public class EmailTemplateController : AdminController
    {
        private EmailTemplateProvider emailTemplateProvider;

        public EmailTemplateController(EmailTemplateProvider emailTemplateProvider)
        {
            this.emailTemplateProvider = emailTemplateProvider;
        }

        [ImportModelStateFromTempData]
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var query = emailTemplateProvider.List();
            return Json(query.ToDataSourceResult(request));
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Edit(int id)
        {
            var model = new EditViewModel();
            var emailTemplate = emailTemplateProvider.GetEmailTemplate(id);
            Mapper.DynamicMap(emailTemplate, model);
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection form, EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var emailTemplate = emailTemplateProvider.GetEmailTemplate(model.Id);
                    Mapper.DynamicMap(model, emailTemplate);
                    emailTemplateProvider.UpdateEmailTemplate(emailTemplate);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }

            return RedirectToAction("Edit");
        }
    }
}