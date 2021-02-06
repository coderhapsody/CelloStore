using System.Collections.Generic;
using CelloStore.Data;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        public Category RootCategory { get; set; }
        public IEnumerable<Category> ChildCategories { get; set; }
    }
}
