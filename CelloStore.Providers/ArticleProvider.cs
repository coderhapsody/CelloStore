using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;
using CelloStore.ViewModels.Page.Article;

namespace CelloStore.Providers
{
    public class ArticleProvider : BaseProvider
    {
        public ArticleProvider(EntitiesModel context, IPrincipal principal) 
            : base(context, principal)
        {
        }

        public void AddOrUpdateArticle(Article article)
        {
            if (article.Id == 0)
                context.Add(article);

            SetAuditFields(article);
            context.SaveChanges();
        }

        public IEnumerable<Article> GetArticles()
        {
            return context.Articles.ToList();
        }

        public IEnumerable<Article> GetPublishedArticles()
        {
            return context.Articles.Where(article => article.IsPublished).ToList();
        }


        public IEnumerable<Article> ListArticles(int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.Articles.Where(article => article.Subject.Contains(search));

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IQueryable<ListArticleViewModel> List()
        {
            var query = from article in context.Articles
                        select new ListArticleViewModel
                        {
                            Id = article.Id,
                            Subject = article.Subject,
                            UrlToken = article.UrlToken,
                            ChangedBy = article.ChangedBy,
                            ChangedWhen = article.ChangedWhen
                        };
            return query;            
        }

        public Article GetArticle(int id)
        {
            return context.Articles.SingleOrDefault(cat => cat.Id == id);
        }

        public Article GetPublishedArticle(int id)
        {
            return context.Articles.SingleOrDefault(cat => cat.Id == id && cat.IsPublished);
        }

        public Article GetArticle(string token)
        {
            return context.Articles.FirstOrDefault(cat => cat.UrlToken == token);
        }

        public void DeleteArticle(int id)
        {
            var article = GetArticle(id);
            if (article != null)
            {
                context.Delete(article);
                context.SaveChanges();
            }
        }
    }
}
