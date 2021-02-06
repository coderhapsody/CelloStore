using System.Web.Mvc;

namespace CelloStore.FrontEnd.Attributes
{
    public abstract class ModelStateTempDataTransferAttribute : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(ModelStateTempDataTransferAttribute).FullName;
    }
}