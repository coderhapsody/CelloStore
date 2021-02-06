using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CelloStore.Providers;

namespace CelloStore.FrontEnd.Base
{
    public class AdminController : BaseController
    {
        #region Overrides of Controller

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!filterContext.IsChildAction)
            {
                if(!HasBackendAccess)
                {
                    filterContext.Result = new HttpStatusCodeResult(401, "String description here if you want");
                    Redirect(FormsAuthentication.DefaultUrl);                    
                }                
                ViewBag.VersionNumber = ConfigurationInstance[ConfigurationKeys.ApplicationVersion];
            }
        }

        #endregion
    }
}