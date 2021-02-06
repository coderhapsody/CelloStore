using System;
using System.Collections.Generic;
using CelloStore.Data;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels
{
    public class ProductViewModel  : BasePageableViewModel
    {
        public Category CurrentCategory { get; set; }
        public Tag CurrentTag { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public int PageCount
        {
            get { return Convert.ToInt32(RowCount + PageSize-1) / PageSize; }
        }
    }
}
