using System.Web.Mvc;
using log4net;

namespace CelloStore.FrontEnd.Controllers
{
    [AllowAnonymous]
    public class ErrorsController : Controller
    {               
        public ActionResult ResourceNotFound()
        {
            var logger = LogManager.GetLogger(this.GetType());
            logger.Error("Resource Not Found: " + Request["aspxerrorpath"]);
            return View();
        }
    }
}