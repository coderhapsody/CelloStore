using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Catalog.Area
{
    public class IndexViewModel : BasePageableViewModel
    {
        public IEnumerable<CelloStore.Data.Area> List { get; set; }        
    }
}
