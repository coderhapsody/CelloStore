using System.Web;
using System.Web.Mvc;
using CelloStore.FrontEnd.Attributes;

namespace CelloStore.FrontEnd
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CelloStoreHandleErrorAttribute());            
        }
    }
}
