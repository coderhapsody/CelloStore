using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Administration.Person
{
    public class IndexViewModel : BaseViewModel
    {
        public IList<ListPersonViewModel> List { get; set; }
    }
}
