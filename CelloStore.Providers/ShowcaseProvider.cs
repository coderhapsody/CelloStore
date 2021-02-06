using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;
using CelloStore.ViewModels.Page.Showcase;

namespace CelloStore.Providers
{
    public class ShowcaseProvider : BaseProvider
    {
        public IDictionary<int, string> extLinkTypes;   

        public ShowcaseProvider(EntitiesModel context, IPrincipal principal) 
            : base(context, principal)
        {
            extLinkTypes = new Dictionary<int, string> {{0, "Url"}, {1, "Article"}};
        }

        public void AddOrUpdateShowcase(Showcase showcase)
        {
            if (showcase.Id == 0)
                context.Add(showcase);

            SetAuditFields(showcase);
            context.SaveChanges();
        }

        public IEnumerable<Showcase> GetAdvertisements()
        {
            return context.Showcases.Where(s => s.IsActive).ToList();
        }

        public IEnumerable<Showcase> GetValidAdvertisements()
        {
            var query = from ad in context.Showcases
                        where ad.FromDate <= DateTime.Today && ad.ToDate >= DateTime.Today
                              && ad.IsActive
                        select ad;
            return query.ToList();
        } 

        public IEnumerable<Showcase> ListShowcases(int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.Showcases.Where(cat => cat.Description.Contains(search));

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IQueryable<ListShowcaseViewModel> List()
        {
            var query = from showcase in context.Showcases
                        join linkType in extLinkTypes on showcase.ExtLinkType equals linkType.Key
                        select new ListShowcaseViewModel
                        {
                            Id = showcase.Id,
                            Description = showcase.Description,
                            First = showcase.First,
                            IsActive = showcase.IsActive,
                            Title = showcase.Title,
                            ExternalLinkType = linkType.Value
                        };
            return query;
        }

        public Showcase GetShowcase(int id)
        {
            return context.Showcases.SingleOrDefault(cat => cat.Id == id);
        }

        public void DeleteShowcase(int id)
        {
            var ad = GetShowcase(id);
            if (ad != null)
            {
                context.Delete(ad);
                context.SaveChanges();
            }
        }

        public string[] GetShowcaseImages()
        {
            return
                context.Showcases.Select(s => s.ImagePath)
                    .Union(context.Showcases.Select(s => s.PriceImagePath))
                    .ToArray();
        }
    }
}
