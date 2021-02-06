using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Administration.Person;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using log4net.Repository.Hierarchy;
using RazorEngine;
using CelloStore.ViewModels;

namespace CelloStore.FrontEnd.Areas.Administration.Controllers
{
    public class PersonController : AdminController
    {
        private readonly PersonProvider personProvider;
        private readonly RoleProvider roleProvider;

        public PersonController(PersonProvider personProvider, RoleProvider roleProvider)
        {
            this.personProvider = personProvider;
            this.roleProvider = roleProvider;

            
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var query = personProvider.List();
            return Json(query.ToDataSourceResult(request));
        }

        public ActionResult ListRole([DataSourceRequest] DataSourceRequest request)
        {
            var query = roleProvider.GetRoles();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            try
            {
                if (form["btnDelete"] != null)
                {
                    var personValues = form.GetValues("chkDelete");
                    if (personValues != null)
                    {
                        foreach (var personValue in personValues)
                        {
                            var personId = Convert.ToInt32(personValue);
                            personProvider.DeletePerson(personId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Edit(int id)
        {
            var model = new EditViewModel();
            var person = personProvider.GetPerson(id);
            AutoMapper.Mapper.DynamicMap(person, model);
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Edit(FormCollection form, EditViewModel model)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ResetPassword(FormCollection form)
        {
            try
            {
                int personId = Convert.ToInt32(form["PersonId"]);
                string email = form["Email"];
                personProvider.ResetPassword(personId, email);
                return Json(new AjaxViewModel
                {
                    IsSuccess = true,
                    Message = String.Format("New password has been sent to {0}", email)
                });
            }
            catch (Exception ex)
            {
                return Json(new AjaxViewModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }
    }
}
