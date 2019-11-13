using System.Web.Mvc;
using MyUmbraco.Repositories;
using MyUmbraco.ViewModels;
using Umbraco.Core.Scoping;
using Umbraco.Web.Mvc;

namespace MyUmbraco.Controllers
{
  public class BlogPostHighFiveController : SurfaceController
  {
    private readonly BlogPostHighFiveRepository _BlogPostHighFiveService;
    public const string PartialViewFolder = "~/Views/Partials/";

    public BlogPostHighFiveController(IScopeProvider scopeProvider)
    {
      _BlogPostHighFiveService = new BlogPostHighFiveRepository(scopeProvider);
    }

    // GET
    public ActionResult Index()
    {
      return PartialView(PartialViewFolder + "_BlogPostHighFive.cshtml");
    }

    [ChildActionOnly]
    public int HandleGetHighFiveCount(int blogPostId)
    {
      return _BlogPostHighFiveService.GetHighFiveCount(blogPostId);
    }

    [ChildActionOnly]
    public bool HandleUserHasHighFived(int blogPostId, int memberId)
    {
      return _BlogPostHighFiveService.MemberHasHighFived(blogPostId, memberId);
    }

    [HttpPost]
    public ActionResult HandleHighFiveComment(BlogPostHighFiveViewModel model)
    {
      _BlogPostHighFiveService.HighFiveComment(model);

      return PartialView(PartialViewFolder + "_BlogPostHighFive.cshtml", model);
    }
  }
}