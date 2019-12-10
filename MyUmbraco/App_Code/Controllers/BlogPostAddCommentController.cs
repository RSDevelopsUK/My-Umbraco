using System.Web.Mvc;
using MyUmbraco.DataAnnotations;
using MyUmbraco.Helpers;
using MyUmbraco.Services;
using MyUmbraco.ViewModels;
using Umbraco.Web.Mvc;

namespace MyUmbraco.Controllers
{
  public class BlogPostAddCommentController : SurfaceController
  {
    private readonly IBlogPostCommentsService _blogPostCommentsRepository;

    public BlogPostAddCommentController(IBlogPostCommentsService blogPostCommentsService)
    {
      _blogPostCommentsRepository = blogPostCommentsService;
    }
    [HttpGet]
    public ActionResult Index(int blogPostId)
    {
      var model = new BlogPostAddCommentViewModel{BlogPostId = blogPostId };

      return PartialView("/Views/Partials/_BlogPostAddComment.cshtml", model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Submit(BlogPostAddCommentViewModel model)
    {
      if (!ModelState.IsValid)
        return CurrentUmbracoPage();

      if (!CaptchaHelper.ReCaptchaPassed(Request.Form["captcha"]))
      {
        ModelState.AddModelError(string.Empty, "You failed the CAPTCHA.");
        return CurrentUmbracoPage();
      }

      var member = Members.GetCurrentMember();

      _blogPostCommentsRepository.PostComment(model.BlogPostId, member.Id, model.Comment);

      return RedirectToCurrentUmbracoPage();
    }
  }
}