using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Page.Showcase
{
    public class IndexViewModel : BasePageableViewModel
    {
        public IEnumerable<ListShowcaseViewModel> List { get; set; }        
    }
}
