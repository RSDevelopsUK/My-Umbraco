using System.Collections.Generic;
using MyUmbraco.Models;
using MyUmbraco.ViewModels;
using Umbraco.Core.Scoping;

namespace MyUmbraco.Repositories
{
  public class BlogPostCommentsRepository
  {
    private readonly IScopeProvider _scopeProvider;

    public BlogPostCommentsRepository(IScopeProvider scopeProvider)
    {
      _scopeProvider = scopeProvider;
    }

    public List<BlogPostCommentViewModel> GetBlogPostComments(int blogPostId)
    {
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
       return scope.Database.Query<BlogPostCommentViewModel>().Where(x => x.BlogPostId == blogPostId).ToList();
      }
    }

    public void PostComment(int blogPostId, int memberId, string comment)
    {
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
        var sql = $"INSERT INTO BlogPostComments (BlogPostId, MemberId, Comment) VALUES ({blogPostId}, {memberId}, '{comment}')";

        scope.Database.Execute(sql);
      }
    }
  }
}