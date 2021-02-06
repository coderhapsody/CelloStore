using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CelloStore.ViewModels.Common;

namespace CelloStore.ViewModels.Page.Showcase
{
    public class CreateEditViewModel
    {
        public CreateEditViewModel()
        {
            ExtLinkTypes = new List<DropDownViewModel>
            {
                new DropDownViewModel() {Text = "Article", Value = "1"},
                new DropDownViewModel() {Text = "Url", Value = "0"}
            };
        }

        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]        
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public string ImagePath { get; set; }

        public string PriceImagePath { get; set; }

        public DateTime? FromDate { get; set; }
        public bool NoFromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool NoToDate { get; set; }

        public bool First { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Link Type")]
        public int ExtLinkType { get; set; }
        public IList<DropDownViewModel> ExtLinkTypes { get; set; }
        public string Url { get; set; }

        [Display(Name = "Article")]
        public int? ArticleId { get; set; }

    }
}
