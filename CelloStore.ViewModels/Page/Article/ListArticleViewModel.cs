using System;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels.Page.Article
{
    public class ListArticleViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string UrlToken { get; set; }
        public string Subject { get; set; }
        public bool IsPublished { get; set; }
        public DateTime ChangedWhen { get; set; }
        public string ChangedBy { get; set; }
    }
}
