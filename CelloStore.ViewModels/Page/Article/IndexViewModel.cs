using System.Collections.Generic;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Page.Article
{
    public class IndexViewModel : BasePageableViewModel
    {
        public IEnumerable<ListArticleViewModel> List { get; set; }        
    }
}
