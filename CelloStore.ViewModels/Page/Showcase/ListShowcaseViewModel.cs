using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Page.Showcase
{
    public class ListShowcaseViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool First { get; set; }
        public string ExternalLinkType { get; set; }
    }
}
