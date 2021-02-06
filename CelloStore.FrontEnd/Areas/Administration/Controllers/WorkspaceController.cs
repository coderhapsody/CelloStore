using System;
using System.Linq;
using System.Web.Mvc;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using RazorEngine;
using RazorEngine.Templating;

namespace CelloStore.FrontEnd.Areas.Administration.Controllers
{
    public class WorkspaceController : AdminController
    {
        private readonly SecurityProvider securityProvider;
        public WorkspaceController(SecurityProvider securityProvider)
        {
            this.securityProvider = securityProvider;
        }

        public ActionResult Dashboard()
        {
            //var template = "Hello @Model.Name, Current date is @DateTime.Now.ToLongDateString()";
            //var result = Engine.Razor.RunCompile(template, "templateKey", null, new { Name = "World" });
            return View();
        }

        
    }
}