using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Page.EmailTemplate
{
    public class IndexViewModel : BasePageableViewModel
    {
        public IEnumerable<ListEmailTemplateViewModel> List { get; set; }        
    }
}
