using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using CelloStore.ViewModels;

namespace CelloStore.FrontEnd.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly ArticleProvider articleProvider;

        public ArticleController(ArticleProvider articleProvider)
        {
            this.articleProvider = articleProvider;
        }        

        public ActionResult ViewContent(string urlToken, int? id)
        {
            if(id.HasValue)
            {
                var articleById = articleProvider.GetPublishedArticle(id.Value);
                return RedirectToAction("ViewContent", new { urlToken = articleById.UrlToken });
            }

            var article = articleProvider.GetArticle(urlToken);
            var model = new ArticleViewModel();
            if(article != null)
            {
                model.Content = article.Content;
                model.IsPublished = article.IsPublished;
                model.Subject = article.Subject;
                model.Id = article.Id;
            }
            return View(model);
        }
    }
}