using MyUmbraco.Models;
using MyUmbraco.ViewModels;
using Umbraco.Core.Scoping;

namespace MyUmbraco.Repositories
{
  public class BlogPostHighFiveRepository
  {
    private readonly IScopeProvider _scopeProvider;

    public BlogPostHighFiveRepository(IScopeProvider scopeProvider)
    {
      _scopeProvider = scopeProvider;
    }

    public int GetHighFiveCount(int BlogPostId)
    {
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
        return scope.Database.Query<BlogPostHighFive>().Count(x => x.BlogPostId == BlogPostId);
      }
    }

    public bool MemberHasHighFived(int BlogPostId, int MemberId)
    {
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
        return scope.Database.Query<BlogPostHighFive>().Count(x => x.BlogPostId == BlogPostId && x.MemberId == MemberId) > 0;
      }
    }

    public void HighFiveComment(BlogPostHighFiveViewModel model)
    {
      var memberHasLiked = MemberHasHighFived(model.BlogPostId, model.MemberId);
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
        if (!memberHasLiked)
        {
          scope.Database.Insert(new BlogPostHighFive { BlogPostId = model.BlogPostId, MemberId = model.MemberId });
        }
        else
        {
          scope.Database.Delete<BlogPostHighFive>($"WHERE BlogPostId = {model.BlogPostId} AND MemberId = {model.MemberId}");
        }
      }
    }
  }
}