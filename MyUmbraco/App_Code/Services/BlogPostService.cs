using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;
using MyUmbraco.ViewModels;
using Newtonsoft.Json.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace MyUmbraco.Services
{
  public class RegisterBlogPostServiceComposer : IUserComposer
  {
    public void Compose(Composition composition)
    {
      composition.Register<IBlogPostService, BlogPostService>(Lifetime.Request);
    }
  }

  public interface IBlogPostService
  {
    int GetBlogPostCount();
    IPublishedContent GetMostRecentBlogPost();
    IEnumerable<IPublishedContent> GetPagedBlogPosts(int skip, int take);
  }
  public class BlogPostService : IBlogPostService
  {
    private readonly IUmbracoContextFactory _context;
    private readonly Func<IPublishedContent, bool> _isBlog = x => x.ContentType.Alias.Equals("blog");

    public BlogPostService(IUmbracoContextFactory context)
    {
      _context = context;
    }
    public int GetBlogPostCount()
    {
      using (var cref = _context.EnsureUmbracoContext())
      {
        var cache = cref.UmbracoContext.Content;
        var blogs = cache.GetAtRoot().FirstOrDefault().Children().First(_isBlog).Children;

        return blogs?.Count(x => x.IsPublished()) ?? 0;
      }
    }

    public IPublishedContent GetMostRecentBlogPost()
    {
      using (var cref = _context.EnsureUmbracoContext())
      {
        var cache = cref.UmbracoContext.Content;
        var blogs = cache.GetAtRoot().FirstOrDefault().Children().First(_isBlog).Children;

        return blogs?.Where(x => x.IsPublished()).OrderBy(x => x.CreateDate).FirstOrDefault();
      }
    }

    public IEnumerable<IPublishedContent> GetPagedBlogPosts(int skip, int take)
    {
      var list = Enumerable.Empty<IPublishedContent>();

      using (var cref = _context.EnsureUmbracoContext())
      {
        var cache = cref.UmbracoContext.Content;
        var blogs = cache.GetAtRoot().FirstOrDefault().Children().First(_isBlog).Children();

        return blogs?.Where(x => x.IsPublished())
                 .OrderByDescending(x => x.CreateDate)
                 .Skip(skip)
                 .Take(take)
                 .ToList() 
               ?? list;
      }
    }
  }
}