using System.Web.Mvc;
using MyUmbraco.Repositories;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace MyUmbraco.Controllers
{
  public class BlogPostController : SurfaceController
  {
    private readonly BlogPostRepository _blogRepository;

    public BlogPostController(IUmbracoContextFactory context)
    {
      _blogRepository = new BlogPostRepository(context);
    }

    public int HandleGetBlogPostCount()
    {
      return _blogRepository.GetBlogPostCount();
    }

    public PartialViewResult HandleGetGetMostRecentBlogPost()
    {
      var featuredBlog = _blogRepository.GetMostRecentBlogPost();

      return PartialView("/Views/Partials/_FeaturedBlogPost.cshtml", featuredBlog);
    }

    public PartialViewResult HandleGetBlogs(int skip, int take, int row = 0)
    {
      var blogPosts = _blogRepository.GetPagedBlogPosts(skip, take);

      ModelState.Clear();
      ViewData["row"] = row + 1;

      return PartialView("/Views/Partials/_BlogPostsLanding.cshtml", blogPosts);
    }

  }
}