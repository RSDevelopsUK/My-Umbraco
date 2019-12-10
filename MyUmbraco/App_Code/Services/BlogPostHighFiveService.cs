using MyUmbraco.Models;
using MyUmbraco.ViewModels;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Scoping;

namespace MyUmbraco.Services
{
  public class RegisterBlogPostHighFiveServiceComposer : IUserComposer
  {
    public void Compose(Composition composition)
    {
      composition.Register<IBlogPostHighFiveService, BlogPostHighFiveService>(Lifetime.Singleton);
    }
  }

  public interface IBlogPostHighFiveService
  {
    int GetHighFiveCount(int blogPostId);
    bool MemberHasHighFived(int blogPostId, int memberId);
    void HighFiveComment(BlogPostHighFiveViewModel model);
  }

  public class BlogPostHighFiveService : IBlogPostHighFiveService
  {

    private readonly IScopeProvider _scopeProvider;

    public BlogPostHighFiveService(IScopeProvider scopeProvider)
    {
      _scopeProvider = scopeProvider;
    }

    public int GetHighFiveCount(int blogPostId)
    {
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
        return scope.Database.Query<BlogPostHighFive>().Count(x => x.BlogPostId == blogPostId);
      }
    }

    public bool MemberHasHighFived(int blogPostId, int memberId)
    {
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
        return scope.Database.Query<BlogPostHighFive>().Count(x => x.BlogPostId == blogPostId && x.MemberId == memberId) > 0;
      }
    }

    public void HighFiveComment(BlogPostHighFiveViewModel model)
    {
      var memberHasLiked = MemberHasHighFived(model.BlogPostId, model.MemberId);
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
        if (!memberHasLiked)
        {
          scope.Database.Insert(new BlogPostHighFive {BlogPostId = model.BlogPostId, MemberId = model.MemberId});
        }
        else
        {
          scope.Database.Delete<BlogPostHighFive>(
            $"WHERE BlogPostId = {model.BlogPostId} AND MemberId = {model.MemberId}");
        }
      }
    }
  }
}