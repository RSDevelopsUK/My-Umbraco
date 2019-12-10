using System.Web.Mvc;
using MyUmbraco.Services;
using MyUmbraco.ViewModels;
using Umbraco.Web.Mvc;

namespace MyUmbraco.Controllers
{
  public class BlogPostHighFiveController : SurfaceController
  {
    private readonly IBlogPostHighFiveService _blogPostHighFiveService;
    public const string PartialViewFolder = "~/Views/Partials/";

    public BlogPostHighFiveController(IBlogPostHighFiveService blogPostHighFiveService)
    {
      _blogPostHighFiveService = blogPostHighFiveService;
    }

    // GET
    public ActionResult Index()
    {
      return PartialView(PartialViewFolder + "_BlogPostHighFive.cshtml");
    }

    [ChildActionOnly]
    public int HandleGetHighFiveCount(int blogPostId)
    {
      return _blogPostHighFiveService.GetHighFiveCount(blogPostId);
    }

    [ChildActionOnly]
    public bool HandleUserHasHighFived(int blogPostId, int memberId)
    {
      return _blogPostHighFiveService.MemberHasHighFived(blogPostId, memberId);
    }

    [HttpPost]
    public ActionResult HandleHighFiveComment(BlogPostHighFiveViewModel model)
    {
      _blogPostHighFiveService.HighFiveComment(model);

      return PartialView(PartialViewFolder + "_BlogPostHighFive.cshtml", model);
    }
  }
}