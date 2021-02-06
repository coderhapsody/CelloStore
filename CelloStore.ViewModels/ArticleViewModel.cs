using System;
using CelloStore.Data;
using CelloStore.ViewModels.Base;

namespace CelloStore.ViewModels
{
    public class ArticleViewModel : BaseRowDataViewModel
    {        
        public string UrlToken { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
