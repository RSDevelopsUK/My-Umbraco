using System.Web.Mvc;
using MyUmbraco.Services;
using Umbraco.Web.Mvc;

namespace MyUmbraco.Controllers
{
  public class BlogPostController : SurfaceController
  {
    private readonly IBlogPostService _blogPostService;

    public BlogPostController(IBlogPostService blogPostService)
    {
      _blogPostService = blogPostService;
    }

    public int HandleGetBlogPostCount()
    {
      return _blogPostService.GetBlogPostCount();
    }

    public PartialViewResult HandleGetGetMostRecentBlogPost()
    {
      var featuredBlog = _blogPostService.GetMostRecentBlogPost();

      return PartialView("/Views/Partials/_FeaturedBlogPost.cshtml", featuredBlog);
    }

    public PartialViewResult HandleGetBlogs(int skip, int take, int row = 0)
    {
      var blogPosts = _blogPostService.GetPagedBlogPosts(skip, take);

      ModelState.Clear();
      ViewData["row"] = row + 1;

      return PartialView("/Views/Partials/_BlogPostsLanding.cshtml", blogPosts);
    }

  }
}