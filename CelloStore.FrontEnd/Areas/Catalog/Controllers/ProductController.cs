using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CelloStore.Data;
using CelloStore.FrontEnd.Attributes;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels.Catalog.Product;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CelloStore.FrontEnd.Areas.Catalog.Controllers
{
    [Authorize]
    public class ProductController : AdminController
    {
        private readonly ProductProvider productProvider;
        private readonly CategoryProvider categoryProvider;
        private readonly TagProvider tagProvider;
        private readonly AreaProvider areaProvider;
        public ProductController(ProductProvider productProvider, CategoryProvider categoryProvider, TagProvider tagProvider, AreaProvider areaProvider)
        {
            this.productProvider = productProvider;
            this.categoryProvider = categoryProvider;
            this.tagProvider = tagProvider;
            this.areaProvider = areaProvider;
        }

        [ImportModelStateFromTempData]
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Index(FormCollection form)
        {
            try
            {
                if(form["btnDelete"] != null)
                {
                    var productValues = form.GetValues("chkDelete");
                    if(productValues != null)
                    {
                        foreach(var productValue in productValues)
                        {
                            var productId = Convert.ToInt32(productValue);
                            productProvider.DeleteProduct(productId);
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

        [HttpPost]
        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            var queryProducts = productProvider.ListProducts();
            return Json(queryProducts.ToDataSourceResult(request));
        }

        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Create()
        {
            var model = new CreateEditViewModel();
            model.Id = 0;
            model.IsActive = true;
            model.UnitPrice = 0;
            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection form, CreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = new Product();
                    product.Name = model.Name;
                    product.Code = model.Code;
                    product.Description = Server.HtmlDecode(model.Description);
                    product.UnitPrice = model.UnitPrice;
                    product.IsActive = model.IsActive;
                    product.IsFeatured = model.IsFeatured;
                    product.Photo = model.Photo;

                    var selectedTags = form.GetValues("selectedTags");
                    var selectedAreas = form.GetValues("selectedAreas");
                    var selectedCategories = form.GetValues("selectedCategories");
                    productProvider.AddOrUpdateProduct(product, selectedTags, selectedAreas, selectedCategories);
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
            var product = productProvider.GetProduct(id);
            model.Id = product.Id;
            model.Name = product.Name;
            model.Description = product.Description;
            model.Code = product.Code;
            model.IsActive = product.IsActive;
            model.IsFeatured = product.IsFeatured;
            model.UnitPrice = product.UnitPrice;
            model.Photo = product.Photo;
            ViewBag.selectedTags = product.Tags.Select(tag => tag.Name).ToArray();
            ViewBag.selectedAreas = product.Areas.Select(area => area.Name).ToArray();
            ViewBag.selectedCategories = product.Categories.Select(cat => cat.Name).ToArray();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ExportModelStateToTempData]
        public ActionResult Edit(FormCollection form, CreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = productProvider.GetProduct(model.Id);
                    product.Name = model.Name;
                    product.Code = model.Code;
                    product.Description = Server.HtmlDecode(model.Description);
                    product.UnitPrice = model.UnitPrice;
                    product.IsActive = model.IsActive;
                    product.IsFeatured = model.IsFeatured;
                    product.Photo = model.Photo;

                    var tags = form.GetValues("selectedTags");
                    var areas = form.GetValues("selectedAreas");
                    var categories = form.GetValues("selectedCategories");
                    productProvider.AddOrUpdateProduct(product, tags, areas, categories);


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

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Media/Products"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Media/Products"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        [HttpGet]
        public ActionResult GetTags()
        {
            return Json(tagProvider.GetTags(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAreas()
        {
            return Json(areaProvider.GetAreas(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCategories()
        {
            return Json(categoryProvider.GetSelectableCategories(), JsonRequestBehavior.AllowGet);
        }

    }
}