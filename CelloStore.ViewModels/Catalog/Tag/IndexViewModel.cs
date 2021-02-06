using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Catalog.Tag
{
    public class IndexViewModel : BasePageableViewModel
    {
        public string Search { get; set; }
        public IEnumerable<CelloStore.Data.Tag> List { get; set; }        
    }
}
