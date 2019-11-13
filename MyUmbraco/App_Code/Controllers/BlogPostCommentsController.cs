using System.Web.Mvc;
using Umbraco.Core.Scoping;
using Umbraco.Web.Mvc;
using MyUmbraco.Repositories;
using MyUmbraco.ViewModels;

namespace MyUmbraco.Controllers
{
  public class BlogPostCommentsController : SurfaceController
  {
    private readonly BlogPostCommentsRepository _blogPostCommentsRepository;

    public BlogPostCommentsController(IScopeProvider context)
    {
      _blogPostCommentsRepository = new BlogPostCommentsRepository(context);
    }

    public ActionResult Index(int blogPostId)
    {
      var comments = _blogPostCommentsRepository.GetBlogPostComments(blogPostId);

      return PartialView("/Views/Partials/_BlogPostComments.cshtml", comments);
    }

    public void PostComment(int blogPostId, int memberId, string comment)
    {
      _blogPostCommentsRepository.PostComment(blogPostId, memberId, comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateUmbracoFormRouteString]
    public ActionResult HandleSubmitComment(BlogPostCommentViewModel model)
    {
      if (!ModelState.IsValid)
        return CurrentUmbracoPage();

      var member = Members.GetCurrentMember();

      PostComment(model.BlogPostId, member.Id, model.Comment);

      return RedirectToCurrentUmbracoPage();
    }
  }
}