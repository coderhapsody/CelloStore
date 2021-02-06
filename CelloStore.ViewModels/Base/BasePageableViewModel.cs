namespace CelloStore.ViewModels.Base
{
    public abstract class BasePageableViewModel
    {
        protected BasePageableViewModel()
        {
            PageSize = 10;
        }

        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}
