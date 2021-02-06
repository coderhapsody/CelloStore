using System;
using System.Web.Mvc;
using AutoMapper;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Administration.Role;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CelloStore.FrontEnd.Areas.Administration.Controllers
{
    public class RoleController : AdminController
    {
        private readonly RoleProvider roleProvider;
        public RoleController(RoleProvider roleProvider)
        {
            this.roleProvider = roleProvider;
        }

        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var query = roleProvider.List();
            return Json(query.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            try
            {
                if (form["btnDelete"] != null)
                {
                    var roleValues = form.GetValues("chkDelete");
                    if (roleValues != null)
                    {
                        foreach (var roleValue in roleValues)
                        {
                            var roleId = Convert.ToInt32(roleValue);
                            roleProvider.DeleteRole(roleId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                Logger.ErrorException(ex.Message, ex);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Create()
        {
            var model = new CreateEditViewModel();
            model.Id = 0;
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Create(FormCollection form, CreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var role = new Role();
                    Mapper.DynamicMap(model, role);
                    roleProvider.AddOrUpdateRole(role);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                    Logger.ErrorException(ex.Message, ex);
                }
            }
            return RedirectToAction("Create");
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Edit(int id)
        {
            var model = new CreateEditViewModel();
            var role = roleProvider.GetRole(id);
            Mapper.DynamicMap(role, model);
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Edit(FormCollection form, CreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var role = roleProvider.GetRole(model.Id);
                    Mapper.DynamicMap(model, role);
                    roleProvider.AddOrUpdateRole(role);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                    Logger.ErrorException(ex.Message, ex);
                }
            }

            return RedirectToAction("Edit");
        }
    }
}