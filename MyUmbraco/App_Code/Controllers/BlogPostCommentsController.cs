using System.Web.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Umbraco.Web.Mvc;
using MyUmbraco.ViewModels;
using MyUmbraco.Helpers;
using MyUmbraco.Services;
using Umbraco.Web;

namespace MyUmbraco.Controllers
{
  public class BlogPostCommentsController : SurfaceController
  {
    private readonly IBlogPostCommentsService _blogPostCommentsRepository;

    public BlogPostCommentsController(IBlogPostCommentsService blogPostCommentsService)
    {
      _blogPostCommentsRepository = blogPostCommentsService;
    }
    public int HandleGetBlogPostCommentCount(int blogPostId)
    {
      return _blogPostCommentsRepository.GetBlogPostCommentCount(blogPostId);
    }
    [HttpPost]
    public PartialViewResult HandleGetBlogPostComments(int blogPostId, int skip, int take)
    {
      var blogPostComments = _blogPostCommentsRepository.GetPagedBlogPostComments(blogPostId, skip, take);

      ModelState.Clear();

      return PartialView("/Views/Partials/_BlogPostComments.cshtml", blogPostComments);
    }

    [HttpGet]
    public ActionResult Index(int blogPostId, int skip, int take)
    {
      var comments = _blogPostCommentsRepository.GetPagedBlogPostComments(blogPostId, skip, take);
      
      return PartialView("/Views/Partials/_BlogPostComments.cshtml", comments);
    }
  }
}