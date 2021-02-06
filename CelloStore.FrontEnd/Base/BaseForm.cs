using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using CelloStore.Data;
using Ninject;
using Ninject.Web.Common;

namespace CelloStore.FrontEnd.Base
{
    public class BaseForm : Page
    {
        public IKernel Kernel { get; private set; }

        public BaseForm()
        {
            Kernel = new StandardKernel();
            try
            {
                Kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                Kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                Kernel.Bind<EntitiesModel>()
                  .ToSelf()
                  .InRequestScope()
                  .WithConstructorArgument("connection",
                      ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                Kernel.Bind<IPrincipal>().ToMethod(ctx => HttpContext.Current.User ?? null);

                log4net.Config.XmlConfigurator.Configure();                
            }
            catch
            {
                Kernel.Dispose();
                throw;
            }
            
        }
        public void Page_Init() { Kernel.Inject(this); }
    }
}