using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;
using CelloStore.ViewModels.Catalog.Category;

namespace CelloStore.Providers
{
    public class CategoryProvider : BaseProvider
    {
        public CategoryProvider(EntitiesModel context, IPrincipal principal)
            : base(context, principal)
        {
        }

        public void AddOrUpdateCategory(Category category)
        {
            if (category.Id == 0)
                context.Add(category);
            SetAuditFields(category);
            context.SaveChanges();
        }

        public IEnumerable<Category> GetParentCategories()
        {
            return context.Categories.Where(cat => !cat.ParentCategoryId.HasValue).ToList();
        }

        public IEnumerable<Category> GetSelectableCategories()
        {
            return context.CreateDetachedCopy(context.Categories.Where(cat => cat.ParentCategoryId.HasValue && cat.IsActive).ToList());
        }

        public IEnumerable<Category> GetCategories()
        {
            return context.Categories.ToList();
        }

        public IEnumerable<Category> GetCategories(int? parentCategoryId)
        {
            var categories = context.Categories.Where(cat => cat.ParentCategoryId == parentCategoryId && cat.IsActive);
            return categories.ToList();
        }

        public IEnumerable<Category> ListCategories(int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.Categories.Where(cat => cat.Name.Contains(search));

            if(!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1)*pageSize).Take(pageSize).ToList();
        }

        public IQueryable<ListCategoryViewModel> List()
        {
            var query = from cat in context.Categories
                        join catParent in context.Categories on cat.ParentCategoryId equals catParent.Id into g
                        from cat2 in g.DefaultIfEmpty()
                        select new ListCategoryViewModel
                        {
                            Id = cat.Id,
                            Name = cat.Name,
                            ParentCategory = cat2.Name,
                            IsActive = cat.IsActive
                        };
            return query;
        }

        public Category GetCategory(int id)
        {
            return context.Categories.SingleOrDefault(cat => cat.Id == id);
        }

        public void DeleteCategory(int id)
        {
            var category = GetCategory(id);
            if (category != null)
            {
                context.Delete(category);
                context.SaveChanges();
            }
        }

        public Category GetCategoryByName(string categoryName, bool ignoreSpace = false)
        {
            if(ignoreSpace)
                return context.Categories.FirstOrDefault(cat => cat.Name.Replace(" ", "") == categoryName);
            return context.Categories.FirstOrDefault(cat => cat.Name == categoryName);
        }
    }
}
