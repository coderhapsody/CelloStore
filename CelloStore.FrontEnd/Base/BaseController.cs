using System;
using System.Configuration;
using System.Web.Mvc;
using CelloStore.Providers;
using Ninject;
using Ninject.Extensions.Logging;

namespace CelloStore.FrontEnd.Base
{
    public class BaseController : Controller
    {
        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public ConfigurationProvider ConfigurationInstance { get; set; }

        [Inject]
        public SecurityProvider SecurityService { get; set; }

        public string CryptographyKey
        {
            get { return ConfigurationManager.AppSettings["CryptographyKey"]; }
        }

        public string CurrentUserName
        {
            get { return User.Identity.Name; }
        }

        public bool HasBackendAccess
        {
            get
            {
                return SecurityService.IsAdministrator(CurrentUserName);                 
            }
        }

        protected virtual void HandleException(Exception ex)
        {
            ModelState.AddModelError(String.Empty, ex.Message);
            Logger.ErrorException(ex.Message, ex);
        }
    }
}