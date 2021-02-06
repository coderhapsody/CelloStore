using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;

namespace CelloStore.Providers
{
    public class TagProvider : BaseProvider
    {
        public TagProvider(EntitiesModel context, IPrincipal principal) : base(context, principal)
        {
        }

        public void AddOrUpdateTag(Tag tag)
        {
            if (tag.Id == 0)
                context.Add(tag);

            SetAuditFields(tag);
            context.SaveChanges();
        }

        public IEnumerable<Tag> GetTags()
        {            
            return context.CreateDetachedCopy(context.Tags.ToList());
        }

        public IEnumerable<Tag> ListTags(int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.Tags.Where(tag => tag.Name.Contains(search));

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public Tag GetTag(int id)
        {
            return context.Tags.SingleOrDefault(cat => cat.Id == id);
        }

        public void DeleteTag(int id)
        {
            var tag = GetTag(id);
            if (tag != null)
            {
                context.Delete(tag);
                context.SaveChanges();
            }
        }

        public Tag GetTagByName(string value)
        {
            return context.Tags.FirstOrDefault(tag => tag.Name == value);
        }
    }
}
