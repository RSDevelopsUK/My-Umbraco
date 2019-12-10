using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyUmbraco.ViewModels;
using NPoco;
using NPoco.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Scoping;

namespace MyUmbraco.Services
{
  public class RegisterBlogPostCommentsServiceComposer : IUserComposer
  {
    public void Compose(Composition composition)
    {
      composition.Register<IBlogPostCommentsService, BlogPostCommentsService>(Lifetime.Request);
    }
  }

  public interface IBlogPostCommentsService
  {
    int GetBlogPostCommentCount(int blogPostId);
    List<BlogPostCommentViewModel> GetPagedBlogPostComments(int blogPostId, int skip, int take);
    void PostComment(int blogPostId, int memberId, string comment);
  }

  public class BlogPostCommentsService : IBlogPostCommentsService
  {
    private readonly IScopeProvider _scopeProvider;

    public BlogPostCommentsService(IScopeProvider scopeProvider)
    {
      _scopeProvider = scopeProvider;
    }

    public int GetBlogPostCommentCount(int blogPostId)
    {
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
        return scope.Database.Query<BlogPostCommentViewModel>()
          .Where(x => x.BlogPostId == blogPostId)
          .Count();
      }
    }

    public List<BlogPostCommentViewModel> GetPagedBlogPostComments(int blogPostId, int skip, int take)
    {
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
        return scope.Database.Query<BlogPostCommentViewModel>()
          .Where(x => x.BlogPostId == blogPostId)
          .OrderByDescending(x => x.CommentedOn)
          .Limit(skip, take)
          .ToList();
      }
    }

    public void PostComment(int blogPostId, int memberId, string comment)
    {
      using (var scope = _scopeProvider.CreateScope(autoComplete: true))
      {
        var commentSafe = HttpUtility.HtmlEncode(comment);
        var sql = $"INSERT INTO BlogPostComments (BlogPostId, MemberId, Comment, CommentedOn) VALUES ({blogPostId}, {memberId}, '{commentSafe}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}')";

        scope.Database.Execute(sql);
      }
    }
  }
}