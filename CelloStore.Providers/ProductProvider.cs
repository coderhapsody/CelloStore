using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CelloStore.Data;
using CelloStore.Utilities.Extensions.Linq;
using CelloStore.ViewModels;
using CelloStore.ViewModels.Catalog.Product;
using Telerik.OpenAccess.FetchOptimization;

namespace CelloStore.Providers
{    

    public class ProductProvider : BaseProvider
    {
        private readonly TagProvider tagProvider;
        private readonly AreaProvider areaProvider;
        private readonly CategoryProvider categoryProvider;

        public static readonly string QueryKeyCategory = "cat";
        public static readonly string QueryKeyTag = "tag";
        public static readonly string QueryKeyArea = "area";

        public ProductProvider(EntitiesModel context, IPrincipal principal, TagProvider tagProvider, AreaProvider areaProvider, CategoryProvider categoryProvider) 
            : base(context, principal)
        {
            this.tagProvider = tagProvider;
            this.areaProvider = areaProvider;
            this.categoryProvider = categoryProvider;
        }

        

        public void AddOrUpdateProduct(Product product, string[] tagNames, string[] areaNames, string[] categoryNames)
        {
            context.GetScope().TransactionProperties.ReadAfterDelete = true;
            
            if (product.Id == 0)
                context.Add(product);


            product.Tags.Clear();
            if(tagNames != null)
            {
                foreach(var tagName in tagNames)
                {
                    var tag = tagProvider.GetTagByName(tagName);
                    if(tag != null)
                    {
                        product.Tags.Add(tag);
                    }
                }
            }

            product.Areas.Clear();
            if(areaNames != null)
            {
                foreach(var areaName in areaNames)
                {
                    var area = areaProvider.GetAreaByName(areaName);
                    if (area != null)
                    {
                        product.Areas.Add(area);
                    }
                }
            }

            product.Categories.Clear();
            if(categoryNames != null)
            {
                foreach(var categoryName in categoryNames)
                {
                    var category = categoryProvider.GetCategoryByName(categoryName);
                    if(category != null)
                    {
                        product.Categories.Add(category);
                    }

                }
            }

            SetAuditFields(product);
            context.SaveChanges();
        }

        public IEnumerable<Product> GetPagedProducts(string areaName, string categoryName, int pageIndex, int pageSize, out int rowCount)
        {
            IQueryable<Product> queryProducts = context.Products.Where(p => p.IsActive);

            queryProducts = from p in queryProducts
                            from area in p.Areas
                            where area.Name.Replace(" ", "") == areaName.Replace(" ","")
                            select p;

            if(!String.IsNullOrEmpty(categoryName))
            {
                queryProducts = from p in queryProducts
                                from cat in p.Categories
                                where cat.Name.Replace(" ", "") == categoryName.Replace(" ","")
                                select p;
            }
            else
            {
                queryProducts = from p in queryProducts
                                    where p.IsFeatured
                                    select p;                
            }

            //if(tagId.HasValue)
            //{
            //    queryProducts = from p in queryProducts
            //                    from tag in p.Tags
            //                    where tag.Id == tagId.Value
            //                    select p;
            //}

            rowCount = queryProducts.Count();
            return queryProducts.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetProducts()
        {
            return context.Products.ToList();
        }

        public IEnumerable<Product> ListProducts(int pageIndex, int pageSize, string search, string sortExpression, out int rowCount)
        {
            var query = context.Products.Where(product => product.Name.Contains(search) || product.Code.Contains(search));

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            rowCount = query.Count();
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IQueryable<ListProductViewModel> ListProducts()
        {
            var query = from product in context.Products
                        select new ListProductViewModel
                        {
                            Id = product.Id,
                            Code = product.Code,
                            Name = product.Name,
                            UnitPrice = product.UnitPrice,
                            IsActive = product.IsActive
                        };
            return query;
        } 

        public Product GetProduct(int id)
        {
            return context.Products.SingleOrDefault(p => p.Id == id);
        }

        public Product GetProduct(string code)
        {
            return context.Products.FirstOrDefault(p => p.Code == code);
        }

        public void DeleteProduct(int id)
        {
            var product = context.Products.Single(prod => prod.Id == id);
            if (product != null)
            {
                context.Delete(product);
                context.SaveChanges();
            }
        }

        public Product GetProductDetail(string areaName, int id)
        {
            var query = from p in context.Products
                        from area in p.Areas
                        where p.Id == id && area.Name.Replace(" ", "") == areaName.Replace(" ","") && p.IsActive
                        select p;
            return query.SingleOrDefault();
        }

        public string[] GetProductImages()
        {
            return context.Products.Select(p => p.Photo).ToArray();
        }
    }
}
