using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace MyUmbraco.Repositories
{
  public class BlogPostRepository
  {
    private readonly IUmbracoContextFactory _context;

    public BlogPostRepository(IUmbracoContextFactory context)
    {
      _context = context;
    }

    public int GetBlogPostCount()
    {
      using (var cref = _context.EnsureUmbracoContext())
      {
        var cache = cref.UmbracoContext.Content;
        var root = cache.GetAtRoot()
          .FirstOrDefault(x => x.ContentType.Alias.Equals("content"));

        if (root == null) return 0;
        {
          var blogs = root.Children.First(x => x.ContentType.Alias.Equals("blog"));
          return blogs.Children.Count(x => x.IsPublished());
        }
      }
    }

    public IPublishedContent GetMostRecentBlogPost()
    {
      using (var cref = _context.EnsureUmbracoContext())
      {
        var cache = cref.UmbracoContext.Content;
        var root = cache.GetAtRoot()
          .FirstOrDefault(x => x.ContentType.Alias.Equals("content"));

        if (root == null) return null;
        {
          var blogs = root.Children.First(x => x.ContentType.Alias.Equals("blog"));
          return blogs.Children
            .Where(x => x.IsPublished())
            .OrderBy(x => x.CreateDate)
            .FirstOrDefault();
        }
      }
    }

    public IEnumerable<IPublishedContent> GetPagedBlogPosts(int skip, int take)
    {
      var list = Enumerable.Empty<IPublishedContent>();
      using (var cref = _context.EnsureUmbracoContext())
      {
        var cache = cref.UmbracoContext.Content;
        var root = cache.GetAtRoot()
          .FirstOrDefault(x => x.ContentType.Alias.Equals("content"));

        if (root == null) return list;
        {
          var blogs = root.Children.First(x => x.ContentType.Alias.Equals("blog"));
          list = blogs.Children
            .Where(x => x.IsPublished())
            .OrderByDescending(x => x.CreateDate)
            .Skip(skip)
            .Take(take)
            .ToList();

          return list;
        }
      }
    }

  }
}