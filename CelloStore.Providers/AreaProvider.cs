using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;
using CelloStore.ViewModels.Catalog.Area;

namespace CelloStore.Providers
{
    public class AreaProvider : BaseProvider
    {
        public AreaProvider(EntitiesModel context, IPrincipal principal) : base(context, principal)
        {
        }

        public Area DefaultArea
        {
            get
            {
                return GetArea(1);    
            }            
        }

        public void AddOrUpdateArea(Area area)
        {
            if(area.Id == 0)
                context.Add(area);
            else
            {
                context.AttachCopy(area);
            }

            SetAuditFields(area);
            context.SaveChanges();
        }

        public IEnumerable<Area> GetAreas(bool activeOnly = false)
        {
            var queryArea = context.Areas;
            if (activeOnly)
            {
                queryArea = queryArea.Where(area => area.IsActive);
            }

            return context.CreateDetachedCopy(queryArea.ToList());
        }
        

        public IEnumerable<Area> ListAreas(int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.Areas.Where(tag => tag.Name.Contains(search));            
            
            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IQueryable<ListAreaViewModel> List()
        {
            var query = from area in context.Areas
                        select new ListAreaViewModel
                        {
                            Id = area.Id,
                            Name = area.Name,
                            IsActive = area.IsActive
                        };
            return query;
        }

        public Area GetArea(int id)
        {
            return context.CreateDetachedCopy(context.Areas.SingleOrDefault(cat => cat.Id == id));
        }

        public Area GetAreaByName(string areaName)
        {
            return context.Areas.FirstOrDefault(area => area.Name == areaName);
        }

        public void DeleteArea(int id)
        {
            var area = context.Areas.Single(a => a.Id == id);
            if (area != null)
            {
                context.Delete(area);
                context.SaveChanges();
            }
        }

        public bool IsActive(string areaName)
        {
            return context.Areas.Any(area => area.Name == areaName && area.IsActive);
        }
    }
}
